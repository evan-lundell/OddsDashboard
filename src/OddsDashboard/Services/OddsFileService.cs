using System.Text.Json;
using OddsDashboard.Dtos;
using OddsDashboard.Shared.Dtos;

namespace OddsDashboard.Services;

public class OddsFileService (ILogger<OddsFileService> logger) : IOddsService
{
    public async Task<OddsResultDto> GetOdds()
    {
        try
        {
            await Task.Delay(2500);
            var oddsJson = await File.ReadAllTextAsync("./SampleData/ncaab_results.json");
            var odds = JsonSerializer.Deserialize<OddsDto[]>(oddsJson, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
            if (odds != null)
            {
                var liveOdds = odds.Where(o => o.CommenceTime <= DateTime.UtcNow);
                var upcomingOdds = odds.Where(o => o.CommenceTime > DateTime.UtcNow);
                return new OddsResultDto
                {
                    LiveOdds = liveOdds,
                    UpcomingOdds = upcomingOdds
                };
            }
            logger.LogWarning("Failed to deserialize json");
            return new OddsResultDto
            {
                LiveOdds = Array.Empty<OddsDto>(),
                UpcomingOdds = Array.Empty<OddsDto>()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}