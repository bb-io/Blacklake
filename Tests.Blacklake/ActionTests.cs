using Apps.Blacklake.Actions;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common.Files;
using Tests.Blacklake.Base;

namespace Tests.Blacklake;

[TestClass]
public class ActionTests : TestBase
{
    public const string LAKE_ID = "bfabed9b-b19a-4e12-9c6a-5d8c4b8493a8";

    [TestMethod]
    public async Task Commit()
    {
        var actions = new Actions(InvocationContext, new FileManager());

        var file = new FileReference { Name = "contentful.translated.xlf" };
        await actions.Commit(new LakeInput { LakeId = LAKE_ID }, new CommitInput { File = file });
    }

    [TestMethod]
    public async Task Leverage()
    {
        var actions = new Actions(InvocationContext, new FileManager());

        var file = new FileReference { Name = "contentful.html" };
        var result = await actions.Leverage(new LakeInput { LakeId = LAKE_ID }, new LeverageInput { File = file, TargetVariant = "nl" });

        Console.WriteLine(result.File.Name);
        Console.WriteLine(result.File.ContentType);
        Assert.IsNotNull(result.File);
    }
}
