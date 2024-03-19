namespace OddsDashboard.Shared.ViewModels;

public record HeadToHeadViewModel(int HomePrice, int AwayPrice)
{
    public string HomePriceString => HomePrice > 0 ? $"+{HomePrice}" : HomePrice.ToString();
    public string AwayPriceString => AwayPrice > 0 ? $"+{AwayPrice}" : AwayPrice.ToString();
}