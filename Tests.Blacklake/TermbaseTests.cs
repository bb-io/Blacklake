using Apps.Blacklake.Actions;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common.Files;
using Newtonsoft.Json;
using Tests.Blacklake.Base;

namespace Tests.Blacklake;

[TestClass]
public class TermbaseTests : TestBase
{
    [TestMethod]
    public async Task Upload_new()
    {
        var actions = new TermbaseActions(InvocationContext, new FileManager());
        var lake = await GetLakeInput();

        var file = new FileReference { Name = "crowdin_export.tbx" };
        var result = await actions.UploadTermbase(lake, new TermbaseInput { TermbaseFile = file });
        Console.WriteLine(JsonConvert.SerializeObject(result));
    }

    [TestMethod]
    public async Task Upload_existing()
    {
        var actions = new TermbaseActions(InvocationContext, new FileManager());
        var lake = await GetLakeInput();

        var termbaseId = await GetTermbaseId(lake);
        var file = new FileReference { Name = "crowdin_export.tbx" };
        var result = await actions.UploadTermbase(lake, new TermbaseInput { TermbaseFile = file, TermbaseId = termbaseId  });
        Console.WriteLine(JsonConvert.SerializeObject(result));
    }

    [TestMethod]
    public async Task Upload_existing_with_missing_param()
    {
        var actions = new TermbaseActions(InvocationContext, new FileManager());
        var lake = await GetLakeInput();

        var termbaseId = await GetTermbaseId(lake);
        var file = new FileReference { Name = "crowdin_export.tbx" };
        var result = await actions.UploadTermbase(lake, new TermbaseInput { TermbaseFile = file, TermbaseId = termbaseId, DeleteMissingTermsAndGroups = true });
        Console.WriteLine(JsonConvert.SerializeObject(result));
    }
}
