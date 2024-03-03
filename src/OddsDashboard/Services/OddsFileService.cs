using System.Text.Json;
using OddsDashboard.Shared.Dtos;

namespace OddsDashboard.Services;

public class OddsFileService (ILogger<OddsFileService> logger) : IOddsService
{
    public async Task<IEnumerable<OddsDto>> GetOdds()
    {
        try
        {
            var oddsJson = await File.ReadAllTextAsync("./SampleData/ncaab_results.json");
            var odds = JsonSerializer.Deserialize<IEnumerable<OddsDto>>(oddsJson, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
            if (odds != null)
            {
                return odds.Take(10);
            }
            logger.LogWarning("Failed to deserialize json");
            return Array.Empty<OddsDto>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}