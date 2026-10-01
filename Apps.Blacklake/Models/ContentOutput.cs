using Apps.Blacklake.Dto;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Blacklake.Models;
public class ContentOutput
{
    [Display("Content ID")]
    public string Id { get; set; }

    [Display("Variant ID")]
    public string VariantId { get; set; }

    [Display("Name")]
    public string Name { get; set; }

    [Display("External ID")]
    public string ExternalId { get; set; }

    [Display("Source external ID")]
    public string SourceExternalId { get; set; }

    public ContentOutput(ContentDto dto)
    {
        Id = dto.Id;
        VariantId = dto.VariantId;
        Name = dto.Name;
        ExternalId = dto.ExternalId;
        SourceExternalId = dto.SourceExternalId;
    }
}
