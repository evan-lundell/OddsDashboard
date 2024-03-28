using System.Text.Json;
using OddsDashboard.Shared.Dtos;

namespace OddsDashboard.Services;

public class OddsService(HttpClient client, ILogger<OddsService> logger, IConfiguration config) : IOddsService
{
    public async Task<IEnumerable<OddsDto>> GetOdds()
    {
        try
        {
            var apiKey = config[Constants.OddsApiKeyEnvVar]!;
            var response =
                await client.GetAsync(
                    $"v4/sports/basketball_ncaab/odds?apiKey={apiKey}&markets=spreads,h2h,totals&regions=us&oddsFormat=american");
            var remainingRequests = response.Headers.TryGetValues("X-Requests-Remaining", out var headers)
                ? headers.FirstOrDefault()
                : null;
            if (remainingRequests != null)
            {
                logger.LogInformation("Remaining Requests: {RemainingRequests}", remainingRequests);
            }

            var content = await response.Content.ReadAsStringAsync();
            var odds = JsonSerializer.Deserialize<IEnumerable<OddsDto>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
            return odds ?? Array.Empty<OddsDto>();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get odds");
            return Array.Empty<OddsDto>();
        }
    }
}