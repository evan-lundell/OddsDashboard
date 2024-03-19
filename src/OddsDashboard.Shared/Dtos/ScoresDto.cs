namespace OddsDashboard.Shared.Dtos;

public record ScoresDto
{
    public required string Id { get; init; }
    public required string SportKey { get; init; }
    public required string SportTitle { get; init; }
    public DateTime CommenceTime { get; init; }
    public bool Completed { get; init; }
    public required string HomeTeam { get; init; }
    public required string AwayTeam { get; init; }
    public ScoreDto[]? Scores { get; init; }
    public DateTime? LastUpdate { get; init; }
}