namespace URLShortener.Domain.Contracts.Models.RequestModels;

public class CreateUserRequestModel
{
    public string Name { get; set; }
    public string Email { get; set; }
}