using Apps.Blacklake.DataHandlers;
using Apps.Blacklake.Dto;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace Tests.Blacklake.Base;
public class TestBase
{
    public IEnumerable<AuthenticationCredentialsProvider> Creds { get; set; }

    public InvocationContext InvocationContext { get; set; }

    public FileManager FileManager { get; set; }

    public TestBase()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        Creds = config.GetSection("ConnectionDefinition").GetChildren()
            .Select(x => new AuthenticationCredentialsProvider(x.Key, x.Value)).ToList();


        var relativePath = config.GetSection("TestFolder").Value;
        var projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        var folderLocation = Path.Combine(projectDirectory, relativePath);

        InvocationContext = new InvocationContext
        {
            AuthenticationCredentialsProviders = Creds,
            Bird = new BirdInfo { Name = "Test Bird" },
            Flight = new FlightInfo { Url = "www.example.com" },
        };

        FileManager = new FileManager();
    }

    public async Task<string> GetLakeId()
    {
        var handler = new LakeDataHandler(InvocationContext);
        var lakesResult = await handler.GetDataAsync(new DataSourceContext { }, CancellationToken.None);
        return lakesResult.Last().Value;
    }

    public async Task<IEnumerable<DataSourceItem>> GetTextFieldIds()
    {
        var lakeId = await GetLakeId();
        var handler = new TextMetaFieldDataHandler(InvocationContext, new LakeInput { LakeId = lakeId });
        return await handler.GetDataAsync(new DataSourceContext { }, CancellationToken.None);
    }

    public async Task<IEnumerable<DataSourceItem>> GetNumberFieldIds()
    {
        var lakeId = await GetLakeId();
        var handler = new TextMetaFieldDataHandler(InvocationContext, new LakeInput { LakeId = lakeId });
        return await handler.GetDataAsync(new DataSourceContext { }, CancellationToken.None);
    }
}
