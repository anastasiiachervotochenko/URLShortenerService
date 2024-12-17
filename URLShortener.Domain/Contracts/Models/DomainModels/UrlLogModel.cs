namespace URLShortener.Domain.Contracts.Models.DomainModels;

public class UrlLogModel
{
    public string ShortUrl { get; set; }
    public string LongUrl { get; set; }
    public long CreatedAt { get; set; }
    public long? ExpiresAt { get; set; }
    public int ClickCount { get; set; }
    public string UserId { get; set; }

    public UrlLogModel(string longUrl, string userId, int? expiresAt = null)
    {
        LongUrl = longUrl;
        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        ClickCount = 0;
        UserId = userId;
        if (expiresAt.HasValue)
        {
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(expiresAt.Value).ToUnixTimeSeconds();
        }
    }
}