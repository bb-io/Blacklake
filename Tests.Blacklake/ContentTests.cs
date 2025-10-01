using Apps.Blacklake.Actions;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common.Files;
using Tests.Blacklake.Base;

namespace Tests.Blacklake;

[TestClass]
public class ContentTests : TestBase
{
    [TestMethod]
    public async Task Commit()
    {
        var actions = new ContentActions(InvocationContext, new FileManager());
        var lakeId = await GetLakeId();

        var file = new FileReference { Name = "contentful.translated.xlf" };
        await actions.Commit(new LakeInput { LakeId = lakeId }, new CommitInput { File = file });
    }

    [TestMethod]
    public async Task Leverage()
    {
        var actions = new ContentActions(InvocationContext, new FileManager());
        var lakeId = await GetLakeId();

        var file = new FileReference { Name = "The Loire Valley!_en-US.html" };
        var result = await actions.Leverage(new LakeInput { LakeId = lakeId }, new LeverageInput { File = file, TargetVariant = "nl" });

        Console.WriteLine($"Total words: {result.TotalWords}");
        Console.WriteLine($"Leveraged words: {result.LeveragedWords}");
        Console.WriteLine($"Fraction leveraged: {result.LeveragedFraction}");
        Console.WriteLine(result.File.Name);
        Console.WriteLine(result.File.ContentType);
        Assert.IsNotNull(result.File);
        Assert.IsTrue(result.TotalWords > 0);
        Assert.IsTrue(result.LeveragedFraction <= 1 && result.LeveragedFraction > 0);
    }
}
