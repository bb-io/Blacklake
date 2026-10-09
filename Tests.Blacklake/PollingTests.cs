using Apps.Blacklake.Events;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common.Polling;
using Newtonsoft.Json;
using Tests.Blacklake.Base;

namespace Tests.Blacklake;

[TestClass]
public class PollingTests : TestBase
{
    private PollingList _pollingList = null!;

    [TestInitialize]
    public void Initialize()
    {
        _pollingList = new PollingList(InvocationContext);
    }

    [TestMethod]
    public async Task OnContentDraftCreated_WithNullMemory_DoesNotFlyBird()
    {
        var lake = new LakeInput { LakeId = "a24f70c3-b96f-4bf9-936d-ab5568d5b9de" };
        var request = new PollingEventRequest<ContentPollingMemory>
        {
            Memory = null,
            PollingTime = DateTime.UtcNow
        };

        var response = await _pollingList.OnContentDraftCreated(request, lake, CreateFilters());

        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Memory);
        Assert.IsNotNull(response.Memory.LastPollSince);
        Assert.IsFalse(response.FlyBird);
        Assert.IsNull(response.Result);
    }

    [TestMethod]
    public async Task OnContentDraftCreated_WithExistingMemory_IdentifiesDrafts()
    {
        var lake = new LakeInput { LakeId = "a24f70c3-b96f-4bf9-936d-ab5568d5b9de" };
        var request = new PollingEventRequest<ContentPollingMemory>
        {
            Memory = new ContentPollingMemory { LastPollSince = DateTime.UtcNow.AddHours(-48) },
            PollingTime = DateTime.UtcNow
        };

        var response = await _pollingList.OnContentDraftCreated(request, lake, CreateFilters());

        foreach(var x in response.Result)
        {
            Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
        }

        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Memory);
        Assert.IsNotNull(response.Memory.LastPollSince);

        if (response.FlyBird)
        {
            Assert.IsNotNull(response.Result);
            Assert.IsTrue(response.Result.Any());
        }
    }

    private static ContentPollingFilters CreateFilters() => new()
    {
        VariantIds = [],
        SystemIds = []
    };
}
