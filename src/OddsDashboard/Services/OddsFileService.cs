using System.Text.Json;
using OddsDashboard.Shared.Dtos;

namespace OddsDashboard.Services;

public class OddsFileService(ILogger<OddsFileService> logger) : IOddsService
{
    public async Task<IEnumerable<OddsDto>> GetOdds()
    {
        var results = await File.ReadAllTextAsync("./SampleData/ncaab_results.json");
        var odds = JsonSerializer.Deserialize<IEnumerable<OddsDto>>(results, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        });
        logger.LogInformation("Fetched odds data from file");
        return odds ?? Array.Empty<OddsDto>();
    }
}