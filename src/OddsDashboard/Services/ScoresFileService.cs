using System.Text.Json;
using OddsDashboard.Shared.Dtos;

namespace OddsDashboard.Services;

public class ScoresFileService(ILogger<ScoresFileService> logger) : IScoresService
{
    public async Task<IEnumerable<ScoresDto>> GetScores()
    {
        var results = await File.ReadAllTextAsync("./SampleData/scores.json");
        var scores = JsonSerializer.Deserialize<IEnumerable<ScoresDto>>(results, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        });
        logger.LogInformation("Fetched scores data from file");
        return scores ?? Array.Empty<ScoresDto>();
    }
}