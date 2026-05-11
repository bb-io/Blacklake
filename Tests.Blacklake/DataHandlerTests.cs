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
        var lake = await GetLakeInput();
        var handler = new VariantDataHandler(InvocationContext, lake);
        var result = await handler.GetDataAsync(new Blackbird.Applications.Sdk.Common.Dynamic.DataSourceContext { }, CancellationToken.None);

        Assert.IsNotNull(result);
        foreach (var item in result)
        {
            Console.WriteLine($"{item.Value} - {item.DisplayName}");
            Assert.IsNotNull(item);
        }
    }

    [TestMethod]
    public async Task Strategies()
    {
        var lake = await GetLakeInput();
        var handler = new StrategyDataHandler(InvocationContext, lake);
        var result = await handler.GetDataAsync(new Blackbird.Applications.Sdk.Common.Dynamic.DataSourceContext { }, CancellationToken.None);

        Assert.IsNotNull(result);
        foreach (var item in result)
        {
            Console.WriteLine($"{item.Value} - {item.DisplayName}");
            Assert.IsNotNull(item);
        }
    }

    [TestMethod]
    public async Task Termbases()
    {
        var lake = await GetLakeInput();
        var handler = new TermbaseDataHandler(InvocationContext, lake);
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
        var lake = await GetLakeInput();
        var handler = new TextMetaFieldDataHandler(InvocationContext, lake);
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
        var lake = await GetLakeInput();
        var handler = new NumberMetaFieldDataHandler(InvocationContext, lake);
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
        var lake = await GetLakeInput();
        var handler = new BooleanMetaFieldDataHandler(InvocationContext, lake);
        var result = await handler.GetDataAsync(new Blackbird.Applications.Sdk.Common.Dynamic.DataSourceContext { }, CancellationToken.None);

        Assert.IsNotNull(result);
        foreach (var item in result)
        {
            Console.WriteLine($"{item.Value} - {item.DisplayName}");
            Assert.IsNotNull(item);
        }
    }
}
