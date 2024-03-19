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
            var scores = await client.GetFromJsonAsync<IEnumerable<ScoresDto>>(
                $"v4/sports/basketball_ncaab/scores?apiKey={apiKey}",
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });
            return scores ?? Array.Empty<ScoresDto>();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get scores");
            return Array.Empty<ScoresDto>();
        }
    }
}