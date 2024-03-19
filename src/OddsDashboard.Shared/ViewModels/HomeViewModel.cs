namespace OddsDashboard.Shared.ViewModels;

public record HomeViewModel()
{
    public required IEnumerable<GameViewModel> LiveGames { get; init; }
    public required IEnumerable<GameViewModel> UpcomingGames { get; init; }
}