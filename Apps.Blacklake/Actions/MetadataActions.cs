
using Apps.Blacklake.DataHandlers;
using Apps.Blacklake.Dto;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;
namespace Apps.Blacklake.Actions;

[ActionList("Metadata")]
public class MetadataActions(InvocationContext invocationContext) : BlacklakeInvocable(invocationContext)
{
    [Action("Update text metadata", Description = "Update a metadata text value on a particular content variant")]
    public async Task UpdateTextMetadata(
        [ActionParameter] LakeInput lake, 
        [ActionParameter][Display("Field")][DataSource(typeof(TextMetaFieldDataHandler))] string metaFieldId,
        [ActionParameter] MetadataInput input, 
        [ActionParameter][Display("Value")] string value)
    {
        await UpdateMetadata(lake, input, metaFieldId, value);
    }

    [Action("Update number metadata", Description = "Update a metadata number value on a particular content variant")]
    public async Task UpdateNumberMetadata(
        [ActionParameter] LakeInput lake,
        [ActionParameter][Display("Field")][DataSource(typeof(NumberMetaFieldDataHandler))] string metaFieldId,
        [ActionParameter] MetadataInput input, 
        [ActionParameter][Display("Value")] double value)
    {
        await UpdateMetadata(lake, input, metaFieldId, value);
    }

    [Action("Update boolean metadata", Description = "Update a metadata boolean value on a particular content variant")]
    public async Task UpdateBooleanMetadata(
        [ActionParameter] LakeInput lake,
        [ActionParameter][Display("Field")][DataSource(typeof(BooleanMetaFieldDataHandler))] string metaFieldId,
        [ActionParameter] MetadataInput input, 
        [ActionParameter][Display("Value")] bool value)
    {
        await UpdateMetadata(lake, input, metaFieldId, value);
    }

    private async Task UpdateMetadata(LakeInput lake, MetadataInput input, string metaFieldId, object value)
    {
        var variantsRequest = new RestRequest($"/lakes/{lake.LakeId}/variants", Method.Get);
        var variantsResult = await Client.ExecuteWithErrorHandling<List<VariantDto>>(variantsRequest);

        var variant = variantsResult.FirstOrDefault(x => x.AllCodes.Contains(input.VariantCode));
        if (variant is null) 
        {
            throw new PluginMisconfigurationException($"Variant code {input.VariantCode} not found in this lake.");
        }

        var contentRequest = new RestRequest($"/lakes/{lake.LakeId}/content/external/{input.ExternalContentId}/variants/{variant.Id}", Method.Get);
        var contentResult = await Client.ExecuteWithErrorHandling<ContentDto>(contentRequest);

        if (contentResult == null)
        {
            // For now we just silent error, no need to create a misconfiguraton exception
            return;
        }

        var metaRequest = new RestRequest($"/lakes/{lake.LakeId}/content/{contentResult.Id}/metadata/{metaFieldId}", Method.Put);
        metaRequest.AddJsonBody(new { value });
        await Client.ExecuteWithErrorHandling(metaRequest);
    }
}
