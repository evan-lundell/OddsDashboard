using OddsDashboard.Shared.Dtos;
using OddsDashboard.Shared.ViewModels;

namespace OddsDashboard.Services;

public class RefreshService(IServiceProvider services, ILogger<RefreshService> logger, ValidTeamsService validTeamsService) : IRefreshService
{
    private readonly string[] _bookmakerPrecedence =
    {
        "fanduel",
        "betmgm",
        "draftkings",
        "williamhill_us"
    };

    public HomeViewModel? HomeViewModel { get; private set; }
    public event Action<HomeViewModel>? DataRefreshed;
    
    public async Task RefreshDashboardData()
    {
        try
        {
            using var scope = services.CreateScope();
            var oddsService = scope.ServiceProvider.GetRequiredService<IOddsService>();
            var scoresService = scope.ServiceProvider.GetRequiredService<IScoresService>();
            var odds = (await oddsService.GetOdds()).Where(o => validTeamsService.IsTeamValid(o.HomeTeam) && validTeamsService.IsTeamValid(o.AwayTeam));
            var scores = (await scoresService.GetScores()).Where(s => validTeamsService.IsTeamValid(s.HomeTeam) && validTeamsService.IsTeamValid(s.AwayTeam));;
            var scoresLookup = new Dictionary<string, Dictionary<string, string>>();
            foreach (var scoresDto in scores.Where(s => s.Scores is { Length: > 0 }))
            {
                if (!scoresLookup.ContainsKey(scoresDto.Id))
                {
                    scoresLookup.Add(scoresDto.Id, new Dictionary<string, string>());
                }

                foreach (var scoreDto in scoresDto.Scores!)
                {
                    scoresLookup[scoresDto.Id].Add(scoreDto.Name, scoreDto.Score);
                }
            }
            
            var oddsArray = odds as OddsDto[] ?? odds.ToArray();
            var liveGames = new List<GameViewModel>();
            var upcomingGames = new List<GameViewModel>();
            foreach (var o in oddsArray)
            {
                BookmakerDto? bookmaker = null;
                foreach (var bookmakerKey in _bookmakerPrecedence)
                {
                    var bm = o.Bookmakers.FirstOrDefault(b => b.Key == bookmakerKey);
                    if (bm != null)
                    {
                        bookmaker = bm;
                        break;
                    }
                }

                bookmaker ??= o.Bookmakers.FirstOrDefault();
                
                ScoresViewModel? scoresViewModel = null;
                if (scoresLookup.TryGetValue(o.Id, out var scoreDictionary))
                {
                    scoresViewModel = new ScoresViewModel(scoreDictionary[o.HomeTeam], scoreDictionary[o.AwayTeam]);
                }
                if (bookmaker == null)
                {
                    var noBookmakerGame = new GameViewModel
                    {
                        HomeTeam = o.HomeTeam,
                        AwayTeam = o.AwayTeam,
                        Scores = scoresViewModel,
                        CommenceTime = o.CommenceTime.ToLocalTime(),
                        Spreads = null,
                        OverUnder = null,
                        HeadToHead = null
                    };
                    if (o.CommenceTime > DateTime.UtcNow)
                    {
                        upcomingGames.Add(noBookmakerGame);
                    }
                    else
                    {
                        liveGames.Add(noBookmakerGame);
                    }
                    continue;
                }
                
                SpreadsViewModel? spreads = null;
                OverUnderViewModel? overUnder = null;
                HeadToHeadViewModel? headToHead = null;
                var spreadsDto = bookmaker.Markets.FirstOrDefault(m => m.Key == "spreads");
                if (spreadsDto != null)
                {
                    var homeOutcome = spreadsDto.Outcomes.First(outcome => outcome.Name == o.HomeTeam);
                    var awayOutcome = spreadsDto.Outcomes.First(outcome => outcome.Name == o.AwayTeam);
                    spreads = new SpreadsViewModel(homeOutcome.Price, awayOutcome.Price, homeOutcome.Point!.Value, awayOutcome.Point!.Value);
                }

                var overUnderDto = bookmaker.Markets.FirstOrDefault(m => m.Key == "totals");
                if (overUnderDto != null)
                {
                    var overOutcome = overUnderDto.Outcomes.First(outcome => outcome.Name == "Over");
                    var underOutcome = overUnderDto.Outcomes.First(outcome => outcome.Name == "Under");
                    overUnder = new OverUnderViewModel(underOutcome.Price, overOutcome.Price, underOutcome.Point!.Value, overOutcome.Point!.Value);
                }

                var headToHeadDto = bookmaker.Markets.FirstOrDefault(m => m.Key == "h2h");
                if (headToHeadDto != null)
                {
                    var homeOutcome = headToHeadDto.Outcomes.First(outcome => outcome.Name == o.HomeTeam);
                    var awayOutcome = headToHeadDto.Outcomes.First(outcome => outcome.Name == o.AwayTeam);
                    headToHead = new HeadToHeadViewModel(homeOutcome.Price, awayOutcome.Price);
                }
                
                var game = new GameViewModel
                {
                    HomeTeam = o.HomeTeam,
                    AwayTeam = o.AwayTeam,
                    Scores = scoresViewModel,
                    CommenceTime = o.CommenceTime.ToLocalTime(),
                    Spreads = spreads,
                    OverUnder = overUnder,
                    HeadToHead = headToHead
                };
                if (o.CommenceTime > DateTime.UtcNow)
                {
                    upcomingGames.Add(game);
                }
                else
                {
                    liveGames.Add(game);
                }
            }

            HomeViewModel =  new HomeViewModel
            {
                LiveGames = liveGames,
                UpcomingGames = upcomingGames
            };
            DataRefreshed?.Invoke(HomeViewModel);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get dashboard data");
            HomeViewModel = new HomeViewModel
            {
                LiveGames = Array.Empty<GameViewModel>(),
                UpcomingGames = Array.Empty<GameViewModel>()
            };
            DataRefreshed?.Invoke(HomeViewModel);
        }
    }
}