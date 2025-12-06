using Apps.Blacklake.Actions;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common.Files;
using Tests.Blacklake.Base;

namespace Tests.Blacklake;

[TestClass]
public class MetaTests : TestBase
{
    [TestMethod]
    public async Task Update_number()
    {
        var actions = new MetadataActions(InvocationContext);
        var lakeId = "a3510bb0-e0b8-40ca-a040-218f73a6eb50";
        var fieldId = "5caecfc7-abbc-4fcd-b502-691e4dc75cd6";
        var variantCode = "nl";
        var externalContentId = "17442755172497";


        await actions.UpdateNumberMetadata(
            new LakeInput { LakeId = lakeId },
            fieldId,
            new MetadataInput { 
                VariantCode = variantCode, 
                ExternalContentId = externalContentId },
            3);
    }
}
