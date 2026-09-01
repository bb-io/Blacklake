using Apps.Blacklake.DataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Blacklake.Models;

public class CommitInput
{
    [Display("File")]
    public FileReference File { get; set; }

    [Display("Language variant code", Description = "Set or overwrite the variant this content should be stored as. Leave empty to use the file's own metadata.")]
    [DataSource(typeof(VariantDataHandler))]
    public string? Variant { get; set; }

    [Display("Align with variant code", Description = "If you are storing monolingual content, use this input to align it with a source variant. If set, this overrides the source language declared in the file.")]
    [DataSource(typeof(VariantDataHandler))]
    public string? AlignmentVariant { get; set; }

    [Display("Source content ID", Description = "If you are storing monolingual content and aligning it, specify the source content ID. Relevant when your CMS stores translations without links to each other. If set, this overrides the content ID declared in the file.")]
    public string? SourceContentId { get; set; }
}
