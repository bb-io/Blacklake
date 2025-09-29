using Apps.Blacklake.Actions;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common.Files;
using Tests.Blacklake.Base;

namespace Tests.Blacklake;

[TestClass]
public class MetaTests : TestBase
{
    [TestMethod]
    public async Task Update_text()
    {
        var actions = new MetadataActions(InvocationContext);
        var lakeId = await GetLakeId();
        var fieldIds = await GetTextFieldIds();
        var variantId = "d62fd6f6-7bde-4051-a5cf-068a2a2c02e6";
        var externalContentId = "5746dLKTkEZjOQX21HX2KI";


        await actions.UpdateTextMetadata(
            new LakeInput { LakeId = lakeId }, 
            new MetadataInput { 
                FieldId = fieldIds.FirstOrDefault().Value, 
                VariantId = variantId, 
                ExternalContentId = externalContentId },
            "New value from test");
    }
}
