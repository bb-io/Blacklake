using System.Net;
using Apps.Blacklake.Constants;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RestSharp;

namespace Apps.Blacklake.Api;

public class BlacklakeClient : BlackBirdRestClient
{
    private const int RetryCount = 3;
    private const int BaseBackoffSeconds = 2;
    private const int MaxBackoffSeconds = 16;

    private static readonly Random Jitter = new();

    private static readonly HashSet<HttpStatusCode> TransientStatusCodes =
    [
        HttpStatusCode.TooManyRequests,
        HttpStatusCode.BadGateway,
        HttpStatusCode.ServiceUnavailable,
        HttpStatusCode.GatewayTimeout
    ];

    private readonly AsyncRetryPolicy<RestResponse> _retryPolicy = Policy
        .HandleResult<RestResponse>(response => TransientStatusCodes.Contains(response.StatusCode))
        .WaitAndRetryAsync(RetryCount, (retryAttempt, result, _) => GetRetryDelay(retryAttempt, result.Result),
            (_, _, _, _) => Task.CompletedTask);

    public BlacklakeClient(IEnumerable<AuthenticationCredentialsProvider> creds) : base(new()
    {
        BaseUrl = new Uri(creds.Get(CredsNames.BlacklakeUrl).Value),
    })
    {
        this.AddDefaultHeader("X-API-KEY", creds.Get(CredsNames.BlacklakeKey).Value);
    }

    public override async Task<RestResponse> ExecuteWithErrorHandling(RestRequest request)
    {
        var response = await _retryPolicy.ExecuteAsync(() => ExecuteAsync(request));
        if (!response.IsSuccessStatusCode)
            throw ConfigureErrorException(response);

        return response;
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        try
        {
            var error = JsonConvert.DeserializeObject<Error>(response.Content!)!;
            if (error.Errors?.Count > 0)
                return new PluginApplicationException(string.Join(' ', error.Errors.SelectMany(x => x.Value)));

            var message = string.Join(": ", new[] { error.Title, error.Detail }.Where(x => !string.IsNullOrWhiteSpace(x)));
            if (!string.IsNullOrEmpty(message))
                return new PluginApplicationException($"Blacklake returned {(int)response.StatusCode} {message}");
        }
        catch (JsonException)
        {
        }

        return new PluginApplicationException(response.ErrorMessage ?? response.Content ?? "Empty error");
    }

    private static TimeSpan GetRetryDelay(int retryAttempt, RestResponse response)
    {
        var retryAfter = response.Headers?
            .FirstOrDefault(h => string.Equals(h.Name, "Retry-After", StringComparison.OrdinalIgnoreCase))?
            .Value?.ToString();

        if (int.TryParse(retryAfter, out var seconds) && seconds > 0)
            return TimeSpan.FromSeconds(seconds);

        var backoff = Math.Min(BaseBackoffSeconds * Math.Pow(2, retryAttempt - 1), MaxBackoffSeconds);
        return TimeSpan.FromSeconds(backoff) + TimeSpan.FromMilliseconds(Jitter.Next(0, 500));
    }
}
