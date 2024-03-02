namespace OddsDashboard.Shared.Dtos;

public record BookmakerDto
{
    public required string Key { get; init; }
    public required string Title { get; init; }
    public DateTime LastUpdate { get; init; }
    public required IEnumerable<MarketDto> Markets { get; init; }
}