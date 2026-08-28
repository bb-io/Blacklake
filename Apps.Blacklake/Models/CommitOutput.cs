using Blackbird.Applications.Sdk.Common;

namespace Apps.Blacklake.Models;
public class CommitOutput
{
    [Display("Number of units added")]
    public int UnitsAdded { get; set; }

    [Display("Number of units updated", Description = "The units that actually had their text content changed")]
    public int UnitsUpdated { get; set; }

    [Display("Number of units metadata updated", Description = "The units that only had their metadata (provenance, quality scores) changed")]
    public int UnitsMetadataUpdated { get; set; }

    [Display("Number of units removed")]
    public int UnitsRemoved { get; set; }
    
}
