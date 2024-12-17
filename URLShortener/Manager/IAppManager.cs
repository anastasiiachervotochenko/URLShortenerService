using URLShortener.Domain.Contracts.Models.DomainModels;
using URLShortener.Domain.Contracts.Models.RequestModels;

namespace URLShortener.Manager;

public interface IAppManager
{
    public Task<UserModel> GetUserByIdAsync(string id);
    public Task<List<UserModel>> GetAllUsersAsync();
    public Task CreateUserAsync(CreateUserRequestModel userModel);
    public Task<string> GetUrlByIdAsync(string id);
    public Task<List<UrlLogModel>> GetAllUrlsAsync();
    public Task CreateUrlAsync(CreateUrlRequestModel createUrlModel);
    public Task DeleteUrlAsync(string userId, string urlId);
}