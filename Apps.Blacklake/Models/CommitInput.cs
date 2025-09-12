using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Blacklake.Models;

public class CommitInput
{
    [Display("File")]
    public FileReference File { get; set; }
}
