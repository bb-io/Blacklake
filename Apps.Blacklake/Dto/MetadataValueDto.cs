using System.Text.Json;

namespace Apps.Blacklake.Dto;

public class MetadataValueDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public JsonElement Value { get; set; }
}
