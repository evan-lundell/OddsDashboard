namespace OddsDashboard.Shared.ViewModels;

public record GameViewModel()
{
    public required string HomeTeam { get; init; }
    public required string AwayTeam { get; init; }
    public ScoresViewModel? Scores { get; init; }
    public DateTime CommenceTime { get; init; }
    public SpreadsViewModel? Spreads { get; init; }
    public OverUnderViewModel? OverUnder { get; init; }
    public HeadToHeadViewModel? HeadToHead { get; init; }
}