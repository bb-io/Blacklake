using Blackbird.Applications.Sdk.Common;

namespace Apps.Blacklake.Models;
public class TermbaseOutput
{
    [Display("Termbase ID")]
    public string TermbaseId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
