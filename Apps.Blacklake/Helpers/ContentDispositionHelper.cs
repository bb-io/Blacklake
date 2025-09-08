using System.Net;
using System.Text.RegularExpressions;

namespace Apps.Blacklake.Helpers;
public static class ContentDispositionHelper
{
    public static string? GetFileName(string contentDisposition)
    {
        if (string.IsNullOrWhiteSpace(contentDisposition))
            return null;

        // Try to match filename* (RFC 5987 / RFC 6266, supports UTF-8 and URL encoding)
        var filenameStarMatch = Regex.Match(contentDisposition, @"filename\*\s*=\s*([^']*)''([^;]+)", RegexOptions.IgnoreCase);
        if (filenameStarMatch.Success)
        {
            try
            {
                var encodingName = filenameStarMatch.Groups[1].Value;
                var encodedFileName = filenameStarMatch.Groups[2].Value;

                var encoding = string.IsNullOrEmpty(encodingName) ? System.Text.Encoding.UTF8 : System.Text.Encoding.GetEncoding(encodingName);
                var decoded = WebUtility.UrlDecode(encodedFileName);
                return decoded;
            }
            catch
            {
                // fall through if decoding fails
            }
        }

        // Fallback: plain filename
        var filenameMatch = Regex.Match(contentDisposition, @"filename\s*=\s*[""']?([^;""']+)[""']?", RegexOptions.IgnoreCase);
        if (filenameMatch.Success)
        {
            return filenameMatch.Groups[1].Value;
        }

        return null;
    }
}