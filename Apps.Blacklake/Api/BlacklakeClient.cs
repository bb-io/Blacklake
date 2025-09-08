using Apps.Blacklake.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using RestSharp;

namespace Apps.Blacklake.Api;

public class BlacklakeClient : BlackBirdRestClient
{
    public BlacklakeClient(IEnumerable<AuthenticationCredentialsProvider> creds) : base(new()
    {
        BaseUrl = new Uri(creds.Get(CredsNames.BlacklakeUrl).Value),
    })
    {
        this.AddDefaultHeader("X-API-KEY", creds.Get(CredsNames.BlacklakeKey).Value);
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        throw new PluginApplicationException(response.ErrorMessage ?? response.Content ?? "Empty error");
    }
}
