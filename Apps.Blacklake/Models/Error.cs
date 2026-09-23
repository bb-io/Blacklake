namespace Apps.Blacklake.Models;

public class Error
{
    public string Title { get; set; }
    public string Detail { get; set; }
    
    public int Status { get; set; }
    
    public Dictionary<string, List<string>> Errors { get; set; }
}