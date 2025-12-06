namespace Apps.Blacklake.Dto;
public class VariantDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string DefaultCode { get; set; }
    public List<string> AllCodes { get; set; }
}
