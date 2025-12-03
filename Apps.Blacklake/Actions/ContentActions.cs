using Apps.Blacklake.Dto;
using Apps.Blacklake.Helpers;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using RestSharp;
using System.Text.Json;

namespace Apps.Blacklake.Actions;

[ActionList("Content")]
public class ContentActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : BlacklakeInvocable(invocationContext)
{
    [Action("Prepare Content", Description = "Takes a file and leverages existing content in a lake to prepare it for further translation. Also applies relevant language assets.")]
    public async Task<LeverageOutput> Leverage([ActionParameter] LakeInput lake, [ActionParameter] LeverageInput input)
    {
        using var fileStream = await fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await fileStream.GetByteData();

        var request = new RestRequest($"/lakes/{lake.LakeId}/leverage", Method.Post);
        request.AddFile("file", fileBytes, input.File.Name, input.File.ContentType);

        request.AddParameter("sourceExternalContentId", input.SourceContentId);
        request.AddParameter("variant", input.TargetVariant);

        var result = await Client.ExecuteWithErrorHandling(request);

        var contentTypeHeader = result.ContentHeaders?.FirstOrDefault(h => h.Name is not null && h.Name.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))?.Value?.ToString() ?? throw new InvalidOperationException("Missing Content-Type header.");
        var mediaType = MediaTypeHeaderValue.Parse(contentTypeHeader);
        var boundary = HeaderUtilities.RemoveQuotes(mediaType.Boundary).Value ?? throw new InvalidOperationException("Missing multipart boundary in Content-Type.");

        using var rawStream = new MemoryStream(result.RawBytes);
        var reader = new MultipartReader(boundary, rawStream);

        MetricsDto? metrics = null;
        string? xliffFileName = null;
        MemoryStream? xliffBuffer = null;

        for (MultipartSection? section = await reader.ReadNextSectionAsync(); section is not null; section = await reader.ReadNextSectionAsync())
        {
            // Content-Type of each section
            var sectionContentType = section.ContentType;

            // Content-Disposition may contain filename for the file part
            ContentDispositionHeaderValue? cd = null;
            if (section.Headers.TryGetValue(HeaderNames.ContentDisposition, out var cdValues))
            {
                if (ContentDispositionHeaderValue.TryParse(cdValues.ToString(), out var parsed)) cd = parsed;
            }

            if (sectionContentType?.StartsWith("application/json", StringComparison.OrdinalIgnoreCase) == true)
            {
                // Metrics part
                using var sr = new StreamReader(section.Body);
                var json = await sr.ReadToEndAsync();
                metrics = JsonSerializer.Deserialize<MetricsDto>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            }
            else if (sectionContentType?.StartsWith("application/xliff+xml", StringComparison.OrdinalIgnoreCase) == true
                     || sectionContentType?.StartsWith("text/xml", StringComparison.OrdinalIgnoreCase) == true
                     || (cd?.FileName.HasValue ?? false))
            {
                // File part (XLIFF). Buffer it to memory for your uploader.
                xliffBuffer = new MemoryStream();
                await section.Body.CopyToAsync(xliffBuffer);
                xliffBuffer.Position = 0;

                // Prefer filename from the part; fall back to previous logic
                xliffFileName =
                    cd?.FileNameStar.Value ??
                    cd?.FileName.Value ??
                    ContentDispositionHelper.GetFileName(result.ContentHeaders.FirstOrDefault(h => h.Name == "Content-Disposition").Value?.ToString()) ?? 
                    (input.File.Name.Split('.')[0] + ".xlf");
            }
        }

        if (xliffBuffer is null)
            throw new InvalidOperationException("No XLIFF file part found in multipart response.");
        if (metrics is null)
            throw new InvalidOperationException("No metrics JSON part found in multipart response.");

        using (xliffBuffer)
        {
            var uploaded = await fileManagementClient.UploadAsync(xliffBuffer, "application/xliff+xml", xliffFileName!);
            return new LeverageOutput
            {
                File = uploaded,
                LeveragedWords = metrics.TotalLeveragedWords,
                TotalWords = metrics.TotalWords,
            };
        }
    }

    [Action("Store Content", Description = "Store a file in a lake. This can be any interoperable supported file type, including monolingual files.")]
    public async Task Commit([ActionParameter] LakeInput lake, [ActionParameter] CommitInput input)
    {
        using var fileStream = await fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await fileStream.GetByteData();

        var request = new RestRequest($"/lakes/{lake.LakeId}/commit", Method.Post);
        request.AddFile("file", fileBytes, input.File.Name, input.File.ContentType);
        request.AddParameter("preview", false);
        request.AddParameter("workflow", InvocationContext.Bird?.Name);
        request.AddParameter("workflowReference", InvocationContext.Flight?.Url);


        await Client.ExecuteWithErrorHandling(request);
    }
}
