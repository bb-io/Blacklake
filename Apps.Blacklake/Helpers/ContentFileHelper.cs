using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Filters.Constants;
using Blackbird.Filters.Enums;
using Blackbird.Filters.Transformations;

namespace Apps.Blacklake.Helpers;

public static class ContentFileHelper
{
    public static async Task<(byte[] Bytes, string Name, string MediaType)> ApplyMetadataOverrides(
        Stream file,
        FileReference reference,
        Action<Transformation, bool> applyOverrides)
    {
        var bytes = await file.GetByteData();
        var unchangedFile = (bytes, reference.Name, reference.ContentType);

        var loaded = Transformation.Load(new MemoryStream(bytes), reference.Name, reference.ContentType);
        if (!loaded.Success)
            return unchangedFile;

        var transformation = loaded.Value;
        var metadata = Metadata(transformation);
        applyOverrides(transformation, loaded.WasBilingual);

        if (Metadata(transformation) == metadata)
            return unchangedFile;

        if (loaded.WasBilingual)
            return (await transformation.ToStream().GetByteData(), transformation.BilingualFileName, MediaTypes.Xliff2);

        var content = transformation.Source();
        if (!content.Success)
            return unchangedFile;

        var overriddenContent = await content.Value.ToStream(MetadataHandling.Include).GetByteData();
        return (overriddenContent, content.Value.OriginalName, content.Value.OriginalMediaType);
    }

    public static string? OverrideWith(this string? current, string? input) =>
        string.IsNullOrWhiteSpace(input) ? current : input.Trim();

    private static (string?, string?, string?, string?) Metadata(Transformation transformation) =>
        (transformation.SourceLanguage,
         transformation.TargetLanguage,
         transformation.SourceSystemReference?.ContentId,
         transformation.TargetSystemReference?.ContentId);
}
