using FluentValidation;
using URLShortener.Domain.Contracts.Interfaces;
using URLShortener.Domain.Contracts.Models.DomainModels;
using URLShortener.Domain.Contracts.Models.RequestModels;
using URLShortener.Domain.Exceptions;

namespace URLShortener.Manager;

public class AppManager : IAppManager
{
    private readonly IAppService _appService;
    private readonly IUrlService _urlService;
    private readonly IValidator<CreateUserRequestModel> _createUserValidator;
    private readonly IValidator<CreateUrlRequestModel> _createUrlValidator;
    private const string UrlUserException = "This user doesn't have an access to modify the URL";

    public AppManager(IAppService appService, IUrlService urlService,
        IValidator<CreateUserRequestModel> createUserValidator,
        IValidator<CreateUrlRequestModel> createUrlValidator)
    {
        _appService = appService;
        _urlService = urlService;
        _createUserValidator = createUserValidator;
        _createUrlValidator = createUrlValidator;
    }

    public async Task<UserModel> GetUserByIdAsync(string id)
    {
        var result = await _appService.GetUserByIdAsync(id);

        return result;
    }

    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        var result = await _appService.GetAllUsersAsync();

        return result;
    }

    public async Task CreateUserAsync(CreateUserRequestModel userModel)
    {
        var validationResult = await _createUserValidator.ValidateAsync(userModel);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await _appService.CreateUserAsync(userModel);
    }

    public async Task<string> GetUrlByIdAsync(string id)
    {
        var result = await _urlService.GetUrlByIdAsync(id);
        await _urlService.UpdateClickValueAsync(id);

        return result.LongUrl;
    }

    public async Task<List<UrlLogModel>> GetAllUrlsAsync()
    {
        var result = await _urlService.GetAllUrlsAsync();

        return result;
    }

    public async Task CreateUrlAsync(CreateUrlRequestModel createUrlModel)
    {
        var validationResult = await _createUrlValidator.ValidateAsync(createUrlModel);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await _appService.GetUserByIdAsync(createUrlModel.UserId);

        await _urlService.CreateUrlAsync(createUrlModel);
    }

    public async Task DeleteUrlAsync(string userId, string urlId)
    {
        var url = await _urlService.GetUrlByIdAsync(urlId);
        if (url.UserId != userId)
        {
            throw new UrlException(UrlUserException);
        }

        await _urlService.DeleteUrlAsync(urlId);
    }
}