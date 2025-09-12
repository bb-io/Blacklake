using Apps.Blacklake.Helpers;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Blacklake.Actions;

[ActionList("Content")]
public class Actions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : BlacklakeInvocable(invocationContext)
{
    [Action("Apply lake", Description = "Takes a file and leverages the file with the data in a lake to prepare it for further translation")]
    public async Task<LeverageOutput> Leverage([ActionParameter] LakeInput lake, [ActionParameter] LeverageInput input)
    {
        using var fileStream = await fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await fileStream.GetByteData();

        var request = new RestRequest($"/lakes/{lake.LakeId}/leverage", Method.Post);
        request.AddFile("file", fileBytes, input.File.Name, input.File.ContentType);

        request.AddParameter("sourceExternalContentId", input.SourceContentId);
        request.AddParameter("variant", input.TargetVariant);

        var result = await Client.ExecuteWithErrorHandling(request);

        var fileData = result.RawBytes;
        var filenameHeader = result.ContentHeaders.First(h => h.Name == "Content-Disposition");
        var filename = ContentDispositionHelper.GetFileName(filenameHeader.Value.ToString()) ?? (input.File.Name.Split('.')[0] + ".xlf");
        using var stream = new MemoryStream(fileData);
        var file = await fileManagementClient.UploadAsync(stream, "application/xliff+xml", filename);
        return new() { File = file };
    }

    [Action("Add to lake", Description = "Commit a file to a lake. This can be any interoperable supported file type, including monolingual files.")]
    public async Task Commit([ActionParameter] LakeInput lake, [ActionParameter] CommitInput input)
    {
        using var fileStream = await fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await fileStream.GetByteData();

        var request = new RestRequest($"/lakes/{lake.LakeId}/commit", Method.Post);
        request.AddFile("file", fileBytes, input.File.Name, input.File.ContentType);
        request.AddParameter("preview", false);

        await Client.ExecuteWithErrorHandling(request);
    }
}
