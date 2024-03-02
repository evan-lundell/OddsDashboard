namespace OddsDashboard.Shared.Dtos;

public record OutcomeDto
{
    public required string Name { get; init; }
    public int Price { get; init; }
    public decimal? Point { get; init; }
}