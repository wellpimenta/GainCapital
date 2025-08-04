using System.Text.Json;
using TesteNubank;

public class Program
{
    public static void Main()
    {
        ProcessCalcs();
    }

    private static void ProcessCalcs()
    {
        var calculator = new CapitalCalculator();

        string line;
        while ((line = Console.ReadLine()) != null && line != "")
        {
            try
            {
                var operations = JsonSerializer.Deserialize<List<Operation>>(line);
                var taxes = calculator.CalculateTaxes(operations);
                var taxResults = taxes.Select(t => new TaxResult { Tax = t }).ToList();

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                string jsonOutput = JsonSerializer.Serialize(taxResults, jsonOptions);
                Console.WriteLine(jsonOutput);
            }
            catch (JsonException)
            {
                Console.WriteLine("[]");
            }
        }
    }
}
