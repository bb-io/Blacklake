using Apps.Blacklake.DataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Blacklake.Models;
public class LeverageInput
{
    [Display("File")]
    public FileReference File { get; set; }

    [Display("Source content ID")]
    public string? SourceContentId { get; set; }

    [Display("Target variant")]
    [DataSource(typeof(VariantDataHandler))]
    public string TargetVariant { get; set; }
}
