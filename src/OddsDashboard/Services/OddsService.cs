using OddsDashboard.Dtos;
using OddsDashboard.Shared.Dtos;

namespace OddsDashboard.Services;

public class OddsService(HttpClient client, ILogger<OddsService> logger, IConfiguration config) : IOddsService
{
    public async Task<OddsResultDto> GetOdds()
    {
        try
        {
            var apiKey = config[Constants.OddsApiKeyEnvVar]!;
            var odds = await client.GetFromJsonAsync<IEnumerable<OddsDto>>(
                $"v4/sports/basketball_ncaab/odds?apiKey={apiKey}&markets=spreads,h2h,totals&regions=us&oddsFormat=american");

            if (odds == null)
            {
                return new OddsResultDto
                {
                    LiveOdds = Array.Empty<OddsDto>(),
                    UpcomingOdds = Array.Empty<OddsDto>()
                };
            }

            odds = odds.ToArray();
            var liveOdds = odds.Where(o => o.CommenceTime <= DateTime.UtcNow);
            var upcomingOdds = odds.Where(o => o.CommenceTime > DateTime.UtcNow);
            return new OddsResultDto
            {
                LiveOdds = liveOdds,
                UpcomingOdds = upcomingOdds
            };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get odds");
            return new OddsResultDto
            {
                LiveOdds = Array.Empty<OddsDto>(),
                UpcomingOdds = Array.Empty<OddsDto>()
            };
        }
    }
}