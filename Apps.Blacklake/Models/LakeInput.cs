using Apps.Blacklake.DataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Blacklake.Models;
public class LakeInput
{
    [Display("Lake ID")]
    [DataSource(typeof(LakeDataHandler))]
    public string LakeId { get; set; }
}
