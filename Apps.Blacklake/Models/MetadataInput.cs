using Apps.Blacklake.DataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Blacklake.Models;
public class MetadataInput
{
    [Display("Variant ID")]
    [DataSource(typeof(VariantDataHandler))]
    public string VariantId { get; set; }

    [Display("External content ID")]
    public string ExternalContentId { get; set; }
}
