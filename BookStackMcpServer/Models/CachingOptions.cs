namespace BookStackMcpServer.Models;

public class CachingOptions
{
    public const string SectionName = "Caching";
    
    public bool Enabled { get; set; } = true;
    public int AbsoluteExpirationMinutes { get; set; } = 5;
    public int SlidingExpirationMinutes { get; set; } = 2;
}
