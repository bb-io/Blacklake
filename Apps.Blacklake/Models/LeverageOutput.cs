using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Blacklake.Models;
public class LeverageOutput
{
    [Display("Leveraged file")]
    public FileReference File { get; set; }
}
