namespace Apps.Blacklake.Dto;
public class MetricsDto
{
    public int TotalWords { get; set; } = 0;
    public int TotalLeveragedWords { get; set; } = 0;
    public IEnumerable<string> LeveragedStyleGuideNames { get; set; } = [];
    public int TotalGlobalDesiredAdded { get; set; } = 0;
    public int TotalGlobalForbiddenAdded { get; set; } = 0;
    public int TotalLocalTermsAdded { get; set; } = 0;
}
