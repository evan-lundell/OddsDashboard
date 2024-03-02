namespace OddsDashboard.Shared.Dtos;

public record OddsDto
{
    public required string Id { get; init; }
    public required string SportKey { get; init; }
    public required string SportTitle { get; init; }
    public DateTime CommenceTime { get; init; }
    public required string HomeTeam { get; init; }
    public required string AwayTeam { get; init; }
    public required IEnumerable<BookmakerDto> Bookmakers { get; init; }
}