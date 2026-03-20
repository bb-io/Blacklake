using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Blacklake.DataHandlers;
public class PrepareForDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData()
    {
        return new List<DataSourceItem> { 
            new DataSourceItem("translation", "Translation (Default)"), 
            new DataSourceItem("edit", "Editing") 
        };
    }
}
