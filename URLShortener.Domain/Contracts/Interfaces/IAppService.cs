using URLShortener.Domain.Contracts.Models.DomainModels;
using URLShortener.Domain.Contracts.Models.RequestModels;

namespace URLShortener.Domain.Contracts.Interfaces;

public interface IAppService
{
    public Task<UserModel> GetUserByIdAsync(string id);
    public Task<List<UserModel>> GetAllUsersAsync();
    public Task CreateUserAsync(CreateUserRequestModel userModel);
}