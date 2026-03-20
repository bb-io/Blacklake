namespace Apps.Blacklake.Dto;
public class ContentChangeAndRulesDto
{
    public IEnumerable<RuleDto> Rules { get; set; } = [];
    public IEnumerable<ContentChangeDto> Changes { get; set; } = [];
}

public class RuleDto
{
    public string? Category { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class ContentChangeDto
{
    public string ContentId { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? ExternalId { get; set; }
    public string? VariantId { get; set; }
    public IEnumerable<ChangeDto> Changes { get; set; } = [];
}

public class ChangeDto
{
    public string? Key { get; set; }
    public IEnumerable<AddedUnitDto> Added { get; set; } = [];
    public IEnumerable<UpdatedUnitDto> Updated { get; set; } = [];
    public IEnumerable<RemovedUnitDto> Removed { get; set; } = [];
}

public class AddedUnitDto
{
    public string Text { get; set; } = string.Empty;
    public string? SourceText { get; set; }
}

public class UpdatedUnitDto
{
    public string Id { get; set; } = string.Empty;
    public string OldText { get; set; } = string.Empty;
    public string NewText { get; set; } = string.Empty;
    public int EditDistance { get; set; }
}

public class RemovedUnitDto
{
    public string Id { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
