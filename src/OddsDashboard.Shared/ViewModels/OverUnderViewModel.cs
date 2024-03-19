using System.Globalization;

namespace OddsDashboard.Shared.ViewModels;

public record OverUnderViewModel(int UnderPrice, int OverPrice, decimal UnderPoint, decimal OverPoint)
{
    public string UnderPriceString => UnderPrice > 0 ? $"+{UnderPrice}" : UnderPrice.ToString();
    public string OverPriceString => UnderPrice > 0 ? $"+{OverPrice}" : OverPrice.ToString();
    public string OverPointString => $"O {OverPoint}";
    public string UnderPointString => $"U {UnderPoint}";
}