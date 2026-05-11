using Apps.Blacklake.DataHandlers;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.Extensions.Configuration;

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

    private LakeInput _lakeInput;
    public async Task<LakeInput> GetLakeInput()
    {
        if (_lakeInput is not null) return _lakeInput;
        var handler = new LakeDataHandler(InvocationContext);
        var lakesResult = await handler.GetDataAsync(new DataSourceContext { }, CancellationToken.None);
        _lakeInput = new LakeInput { LakeId = lakesResult.First().Value };
        return _lakeInput;
    }

    private string _termbaseId;
    public async Task<string> GetTermbaseId(LakeInput lakeInput)
    {
        if (_termbaseId is not null) return _termbaseId;
        var handler = new TermbaseDataHandler(InvocationContext, lakeInput);
        var lakesResult = await handler.GetDataAsync(new DataSourceContext { }, CancellationToken.None);
        _termbaseId = lakesResult.First().Value;
        return _termbaseId;
    }

    public async Task<IEnumerable<DataSourceItem>> GetTextFieldIds()
    {
        var lake = await GetLakeInput();
        var handler = new TextMetaFieldDataHandler(InvocationContext, lake);
        return await handler.GetDataAsync(new DataSourceContext { }, CancellationToken.None);
    }

    public async Task<IEnumerable<DataSourceItem>> GetNumberFieldIds()
    {
        var lake = await GetLakeInput();
        var handler = new TextMetaFieldDataHandler(InvocationContext, lake);
        return await handler.GetDataAsync(new DataSourceContext { }, CancellationToken.None);
    }
}
