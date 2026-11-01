using Blackbird.Applications.Sdk.Common;

namespace Apps.Blacklake.Models;

public class NumberMetadataOutput
{
    [Display("Field ID")]
    public string? FieldId { get; set; }

    [Display("Field name")]
    public string? FieldName { get; set; }

    [Display("Field value")]
    public double? FieldValue { get; set; }
}
