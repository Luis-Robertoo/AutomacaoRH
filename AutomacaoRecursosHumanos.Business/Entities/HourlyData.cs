namespace AutomacaoRecursosHumanos.Business.Entities;

public class HourlyData
{
    public HourlyData(
        int codigo,
        string nome,
        decimal valorHora,
        DateOnly data,
        DateTime entrada,
        DateTime saida,
        TimeOnly almoco,
        decimal totalHorasDia)
    {
        Codigo = codigo;
        Nome = nome;
        ValorHora = valorHora;
        Data = data;
        Entrada = entrada;
        Saida = saida;
        Almoco = almoco;
        TotalHorasDia = totalHorasDia;
    }

    public HourlyData() { }

    public int Codigo { get; set; }
    public string Nome { get; set; }
    public decimal ValorHora { get; set; }
    public DateOnly Data { get; set; }
    public DateTime Entrada { get; set; }
    public DateTime Saida { get; set; }
    public TimeOnly Almoco { get; set; }
    public decimal TotalHorasDia { get; set; }

    public static implicit operator HourlyData(string line)
    {
        var dados = line.Split(";");

        if (dados.Length != 7) return new HourlyData();

        var data = dados[3].Trim().Split("/");
        var dia = Convert.ToInt32(data[0]);
        var mes = Convert.ToInt32(data[1]);
        var ano = Convert.ToInt32(data[2]);

        var dataFormatada = new DateOnly(ano, mes, dia);

        var horarioEntrada = dados[4].Trim().Split(":");
        var horaEntrada = Convert.ToInt16(horarioEntrada[0]);
        var minutoEntrada = Convert.ToInt16(horarioEntrada[1]);
        var segundoEntrada = Convert.ToInt16(horarioEntrada[2]);
        var horarioEntradaFormatada = new DateTime(ano, mes, dia, horaEntrada, minutoEntrada, segundoEntrada);

        var horarioSaida = dados[5].Trim().Split(":");
        var horaSaida = Convert.ToInt16(horarioSaida[0]);
        var minutoSaida = Convert.ToInt16(horarioSaida[1]);
        var segundoSaida = Convert.ToInt16(horarioSaida[2]);
        var horarioSaidaFormatada = new DateTime(ano, mes, dia, horaSaida, minutoSaida, segundoSaida);

        var tempoAlmoco = dados[6].Trim().Split('-');
        var inicioAlmoco = tempoAlmoco[0].Trim().Split(":");
        var inicioAlmocoFormatado = new TimeOnly(Convert.ToInt16(inicioAlmoco[0].Trim()), Convert.ToInt16(inicioAlmoco[1].Trim()));

        var fimAlmoco = tempoAlmoco[1].Trim().Split(":");
        var fimAlmocoFormatado = new TimeOnly(Convert.ToInt16(fimAlmoco[0].Trim()), Convert.ToInt16(fimAlmoco[1].Trim()));

        var almoco = new TimeOnly(fimAlmocoFormatado.Ticks - inicioAlmocoFormatado.Ticks);

        var totalHoras = new TimeOnly((horarioSaidaFormatada.Ticks - horarioEntradaFormatada.Ticks) - almoco.Ticks);

        return new HourlyData(
            Convert.ToInt32(dados[0]),
            dados[1].Trim(),
            Convert.ToDecimal(dados[2].Trim().Replace("R$ ", "")),
            dataFormatada,
            horarioEntradaFormatada,
            horarioSaidaFormatada,
            almoco,
            Convert.ToDecimal(new TimeSpan(totalHoras.Ticks).TotalHours));
    }
}
