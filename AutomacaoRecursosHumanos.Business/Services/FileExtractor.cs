using AutomacaoRecursosHumanos.Business.Entities;
using AutomacaoRecursosHumanos.Business.Helper;
using AutomacaoRecursosHumanos.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AutomacaoRecursosHumanos.Business.Services;

public class FileExtractor : IFileExtractor
{
    private static readonly string _ExtensaoValida = ".csv";
    private static readonly string _ErroEstruturaDados = "Erro na estrutura de dados do arquivo.";
    private static readonly string _ErroNomeArquivo = "Você deve ajustar o nome do arquivo de acordo com o padrão: departamento-mes-ano. Ex: Departamento de TI-ABRIL-2022";
    private readonly ILogger<FileExtractor> _logger;

    public FileExtractor(ILogger<FileExtractor> logger)
    {
        _logger = logger;
    }

    public async Task<(string, string)> Extract(IEnumerable<IFormFile> files)
    {

        if (files is null || files.Count() == 0 || !ValidateExtensionFiles(files)) return (null, null);
        var (mesVigencia, anoVigencia) = GetMonthAndYear(files);

        var titulo = $"RelatorioPagamentos-{mesVigencia}-{anoVigencia}.json";

        _logger.LogInformation($"Iniciando a extração dos dados dos {files.Count()} arquivos.");

        var departments = new List<Department>();
        var errors = new List<Error>();

        Parallel.ForEach(files,
            async file =>
            {
                var (department, error) = await ExtractDatas(file);
                if (department != null)
                {
                    departments.Add(department);
                    return;
                }

                errors.Add(error);
            });

        var response = new Response { Departments = departments, Errors = errors };
        var dados = JsonSerializer.Serialize(response);

        return (dados, titulo);

    }

    private async Task<(Department?, Error?)> ExtractDatas(IFormFile file)
    {
        var lineCount = 0;
        try
        {
            var fileName = file.FileName.Split("-");
            var anoVigencia = Convert.ToInt16(fileName[2].Trim().Replace(".csv", ""));
            var mesVigencia = fileName[1].Trim().ToUpper();
            var mesVigenciaNumero = new MonthToNumber().Meses.FirstOrDefault(mes => mes.ContainsKey(mesVigencia)).Values.FirstOrDefault();

            var InicioMes = new DateTime(anoVigencia, mesVigenciaNumero, 1);
            var FimMes = new DateTime(anoVigencia, mesVigenciaNumero + 1, 1).AddDays(-1);

            var diasMes = ValidDaysMonth(InicioMes, FimMes);
            var horasMes = diasMes * 8;

            var valorHorasFaltantesDepartamento = decimal.Zero;
            var valorHorasExtrasDepartamento = decimal.Zero;

            var department = new Department
            {
                Departamento = fileName[0].Trim(),
                MesVigencia = mesVigencia,
                AnoVigencia = anoVigencia,
            };

            var employees = new List<Employee>();

            _logger.LogInformation($"Iniciando a extração dos dados do arquivo {file.FileName}.");

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    lineCount++;
                    if (line.Contains("Código")) continue;
                    HourlyData pointOfDay = line;

                    if (pointOfDay.Nome is null) throw new Exception(_ErroEstruturaDados);

                    var horasExtras = pointOfDay.TotalHorasDia > 8 ? pointOfDay.TotalHorasDia - 8 : 0;
                    var horasDebito = pointOfDay.TotalHorasDia < 8 ? pointOfDay.TotalHorasDia - 8 : 0;

                    var employeeSave = employees.FirstOrDefault(e => e.Codigo == pointOfDay.Codigo);

                    valorHorasExtrasDepartamento += (horasExtras * pointOfDay.ValorHora);
                    valorHorasFaltantesDepartamento += (horasDebito * pointOfDay.ValorHora);

                    if (employeeSave is null)
                    {
                        var employeeSaved = new Employee
                        {
                            Codigo = pointOfDay.Codigo,
                            Nome = pointOfDay.Nome,
                            DiasTrabalhados = 1,
                            TotalReceber = Math.Round(pointOfDay.TotalHorasDia * pointOfDay.ValorHora, 2),
                            HorasExtras = Math.Round(horasExtras, 2),
                            HorasDebito = Math.Round(horasDebito, 2),
                            DiasExtras = 1 - diasMes,
                            DiasFalta = diasMes - 1,
                        };
                        employees.Add(employeeSaved);

                        continue;
                    }

                    employeeSave.DiasTrabalhados++;
                    employeeSave.HorasDebito = Math.Round(employeeSave.HorasDebito + horasDebito, 2);
                    employeeSave.HorasExtras = Math.Round(employeeSave.HorasExtras + horasExtras, 2);
                    employeeSave.TotalReceber = Math.Round(employeeSave.TotalReceber + (pointOfDay.TotalHorasDia * pointOfDay.ValorHora) + (horasExtras * pointOfDay.ValorHora), 2);
                    employeeSave.DiasFalta = employeeSave.DiasTrabalhados < diasMes ? employeeSave.DiasTrabalhados - diasMes : 0;
                    employeeSave.DiasExtras = employeeSave.DiasTrabalhados > diasMes ? employeeSave.DiasTrabalhados - diasMes : 0;
                }
            }

            department.Funcionarios = employees.ToArray();
            department.TotalPagar = Math.Round(employees.Sum(e => e.TotalReceber), 2);
            department.TotalDescontos = Math.Round(valorHorasFaltantesDepartamento, 2) * -1;
            department.TotalExtras = Math.Round(valorHorasExtrasDepartamento, 2);

            _logger.LogInformation($"Finalizado a extração dos dados do arquivo {file.FileName}.");

            return (department, null);

        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Erro no arquivo {file.FileName} - {ex.Message}");

            var error = new Error
            {
                NomeArquivo = file.FileName,
                Erro = $"{(lineCount == 0 ? _ErroNomeArquivo : $"Erro na linha {lineCount} do arquivo. - Tipo de dado diferente do esperado. - {ex.Message}")}"
            };
            return (null, error);
        }
    }

    public int ValidDaysMonth(DateTime firstDate, DateTime EndDate)
    {
        var totaldays = 0;
        while (firstDate <= EndDate)
        {
            if (firstDate.DayOfWeek == DayOfWeek.Sunday || firstDate.DayOfWeek == DayOfWeek.Saturday)
            {
                firstDate = firstDate.AddDays(1);
                continue;
            }

            totaldays++;
            firstDate = firstDate.AddDays(1);
        }

        return totaldays;
    }

    public bool ValidateExtensionFiles(IEnumerable<IFormFile> files)
    {
        var result = files.Any(f => f.FileName.Contains(_ExtensaoValida));
        return result;
    }

    public (string, int) GetMonthAndYear(IEnumerable<IFormFile> files)
    {
        var fileName = files.Select(f => f.FileName).Where(f => f.Contains("-")).FirstOrDefault().Split("-");

        var mes = fileName[1].Trim().ToUpper();
        var ano = Convert.ToInt16(fileName[2].Trim().Replace(".csv", ""));
        return (mes, ano);
    }
}
