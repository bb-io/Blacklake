using Apps.Blacklake.Dto;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Blacklake.DataHandlers;
public class StrategyDataHandler(InvocationContext invocationContext, [ActionParameter] LakeInput lakeInput) : BlacklakeInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (lakeInput?.LakeId is null) throw new PluginMisconfigurationException("Please select a lake first");

        var request = new RestRequest($"/lakes/{lakeInput.LakeId}/strategies", Method.Get);
        var result = await Client.ExecuteWithErrorHandling<IEnumerable<StrategyDto>>(request);

        return result
            .Where(x => context.SearchString == null || x.Name == null || x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase) )
            .Select(x => new DataSourceItem(x.Id.ToString(), x?.Name ?? $"Strategy {x?.Id}"));

    }
}
