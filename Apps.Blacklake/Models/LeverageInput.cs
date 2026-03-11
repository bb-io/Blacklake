using Apps.Blacklake.DataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Blacklake.Models;
public class LeverageInput
{
    [Display("File")]
    public FileReference File { get; set; }

    [Display("Target variant code", Description = "The variant code to prepare content for.")]
    [DataSource(typeof(VariantDataHandler))]
    public string TargetVariant { get; set; }

    [Display("Source content ID", Description = "Specify the source content ID. Use when your CMS stores translations without links to each other.")]
    public string? SourceContentId { get; set; }

    [Display("Strategy ID", Description = "Select a leveraging strategy configured in your Blacklake. If not set, content is only diffed.")]
    [DataSource(typeof(StrategyDataHandler))]
    public string? StrategyId { get; set; }
}
