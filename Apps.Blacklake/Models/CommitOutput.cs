using Blackbird.Applications.Sdk.Common;

namespace Apps.Blacklake.Models;
public class CommitOutput
{
    [Display("Number of units added")]
    public int UnitsAdded { get; set; }

    [Display("Number of units updated")]
    public int UnitsUpdated { get; set; }

    [Display("Number of units removed")]
    public int UnitsRemoved { get; set; }
    
}
