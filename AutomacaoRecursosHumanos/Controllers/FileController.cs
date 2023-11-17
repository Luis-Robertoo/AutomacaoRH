using AutomacaoRecursosHumanos.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AutomacaoRecursosHumanos.Controllers;

public class FileController : Controller
{
    private readonly ILogger<FileController> _logger;
    private readonly IFileExtractor _fileExtractor;

    public FileController(ILogger<FileController> logger, IFileExtractor fileExtractor)
    {
        _logger = logger;
        _fileExtractor = fileExtractor;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IEnumerable<IFormFile> files)
    {
        var (dados, titulo) = await _fileExtractor.Extract(files);

        ViewData["Message"] = "";
        if (dados is null || titulo is null)
        {
            ViewData["Message"] = "Escolha arquivos válidos.";
            return View("Index");
        }

        return File(new MemoryStream(Encoding.UTF8.GetBytes(dados)), "application/json", titulo);
    }

}
