using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Blacklake.DataHandlers;
public class VariantIdDataHandler(InvocationContext invocationContext, [ActionParameter] LakeInput lakeInput) : BlacklakeInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (lakeInput?.LakeId is null) throw new PluginMisconfigurationException("Please select a lake first");

        var result = await Client.GetLakeVariants(lakeInput);

        return result
            .Where(x => context.SearchString == null || x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase) )
            .Select(x => new DataSourceItem(x.Id, x.Name));

    }
}
