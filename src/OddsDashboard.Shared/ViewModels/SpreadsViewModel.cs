using System.Globalization;

namespace OddsDashboard.Shared.ViewModels;

public record SpreadsViewModel(int HomePrice, int AwayPrice, decimal HomePoint, decimal AwayPoint)
{
    public string HomePriceString => HomePrice > 0 ? $"+{HomePrice}" : HomePrice.ToString();
    public string AwayPriceString => AwayPrice > 0 ? $"+{AwayPrice}" : AwayPrice.ToString();
    public string HomePointString => HomePoint > 0 ? $"+{HomePoint}" : HomePoint.ToString(CultureInfo.InvariantCulture);
    public string AwayPointString => AwayPoint > 0 ? $"+{AwayPoint}" : AwayPoint.ToString(CultureInfo.InvariantCulture);
}