using System.Text.Json;
using OddsDashboard.Shared.Dtos;

namespace OddsDashboard.Services;

public class ScoresService(HttpClient client, ILogger<ScoresService> logger, IConfiguration config)
    : IScoresService
{
    public async Task<IEnumerable<ScoresDto>> GetScores()
    {
        try
        {
            var apiKey = config[Constants.OddsApiKeyEnvVar]!;
            var response = await client.GetAsync(
                $"v4/sports/basketball_ncaab/scores?apiKey={apiKey}");
            var remainingRequests = response.Headers.TryGetValues("X-Requests-Remaining", out var headers)
                ? headers.FirstOrDefault()
                : null;
            if (remainingRequests != null)
            {
                logger.LogInformation("Remaining Requests: {RemainingRequests}", remainingRequests);
            }

            var content = await response.Content.ReadAsStringAsync();
            var scores = JsonSerializer.Deserialize<IEnumerable<ScoresDto>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
            return scores ?? Array.Empty<ScoresDto>();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get scores");
            return Array.Empty<ScoresDto>();
        }
    }
}