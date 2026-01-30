using Apps.Blacklake.DataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Blacklake.Models;

public class CommitInput
{
    [Display("File")]
    public FileReference File { get; set; }

    [Display("Align with variant")]
    [DataSource(typeof(VariantDataHandler))]
    public string? AlignmentVariant { get; set; }
}
