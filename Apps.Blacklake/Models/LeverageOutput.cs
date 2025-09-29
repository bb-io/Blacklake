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

    [Display("Fraction leveraged")]
    public double LeveragedFraction => (double)LeveragedWords / (double)TotalWords;
}
