using Apps.Blacklake.Actions;
using Apps.Blacklake.Api;
using Apps.Blacklake.Dto;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common.Exceptions;
using RestSharp;
using System.Text;
using Tests.Blacklake.Base;

namespace Tests.Blacklake;

[TestClass]
public class MetaTests : TestBase
{
    private BlacklakeClient client = null!;
    private MetadataActions actions = null!;
    private LakeInput lake = null!;
    private ContentInput content = null!;
    private string externalId = null!;

    [TestInitialize]
    public async Task CreateFixture()
    {
        client = new BlacklakeClient(Creds);
        actions = new MetadataActions(InvocationContext);
        var request = new RestRequest("/lakes", Method.Post);
        request.AddJsonBody(new { name = $"Get metadata integration tests {Guid.NewGuid()}" });
        var created = await client.ExecuteWithErrorHandling<LakeDto>(request);
        lake = new LakeInput { LakeId = created.Id };

        var commit = new RestRequest($"/lakes/{lake.LakeId}/commit", Method.Post);
        const string html = """
            <html lang="en"><head><meta name="blackbird-ucid" content="metadata/test?name=a&amp;value=b + c"></head>
            <body><p data-blackbird-key="title">Metadata test content</p></body></html>
            """;
        commit.AddFile("file", Encoding.UTF8.GetBytes(html), "metadata-test.html", "text/html");
        commit.AddParameter("variant", "en");
        var result = await client.ExecuteWithErrorHandling<ContentChangeAndRulesDto>(commit);
        content = new ContentInput { ContentId = result.Changes.Single().ContentId };
        externalId = (await client.ExecuteWithErrorHandling<MinifiedContentDto>(new RestRequest($"/lakes/{lake.LakeId}/content/{content.ContentId}", Method.Get))).ExternalId;
        Console.WriteLine($"Test lake: {lake.LakeId}; content: {content.ContentId}");
    }

    [TestCleanup]
    public async Task DeleteFixture()
    {
        if (lake is not null)
            await client.ExecuteWithErrorHandling(new RestRequest($"/lakes/{lake.LakeId}", Method.Delete));
        client.Dispose();
    }

    [TestMethod]
    public async Task FindContentByExternalId_ResolvesVariantAndSpecialCharacters()
    {
        var contentActions = new ContentActions(InvocationContext, new FileManager());
        var expected = await client.ExecuteWithErrorHandling<MinifiedContentDto>(new RestRequest($"/lakes/{lake.LakeId}/content/{content.ContentId}", Method.Get));
        var result = await contentActions.FindContentByExternalId(lake, externalId, "en");
        Assert.AreEqual(expected.Id, result.ContentId);
        Assert.AreEqual(expected.Name, result.ContentName);

        await Assert.ThrowsExceptionAsync<PluginApplicationException>(() => contentActions.FindContentByExternalId(lake, "missing", "en"));
        await Assert.ThrowsExceptionAsync<PluginMisconfigurationException>(() => contentActions.FindContentByExternalId(lake, externalId, "missing-variant"));
    }

    [TestMethod]
    public async Task FindContentByExternalId_SelectsRequestedVariant()
    {
        var commit = new RestRequest($"/lakes/{lake.LakeId}/commit", Method.Post);
        var html = "<html lang=\"fr\"><head><meta name=\"blackbird-ucid\" content=\"metadata/test?name=a&amp;value=b + c\"></head><body><p data-blackbird-key=\"title\">Bonjour</p></body></html>";
        commit.AddFile("file", Encoding.UTF8.GetBytes(html), "metadata-fr.html", "text/html");
        commit.AddParameter("variant", "fr");
        await client.ExecuteWithErrorHandling(commit);

        var contentActions = new ContentActions(InvocationContext, new FileManager());
        var result = await contentActions.FindContentByExternalId(lake, externalId, "en");
        Assert.AreEqual(content.ContentId, result.ContentId);
        var french = await contentActions.FindContentByExternalId(lake, externalId, "fr");
        Assert.AreNotEqual(result.ContentId, french.ContentId);
    }

    [TestMethod]
    public async Task UpdateMetadata_ResolvesExternalIdWithSpecialCharacters()
    {
        StringAssert.Contains(externalId, "metadata/test?name=a");
        var input = new MetadataInput { VariantCode = "en", ExternalContentId = externalId };
        var text = await CreateField("text");
        var number = await CreateField("number");
        var boolean = await CreateField("boolean");

        await actions.UpdateTextMetadata(lake, text.Id, input, "Updated");
        await actions.UpdateNumberMetadata(lake, number.Id, input, 3.5);
        await actions.UpdateBooleanMetadata(lake, boolean.Id, input, true);

        Assert.AreEqual("Updated", (await actions.GetTextMetadata(lake, content, text.Id)).FieldValue);
        Assert.AreEqual(3.5, (await actions.GetNumberMetadata(lake, content, number.Id)).FieldValue);
        Assert.AreEqual(true, (await actions.GetBooleanMetadata(lake, content, boolean.Id)).FieldValue);
    }

    [DataTestMethod]
    [DataRow("Marketing")]
    [DataRow("")]
    [DataRow("2026-11-01T12:34:56Z")]
    public async Task GetTextMetadata_ReturnsFieldAndText(string value)
    {
        var field = await CreateField("text", value);
        var result = await actions.GetTextMetadata(lake, content, field.Id);

        Assert.AreEqual(field.Id, result.FieldId);
        Assert.AreEqual(field.Name, result.FieldName);
        Assert.AreEqual(value, result.FieldValue);
    }

    [DataTestMethod]
    [DataRow(42.5)]
    [DataRow(0.0)]
    [DataRow(-7.25)]
    public async Task GetNumberMetadata_ReturnsFieldAndNumber(double value)
    {
        var field = await CreateField("number", value);
        var result = await actions.GetNumberMetadata(lake, content, field.Id);

        Assert.AreEqual(field.Id, result.FieldId);
        Assert.AreEqual(field.Name, result.FieldName);
        Assert.AreEqual(value, result.FieldValue);
    }

    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task GetBooleanMetadata_ReturnsFieldAndBoolean(bool value)
    {
        var field = await CreateField("boolean", value);
        var result = await actions.GetBooleanMetadata(lake, content, field.Id);

        Assert.AreEqual(field.Id, result.FieldId);
        Assert.AreEqual(field.Name, result.FieldName);
        Assert.AreEqual(value, result.FieldValue);
    }

    [DataTestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task GetMetadata_UnsetFieldOrUnknownContent_ReturnsNullValues(bool unknownContent)
    {
        var field = await CreateField("text", "Marketing");
        var input = unknownContent ? new ContentInput { ContentId = Guid.NewGuid().ToString() } : content;
        var fieldId = unknownContent ? field.Id : (await CreateField("text")).Id;

        await AssertNullOutputs(input, fieldId);
    }

    [TestMethod]
    public async Task GetMetadata_UnknownField_ReturnsNullValues()
    {
        await CreateField("text", "Marketing");
        await AssertNullOutputs(content, Guid.NewGuid().ToString());
    }

    [TestMethod]
    public async Task GetMetadata_ContentWithoutMetadata_ReturnsNullValues()
    {
        var field = await CreateField("text");
        await AssertNullOutputs(content, field.Id);
    }

    [TestMethod]
    public async Task GetMetadata_WrongFieldType_ReportsHowToFix()
    {
        var text = await CreateField("text", "Marketing");
        var number = await CreateField("number", 42.5);
        var boolean = await CreateField("boolean", false);

        var textError = await Assert.ThrowsExceptionAsync<PluginMisconfigurationException>(
            () => actions.GetTextMetadata(lake, content, number.Id));
        var numberError = await Assert.ThrowsExceptionAsync<PluginMisconfigurationException>(
            () => actions.GetNumberMetadata(lake, content, boolean.Id));
        var booleanError = await Assert.ThrowsExceptionAsync<PluginMisconfigurationException>(
            () => actions.GetBooleanMetadata(lake, content, text.Id));

        StringAssert.Contains(textError.Message, "select a text metadata field");
        StringAssert.Contains(numberError.Message, "select a number metadata field");
        StringAssert.Contains(booleanError.Message, "select a boolean metadata field");
    }

    [TestMethod]
    public async Task GetMetadata_InvalidInputs_ReportHowToFix()
    {
        var lakeError = await Assert.ThrowsExceptionAsync<PluginMisconfigurationException>(
            () => actions.GetTextMetadata(new LakeInput(), content, Guid.NewGuid().ToString()));
        var contentError = await Assert.ThrowsExceptionAsync<PluginMisconfigurationException>(
            () => actions.GetNumberMetadata(lake, new ContentInput(), Guid.NewGuid().ToString()));
        var fieldError = await Assert.ThrowsExceptionAsync<PluginMisconfigurationException>(
            () => actions.GetBooleanMetadata(lake, content, ""));

        StringAssert.Contains(lakeError.Message, "select a lake");
        StringAssert.Contains(contentError.Message, "valid content ID");
        StringAssert.Contains(fieldError.Message, "select a metadata field");
    }

    private async Task AssertNullOutputs(ContentInput input, string fieldId)
    {
        var text = await actions.GetTextMetadata(lake, input, fieldId);
        Assert.IsNull(text.FieldId);
        Assert.IsNull(text.FieldName);
        Assert.IsNull(text.FieldValue);

        var number = await actions.GetNumberMetadata(lake, input, fieldId);
        Assert.IsNull(number.FieldId);
        Assert.IsNull(number.FieldName);
        Assert.IsNull(number.FieldValue);

        var boolean = await actions.GetBooleanMetadata(lake, input, fieldId);
        Assert.IsNull(boolean.FieldId);
        Assert.IsNull(boolean.FieldName);
        Assert.IsNull(boolean.FieldValue);
    }

    private async Task<MetadataDto> CreateField(string type, object? value = null)
    {
        var request = new RestRequest($"/lakes/{lake.LakeId}/metadata", Method.Post);
        request.AddJsonBody(new { type, name = $"Test {type} field", description = "Metadata action integration test" });
        var field = await client.ExecuteWithErrorHandling<MetadataDto>(request);
        if (value is not null)
        {
            var setValue = new RestRequest($"/lakes/{lake.LakeId}/content/{content.ContentId}/metadata/{field.Id}", Method.Put);
            setValue.AddJsonBody(new { value });
            await client.ExecuteWithErrorHandling(setValue);
        }
        return field;
    }
}
