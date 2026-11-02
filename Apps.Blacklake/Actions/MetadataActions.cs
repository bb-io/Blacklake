
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
using System.Text.Json;
namespace Apps.Blacklake.Actions;

[ActionList("Metadata")]
public class MetadataActions(InvocationContext invocationContext) : BlacklakeInvocable(invocationContext)
{
    [Action("Get text metadata", Description = "Get a text metadata field's ID, name, and value for a content item.")]
    public async Task<TextMetadataOutput> GetTextMetadata(
        [ActionParameter] LakeInput lake,
        [ActionParameter] ContentInput input,
        [ActionParameter][Display("Metadata field ID")][DataSource(typeof(TextMetaFieldDataHandler))] string metaFieldId)
    {
        var field = await GetMetadata(lake, input, metaFieldId, JsonValueKind.String, "text");
        return new TextMetadataOutput
        {
            FieldId = field?.Id,
            FieldName = field?.Name,
            FieldValue = field?.Value.GetString()
        };
    }

    [Action("Get number metadata", Description = "Get a number metadata field's ID, name, and value for a content item.")]
    public async Task<NumberMetadataOutput> GetNumberMetadata(
        [ActionParameter] LakeInput lake,
        [ActionParameter] ContentInput input,
        [ActionParameter][Display("Metadata field ID")][DataSource(typeof(NumberMetaFieldDataHandler))] string metaFieldId)
    {
        var field = await GetMetadata(lake, input, metaFieldId, JsonValueKind.Number, "number");
        return new NumberMetadataOutput
        {
            FieldId = field?.Id,
            FieldName = field?.Name,
            FieldValue = field?.Value.GetDouble()
        };
    }

    [Action("Get boolean metadata", Description = "Get a boolean metadata field's ID, name, and value for a content item.")]
    public async Task<BooleanMetadataOutput> GetBooleanMetadata(
        [ActionParameter] LakeInput lake,
        [ActionParameter] ContentInput input,
        [ActionParameter][Display("Metadata field ID")][DataSource(typeof(BooleanMetaFieldDataHandler))] string metaFieldId)
    {
        var field = await GetMetadata(lake, input, metaFieldId, JsonValueKind.True, "boolean");
        return new BooleanMetadataOutput
        {
            FieldId = field?.Id,
            FieldName = field?.Name,
            FieldValue = field?.Value.GetBoolean()
        };
    }

    private async Task<MetadataValueDto?> GetMetadata(LakeInput lake, ContentInput input, string metaFieldId,
        JsonValueKind valueKind, string fieldType)
    {
        if (!Guid.TryParse(lake?.LakeId, out _))
            throw new PluginMisconfigurationException("Please select a lake or enter a valid lake ID.");
        if (!Guid.TryParse(input?.ContentId, out _))
            throw new PluginMisconfigurationException("Please enter a valid content ID from this lake, rather than an external content ID.");
        if (!Guid.TryParse(metaFieldId, out var fieldId))
            throw new PluginMisconfigurationException("Please select a metadata field or enter a valid metadata field ID.");

        var request = new RestRequest($"/lakes/{lake.LakeId}/content/{input.ContentId}/metadata", Method.Get);
        var response = await Client.ExecuteWithErrorHandling(request);
        if (string.IsNullOrWhiteSpace(response.Content))
            return null;

        List<MetadataValueDto> fields;
        try
        {
            var serializeOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            fields = JsonSerializer.Deserialize<List<MetadataValueDto>>(response.Content, serializeOptions) ?? [];
        }
        catch (JsonException)
        {
            throw new PluginApplicationException("The metadata response could not be read. Please try again.");
        }

        var field = fields.FirstOrDefault(x => Guid.TryParse(x.Id, out var id) && id == fieldId);
        if (field is null || field.Value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
            return null;

        if (field.Value.ValueKind != valueKind && !(fieldType == "boolean" && field.Value.ValueKind == JsonValueKind.False))
            throw new PluginMisconfigurationException($"The selected metadata field does not contain a {fieldType} value. Please select a {fieldType} metadata field.");

        return field;
    }

    [Action("Update text metadata", Description = "Update a text metadata value for a content variant.")]
    public async Task UpdateTextMetadata(
        [ActionParameter] LakeInput lake, 
        [ActionParameter][Display("Field")][DataSource(typeof(TextMetaFieldDataHandler))] string metaFieldId,
        [ActionParameter] MetadataInput input, 
        [ActionParameter][Display("Value")] string value)
    {
        await UpdateMetadata(lake, input, metaFieldId, value);
    }

    [Action("Update number metadata", Description = "Update a number metadata value for a content variant.")]
    public async Task UpdateNumberMetadata(
        [ActionParameter] LakeInput lake,
        [ActionParameter][Display("Field")][DataSource(typeof(NumberMetaFieldDataHandler))] string metaFieldId,
        [ActionParameter] MetadataInput input, 
        [ActionParameter][Display("Value")] double value)
    {
        await UpdateMetadata(lake, input, metaFieldId, value);
    }

    [Action("Update boolean metadata", Description = "Update a boolean metadata value for a content variant.")]
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

        var contentRequest = new RestRequest($"/lakes/{lake.LakeId}/content/external", Method.Get);
        contentRequest.AddQueryParameter("externalId", input.ExternalContentId);
        contentRequest.AddQueryParameter("variantId", variant.Id);
        var contentResult = await Client.ExecuteWithErrorHandling<MinifiedContentDto>(contentRequest);

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
