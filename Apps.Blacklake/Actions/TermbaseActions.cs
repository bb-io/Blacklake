using Apps.Blacklake.Dto;
using Apps.Blacklake.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Blacklake.Actions;

[ActionList("Termbases")]
public class TermbaseActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : BlacklakeInvocable(invocationContext)
{  
    [Action("Upload Termbase", Description = "Upload a termbase to Blacklake. Optionally create update an existing termbase with its content")]
    public async Task<TermbaseOutput> UploadTermbase([ActionParameter] LakeInput lake, [ActionParameter] TermbaseInput input)
    {
        using var fileStream = await fileManagementClient.DownloadAsync(input.TermbaseFile);
        var fileBytes = await fileStream.GetByteData();

        var request = new RestRequest($"/lakes/{lake.LakeId}/termbases/import", Method.Post);
        if (input.TermbaseId is not null)
        {
            request = new RestRequest($"/lakes/{lake.LakeId}/termbases/{input.TermbaseId}/import", Method.Post);
            if (input.DeleteMissingTermsAndGroups.HasValue)
            {
                request.AddParameter("deleteMissingTermsAndGroups", input.DeleteMissingTermsAndGroups.Value);
            }            
        }
        request.AddFile("file", fileBytes, input.TermbaseFile.Name, input.TermbaseFile.ContentType);

        var result = await Client.ExecuteWithErrorHandling<TermbaseDto>(request);

        return new TermbaseOutput
        {
            TermbaseId = result.Id,
            Name = result.Name,
            Description = result.Description,
        };
    }
}
