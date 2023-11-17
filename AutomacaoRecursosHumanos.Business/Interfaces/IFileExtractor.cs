using Microsoft.AspNetCore.Http;

namespace AutomacaoRecursosHumanos.Business.Interfaces;

public interface IFileExtractor
{
    Task<(string, string)> Extract(IEnumerable<IFormFile> files);
}
