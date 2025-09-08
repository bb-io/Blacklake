using Apps.Blacklake.Actions;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common.Files;
using Tests.Blacklake.Base;

namespace Tests.Blacklake;

[TestClass]
public class ActionTests : TestBase
{
    [TestMethod]
    public async Task Commit()
    {
        var actions = new Actions(InvocationContext, new FileManager());

        var file = new FileReference { Name = "contentful.translated.xlf" };
        await actions.Commit(new LakeInput { LakeId = "c3b93820-f79a-42fd-a654-bddff5378eaf" }, new CommitInput { File = file });
    }

    [TestMethod]
    public async Task Leverage()
    {
        var actions = new Actions(InvocationContext, new FileManager());

        var file = new FileReference { Name = "contentful.xlf" };
        var result = await actions.Leverage(new LakeInput { LakeId = "c3b93820-f79a-42fd-a654-bddff5378eaf" }, new LeverageInput { File = file });

        Console.WriteLine(result.File.Name);
        Console.WriteLine(result.File.ContentType);
        Assert.IsNotNull(result.File);
    }
}
