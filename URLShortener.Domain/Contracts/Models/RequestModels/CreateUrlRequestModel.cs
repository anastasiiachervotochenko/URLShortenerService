namespace URLShortener.Domain.Contracts.Models.RequestModels;

public class CreateUrlRequestModel
{
    public string Url { get; set; }
    public string UserId { get; set; }
    public string? ShortUrl { get; set; }
    public int? TimeToLive { get; set; }
}