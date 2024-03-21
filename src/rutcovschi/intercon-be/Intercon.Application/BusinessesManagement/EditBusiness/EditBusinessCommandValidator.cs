using FluentValidation;
using Intercon.Application.Abstractions;

namespace Intercon.Application.BusinessesManagement.EditBusiness;

public class EditBusinessCommandValidator : AbstractValidator<EditBusinessCommand>
{
    public EditBusinessCommandValidator(IBusinessRepository businessRepository)
    {
        RuleFor(x => x.BusinessId).NotEmpty();

        RuleFor(x => x.BusinessId)
            .MustAsync(businessRepository.BusinessExistsAsync);

        When(x => !string.IsNullOrEmpty(x.Data.Title), () =>
        {
            RuleFor(x => x.Data.Title)
                .MaximumLength(255)
                .WithName(x => nameof(x.Data.Title));
        });
        When(x => !string.IsNullOrEmpty(x.Data.ShortDescription), () =>
        {
            RuleFor(x => x.Data.ShortDescription)
                .MaximumLength(500)
                .WithName(x => nameof(x.Data.ShortDescription));
        });
        When(x => x.Data.Address != null, () =>
        {
            RuleFor(x => x.Data.Address!.Street)
                .MaximumLength(255)
                .WithName(x => nameof(x.Data.Address.Street));

            RuleFor(x => x.Data.Address!.Latitude)
                .MaximumLength(255)
                .WithName(x => nameof(x.Data.Address.Latitude));

            RuleFor(x => x.Data.Address!.Longitude)
                .MaximumLength(255)
                .WithName(x => nameof(x.Data.Address.Longitude));
        });
        When(x => x.Data.Image != null, () =>
        {
            RuleFor(x => x.Data.Image!.Data)
                .NotEmpty()
                .WithName(x => nameof(x.Data.Image.Data));
        });
    }
}