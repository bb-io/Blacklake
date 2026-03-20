using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Blacklake.Models;
public class LeverageOutput
{
    [Display("Leveraged file")]
    public FileReference File { get; set; }

    [Display("Total words")]
    public int TotalWords { get; set; }

    [Display("Total leveraged words")]
    public int LeveragedWords { get; set; }

    [Display("Fraction leveraged", Description = "Between 0.0 and 1.0, indicates the percentage of leveraged words / total words")]
    public double LeveragedFraction => TotalWords > 0 ? ((double)LeveragedWords / (double)TotalWords) : 0;

    [Display("Leveraged style guides", Description = "The style guides that were leveraged depending on the variant")]
    public IEnumerable<string> LeveragedStyleGuideNames { get; set; } = [];
}
