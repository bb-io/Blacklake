using Apps.Blacklake.DataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
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

    [Display("Source variant code", Description = "The variant the content in the file is written in. If set, this overrides the language declared in the file. Leave empty to use the file's own metadata.")]
    [DataSource(typeof(VariantDataHandler))]
    public string? SourceVariant { get; set; }

    [Display("Source content ID", Description = "Specify the source content ID. Use when your CMS stores translations without links to each other. If set, this overrides the content ID declared in the file.")]
    public string? SourceContentId { get; set; }

    [Display("Strategy ID", Description = "Select a leveraging strategy configured in your Blacklake. If not set, content is only diffed.")]
    [DataSource(typeof(StrategyDataHandler))]
    public string? StrategyId { get; set; }

    [Display("Prepare for", Description = "Whether to prepare for translation (default) or editing.")]
    [StaticDataSource(typeof(PrepareForDataHandler))]
    public string? PrepareFor { get; set; }

    [Display("Termbase IDs", Description = "Select termbases to be levarged.")]
    [DataSource(typeof(TermbaseDataHandler))]
    public IEnumerable<string>? TermbaseIds { get; set; }
}
