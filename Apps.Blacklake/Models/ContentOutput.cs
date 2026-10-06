using Apps.Blacklake.Dto;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Blacklake.Models;
public class ContentOutput
{
    [Display("Content ID")]
    public string Id { get; set; }

    [Display("System ID")]
    public string SystemId { get; set; }

    [Display("Variant ID")]
    public string VariantId { get; set; }

    [Display("Variant")]
    public string? TargetVariant { get; set; }

    [Display("Lake ID")]
    public string LakeId { get; set; }

    [Display("Name")]
    public string Name { get; set; }

    [Display("External ID")]
    public string ExternalId { get; set; }

    [Display("Source external ID")]
    public string ContentId { get; set; }

    public ContentOutput(ContentDto dto, string? variantCode, string lakeId)
    {
        Id = dto.Id;
        VariantId = dto.VariantId;
        Name = dto.Name;
        ExternalId = dto.ExternalId;
        ContentId = dto.SourceExternalId;
        SystemId = dto.SystemId;
        TargetVariant = variantCode;
        LakeId = lakeId;
    }
}
