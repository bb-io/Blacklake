using RestSharp;

namespace Apps.Blacklake.Helpers;

public static class ResponseExtensions
{
    public static bool IsConnectionFailure(this RestResponse response) => response.ErrorException is HttpRequestException;
}