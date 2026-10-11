using Apps.Blacklake.DataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Blacklake.Models;
public class ContentPollingFilters
{
    [Display("Variant IDs", Description = "The variant ID of the content.")]
    [DataSource(typeof(VariantIdDataHandler))]
    public IEnumerable<string>? VariantIds { get; set; }

    [Display("System IDs", Description = "The system ID of the content.")]
    [DataSource(typeof(SystemIdDataHandler))]
    public IEnumerable<string>? SystemIds { get; set; }

    [Display("Source external ID", Description = "Relevant when creating a checkpoint to wait for content to be edited in Blacklake.")]
    public string ContentId { get; set; }
}
