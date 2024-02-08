using FluentValidation;

namespace Intercon.Application.BusinessesManagement.CreateBusiness;

public class CreateBusinessCommandValidator : AbstractValidator<CreateBusinessCommand>
{
    public CreateBusinessCommandValidator()
    {
        RuleFor(x => x.Data.Title)
            .NotEmpty()
            .MaximumLength(255)
            .WithName(x => nameof(x.Data.Title));
        RuleFor(x => x.Data.ShortDescription)
            .NotEmpty()
            .MaximumLength(500)
            .WithName(x => nameof(x.Data.ShortDescription));
        RuleFor(x => x.Data.Address)
            .NotEmpty()
            .WithName(x => nameof(x.Data.Address));
    }
}