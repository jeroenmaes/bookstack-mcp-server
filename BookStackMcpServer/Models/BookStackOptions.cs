namespace BookStackMcpServer.Models;

public class BookStackOptions
{
    public const string SectionName = "BookStack";
    
    public string BaseUrl { get; set; } = string.Empty;
    public bool EnableWrite { get; set; } = false;
}