using Blackbird.Applications.Sdk.Common;

namespace Apps.Blacklake.Models;

public class FindContentOutput
{
    [Display("Content ID")]
    public string ContentId { get; set; } = string.Empty;

    [Display("Content name")]
    public string ContentName { get; set; } = string.Empty;
}
