using System.ComponentModel.DataAnnotations;

namespace URLShortener.Data.Entity;

public class UrlLog
{
    [Key] public required string ShortUrl { get; set; }
    public string LongUrl { get; set; }
    public long CreatedAt { get; set; }
    public long? ExpiresAt { get; set; }
    public int ClickCount { get; set; }
    public string UserId { get; set; }
}