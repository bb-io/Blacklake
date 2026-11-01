using Blackbird.Applications.Sdk.Common;

namespace Apps.Blacklake.Models;

public class ContentInput
{
    [Display("Content ID", Description = "The content item's ID in the lake.")]
    public string ContentId { get; set; } = string.Empty;
}
