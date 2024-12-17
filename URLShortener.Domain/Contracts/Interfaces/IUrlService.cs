using URLShortener.Domain.Contracts.Models.DomainModels;
using URLShortener.Domain.Contracts.Models.RequestModels;

namespace URLShortener.Domain.Contracts.Interfaces;

public interface IUrlService
{
    public Task<UrlLogModel> GetUrlByIdAsync(string id);
    public Task<List<UrlLogModel>> GetAllUrlsAsync();
    public Task CreateUrlAsync(CreateUrlRequestModel createUrlModel);
    public Task DeleteUrlAsync(string id);
    public Task UpdateClickValueAsync(string id);
    public Task<int> CleanupExpiredUrlsAsync();
}