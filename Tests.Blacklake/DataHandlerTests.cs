using Apps.Blacklake.Actions;
using Apps.Blacklake.DataHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
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
}
