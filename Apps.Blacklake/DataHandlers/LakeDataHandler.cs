using Apps.Blacklake.Dto;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Blacklake.DataHandlers;
public class LakeDataHandler(InvocationContext invocationContext) : BlacklakeInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new RestRequest("/lakes", Method.Get);
        var result = await Client.ExecuteWithErrorHandling<IEnumerable<LakeDto>>(request);

        return result
            .Where(x => context.SearchString == null || x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase) )
            .Select(x => new DataSourceItem(x.Id, x.Name));

    }
}
