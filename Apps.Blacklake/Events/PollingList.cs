using Apps.Blacklake.Dto;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;
using System.Globalization;

namespace Apps.Blacklake.Events;

[PollingEventList("Content")]
public class PollingList(InvocationContext invocationContext) : BlacklakeInvocable(invocationContext)
{
    [PollingEvent("On draft saved",
       Description = "Triggers when a draft is saved in or to a Lake.")]
    [MultipleEvents]
    public async Task<PollingEventResponse<ContentPollingMemory, List<ContentOutput>>> OnContentDraftCreated(
       PollingEventRequest<ContentPollingMemory> request,
       [PollingEventParameter] LakeInput lake, [PollingEventParameter] ContentPollingFilters filters)
    {
        if (string.IsNullOrEmpty(lake.LakeId)) throw new PluginMisconfigurationException("The lake ID is null or empty.");

        if (request.Memory is null || request.Memory.LastPollSince is null)
        {
            return new()
            {
                FlyBird = false,
                Memory = new() { LastPollSince = DateTime.UtcNow }
            };
        }

        var lastDate = request.Memory.LastPollSince.Value;
        var now = DateTime.UtcNow;

        var restRequest = new RestRequest($"/lakes/{lake.LakeId}/content", Method.Get);
        restRequest.AddQueryParameter("draftChangedSince", lastDate.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture));

        if (filters.VariantIds is not null && filters.VariantIds.Any())
        {
            foreach (var variantId in filters.VariantIds.Where(id => !string.IsNullOrEmpty(id)))
            {
                restRequest.AddQueryParameter("variantIds", variantId);  
            }
        }

        if (filters.SystemIds is not null && filters.SystemIds.Any())
        {
            foreach (var systemId in filters.SystemIds.Where(id => !string.IsNullOrEmpty(id)))
            {
                restRequest.AddQueryParameter("systemIds", systemId);
            }
        }

        var response = await Client.ExecuteWithErrorHandling<IEnumerable<MinifiedContentDto>>(restRequest);

        var variants = await Client.GetLakeVariants(lake);

        var result = response.Select(x => new ContentOutput(x, variants.FirstOrDefault(y => y.Id == x.VariantId)?.DefaultCode, lake.LakeId)).ToList();

        return new()
        {
            FlyBird = result.Count > 0,
            Result = result,
            Memory = new() { LastPollSince = now }
        };
    }
}
