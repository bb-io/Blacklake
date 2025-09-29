using Apps.Blacklake.DataHandlers;
using Apps.Blacklake.Models;
using Tests.Blacklake.Base;

namespace Tests.Blacklake;

[TestClass]
public class DataHandlerTests : TestBase
{
    [TestMethod]
    public async Task Lakes()
    {
        var handler = new LakeDataHandler(InvocationContext);
        var result = await handler.GetDataAsync(new Blackbird.Applications.Sdk.Common.Dynamic.DataSourceContext { }, CancellationToken.None);
    
        Assert.IsNotNull(result);
        foreach(var item in result)
        {
            Console.WriteLine($"{item.Value} - {item.DisplayName}");
            Assert.IsNotNull(item);
        }

    }

    [TestMethod]
    public async Task Variants()
    {
        var lakeId = await GetLakeId();
        var handler = new VariantDataHandler(InvocationContext, new LakeInput { LakeId = lakeId });
        var result = await handler.GetDataAsync(new Blackbird.Applications.Sdk.Common.Dynamic.DataSourceContext { }, CancellationToken.None);

        Assert.IsNotNull(result);
        foreach (var item in result)
        {
            Console.WriteLine($"{item.Value} - {item.DisplayName}");
            Assert.IsNotNull(item);
        }
    }

    [TestMethod]
    public async Task Text_metadata()
    {
        var lakeId = await GetLakeId();
        var handler = new TextMetaFieldDataHandler(InvocationContext, new LakeInput { LakeId = lakeId });
        var result = await handler.GetDataAsync(new Blackbird.Applications.Sdk.Common.Dynamic.DataSourceContext { }, CancellationToken.None);

        Assert.IsNotNull(result);
        foreach (var item in result)
        {
            Console.WriteLine($"{item.Value} - {item.DisplayName}");
            Assert.IsNotNull(item);
        }
    }

    [TestMethod]
    public async Task Number_metadata()
    {
        var lakeId = await GetLakeId();
        var handler = new NumberMetaFieldDataHandler(InvocationContext, new LakeInput { LakeId = lakeId });
        var result = await handler.GetDataAsync(new Blackbird.Applications.Sdk.Common.Dynamic.DataSourceContext { }, CancellationToken.None);

        Assert.IsNotNull(result);
        foreach (var item in result)
        {
            Console.WriteLine($"{item.Value} - {item.DisplayName}");
            Assert.IsNotNull(item);
        }
    }

    [TestMethod]
    public async Task Boolean_metadata()
    {
        var lakeId = await GetLakeId();
        var handler = new BooleanMetaFieldDataHandler(InvocationContext, new LakeInput { LakeId = lakeId });
        var result = await handler.GetDataAsync(new Blackbird.Applications.Sdk.Common.Dynamic.DataSourceContext { }, CancellationToken.None);

        Assert.IsNotNull(result);
        foreach (var item in result)
        {
            Console.WriteLine($"{item.Value} - {item.DisplayName}");
            Assert.IsNotNull(item);
        }
    }
}
