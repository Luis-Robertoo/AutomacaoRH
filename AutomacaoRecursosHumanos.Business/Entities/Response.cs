namespace AutomacaoRecursosHumanos.Business.Entities;

public class Response
{
    public IEnumerable<Error>? Errors { get; set; }
    public IEnumerable<Department> Departments { get; set; }
}
