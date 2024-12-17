using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using URLShortener.Data;
using URLShortener.Data.Entity;
using URLShortener.Domain.Contracts.Interfaces;
using URLShortener.Domain.Contracts.Models.DomainModels;
using URLShortener.Domain.Contracts.Models.RequestModels;
using URLShortener.Domain.Exceptions;

namespace URLShortener.Domain.Services;

public class UrlService : IUrlService
{
    private const int UrlSize = 5;

    private static readonly char[] Characters =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    private static readonly string[] ReservedUrlNames =
    [
        "All",
        "User",
        "Update",
        "Create"
    ];

    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UrlService> _logger;

    private const string UrlNotFountException = "404 Not Found";
    private const string UniqueUrlError = "URL with same name already exists";
    private const string FailedToCreateUrlError = "Failed to create URL. Try again later.";

    public UrlService(AppDbContext context, IMapper mapper, ILogger<UrlService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UrlLogModel> GetUrlByIdAsync(string id)
    {
        var currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var urlLog = await _context.UrlLog.FirstOrDefaultAsync((u) => u.ShortUrl.Equals(id)
                                                                      && (u.ExpiresAt == null ||
                                                                          u.ExpiresAt > currentUnixTime));

        if (urlLog == null)
        {
            throw new NotFoundException(UrlNotFountException);
        }

        return _mapper.Map<UrlLog, UrlLogModel>(urlLog);
    }

    public async Task<List<UrlLogModel>> GetAllUrlsAsync()
    {
        var urlLogs = await _context.UrlLog.ToListAsync();
        var mappedUrls = _mapper.Map<List<UrlLog>, List<UrlLogModel>>(urlLogs);

        return mappedUrls;
    }

    public async Task CreateUrlAsync(CreateUrlRequestModel createUrlModel)
    {
        var urlLogModel = new UrlLogModel(createUrlModel.Url, createUrlModel.UserId, createUrlModel.TimeToLive);
        string shortUrl;

        if (!string.IsNullOrWhiteSpace(createUrlModel.ShortUrl))
        {
            if (await IsShortUrlReserved(createUrlModel.ShortUrl))
            {
                throw new DuplicateUrlException(UniqueUrlError);
            }

            shortUrl = createUrlModel.ShortUrl;
        }
        else
        {
            shortUrl = await GenerateUniqueShortUrlAsync();
        }

        urlLogModel.ShortUrl = shortUrl;
        _context.UrlLog.Add(_mapper.Map<UrlLogModel, UrlLog>(urlLogModel));
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUrlAsync(string id)
    {
        var urlLog = _context.UrlLog.First((u) => u.ShortUrl.Equals(id));
        _context.UrlLog.Remove(urlLog);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateClickValueAsync(string id)
    {
        var urlLog = await _context.UrlLog.FirstOrDefaultAsync((u) => u.ShortUrl.Equals(id));

        if (urlLog == null)
        {
            throw new NotFoundException(UrlNotFountException);
        }

        urlLog.ClickCount++;
        _context.UrlLog.Update(urlLog);

        await _context.SaveChangesAsync();
    }

    public async Task<int> CleanupExpiredUrlsAsync()
    {
        var currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var expiredUrls = _context.UrlLog
            .Where(u => u.ExpiresAt != null && u.ExpiresAt < currentUnixTime);
        var count = await expiredUrls.CountAsync();

        _context.UrlLog.RemoveRange(expiredUrls);

        await _context.SaveChangesAsync();

        return count;
    }
    
    private async Task<string> GenerateUniqueShortUrlAsync()
    {
        for (var i = 0; i < 10; i++)
        {
            var shortCode = GenerateUniqueId();
            if (await IsShortUrlReserved(shortCode))
            {
                return shortCode;
            }
        }

        _logger.LogWarning("Generation of unique short url fail");
        throw new UrlException(FailedToCreateUrlError);
    }
    
    private string GenerateUniqueId()
    {
        var uniqueId = new StringBuilder(UrlSize);
        var random = new Random();

        for (var i = 0; i < UrlSize; i++)
        {
            var index = random.Next(Characters.Length);
            uniqueId.Append(Characters[index]);
        }

        return uniqueId.ToString();
    }

    private async Task<bool> IsShortUrlReserved(string url)
    {
        var existInReservedUrls = ReservedUrlNames.Contains(url);
        var existInDatabase = await _context.UrlLog.AnyAsync((log) => log.ShortUrl.Equals(url));

        return existInDatabase || existInReservedUrls;
    }
}