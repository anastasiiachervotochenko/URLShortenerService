using System.Text.RegularExpressions;
using FluentValidation;
using URLShortener.Domain.Contracts.Models.RequestModels;

namespace URLShortener.Validators;

public class CreateUrlRequestModelValidator : AbstractValidator<CreateUrlRequestModel>
{
    public CreateUrlRequestModelValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("URL is required")
            .Matches(new Regex(@"[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)"))
            .WithMessage("Provided URL is invalid");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User Id is required");

        RuleFor(x => x.ShortUrl)
            .Matches(new Regex(@"^[a-zA-Z0-9]+$"))
            .WithMessage("Provided ShortUrl is invalid");

        RuleFor(x => x.TimeToLive)
            .GreaterThan(0).WithMessage("TimeToLive must be greater than zero.")
            .LessThanOrEqualTo(86400).WithMessage("TimeToLive must not exceed 86400 seconds (24 hours).");
    }
}