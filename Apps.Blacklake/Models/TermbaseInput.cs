using Apps.Blacklake.DataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Blacklake.Models;
public class TermbaseInput
{
    [Display("Termbase file")]
    public FileReference TermbaseFile { get; set; }

    [Display("Termbase ID", Description = "If selected, this termbase will be updated instead of a new one being created.")]
    [DataSource(typeof(TermbaseDataHandler))]
    public string? TermbaseId { get; set; }

    [Display("Delete missing", Description = "If updating a termbase and this value is true, Blacklake will delete entries in its termbase that don't appear in the file.")]
    public bool? DeleteMissingTermsAndGroups { get; set; }
}
