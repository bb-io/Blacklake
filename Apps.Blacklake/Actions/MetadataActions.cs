
using Apps.Blacklake.Dto;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;
namespace Apps.Blacklake.Actions;

[ActionList("Metadata")]
public class MetadataActions(InvocationContext invocationContext) : BlacklakeInvocable(invocationContext)
{
    [Action("Update text metadata", Description = "Update a metadata text value on a particular content variant")]
    public async Task UpdateTextMetadata([ActionParameter] LakeInput lake, [ActionParameter] MetadataInput input, [ActionParameter][Display("Value")] string value)
    {
        await UpdateMetadata(lake, input, value);
    }

    [Action("Update number metadata", Description = "Update a metadata number value on a particular content variant")]
    public async Task UpdateNumberMetadata([ActionParameter] LakeInput lake, [ActionParameter] MetadataInput input, [ActionParameter][Display("Value")] double value)
    {
        await UpdateMetadata(lake, input, value);
    }

    [Action("Update boolean metadata", Description = "Update a metadata boolean value on a particular content variant")]
    public async Task UpdateBooleanMetadata([ActionParameter] LakeInput lake, [ActionParameter] MetadataInput input, [ActionParameter][Display("Value")] bool value)
    {
        await UpdateMetadata(lake, input, value);
    }

    private async Task UpdateMetadata(LakeInput lake, MetadataInput input, object value)
    {
        var contentRequest = new RestRequest($"/lakes/{lake.LakeId}/content/external/{input.ExternalContentId}/variants/{input.VariantId}", Method.Get);
        var contentResult = await Client.ExecuteWithErrorHandling<ContentDto>(contentRequest);

        if (contentResult == null)
        {
            // For now we just silent error, no need to create a misconfiguraton exception
            return;
        }

        var metaRequest = new RestRequest($"/lakes/{lake.LakeId}/content/{contentResult.Id}/metadata/{input.FieldId}", Method.Put);
        metaRequest.AddJsonBody(new { value });
        await Client.ExecuteWithErrorHandling(metaRequest);
    }
}
