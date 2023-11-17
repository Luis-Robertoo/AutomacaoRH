namespace AutomacaoRecursosHumanos.Business.Helper;

public class MonthToNumber
{
    public List<Dictionary<string, int>> Meses { get; private set; } = new List<Dictionary<string, int>>();

    public MonthToNumber()
    {
        Meses.Add(new Dictionary<string, int> { { "JANEIRO", 1 } });
        Meses.Add(new Dictionary<string, int> { { "FEVEREIRO", 2 } });
        Meses.Add(new Dictionary<string, int> { { "MARCO", 3 } });
        Meses.Add(new Dictionary<string, int> { { "ABRIL", 4 } });
        Meses.Add(new Dictionary<string, int> { { "MAIO", 5 } });
        Meses.Add(new Dictionary<string, int> { { "JUNHO", 6 } });
        Meses.Add(new Dictionary<string, int> { { "JULHO", 7 } });
        Meses.Add(new Dictionary<string, int> { { "AGOSTO", 8 } });
        Meses.Add(new Dictionary<string, int> { { "SETEMBRO", 9 } });
        Meses.Add(new Dictionary<string, int> { { "OUTUBRO", 10 } });
        Meses.Add(new Dictionary<string, int> { { "NOVEMBRO", 11 } });
        Meses.Add(new Dictionary<string, int> { { "DEZEMBRO", 12 } });
    }
}
