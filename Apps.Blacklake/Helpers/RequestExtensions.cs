using RestSharp;

namespace Apps.Blacklake.Helpers;

public static class RequestExtensions
{
    public static void AddOverrideParameter(this RestRequest request, string name, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            request.AddParameter(name, value.Trim());
    }
}
