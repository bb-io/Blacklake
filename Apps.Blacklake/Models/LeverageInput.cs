using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Blacklake.Models;
public class LeverageInput
{
    [Display("File")]
    public FileReference File { get; set; }

    [Display("Target content unique ID")]
    public string? TargetContentId { get; set; }

    [Display("Target variant")]
    public string? TargetVariant { get; set; }
}
