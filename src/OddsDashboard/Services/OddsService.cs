using OddsDashboard.Shared.Dtos;

namespace OddsDashboard.Services;

public class OddsService(HttpClient client, ILogger<OddsService> logger, IConfiguration config) : IOddsService
{
    public async Task<IEnumerable<OddsDto>> GetOdds()
    {
        try
        {
            var apiKey = config[Constants.OddsApiKeyEnvVar]!;
            var odds = await client.GetFromJsonAsync<IEnumerable<OddsDto>>(
                $"v4/sports/basketball_ncaab/odds?apiKey={apiKey}&markets=spreads,h2h,totals&regions=us&oddsFormat=american");
            return odds ?? Array.Empty<OddsDto>();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get odds");
            return Array.Empty<OddsDto>();
        }
    }
}