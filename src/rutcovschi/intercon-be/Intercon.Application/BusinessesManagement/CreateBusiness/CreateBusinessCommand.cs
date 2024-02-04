using FluentValidation;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Domain;
using Intercon.Domain.ComplexTypes;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;
using Intercon.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.BusinessesManagement.CreateBusiness;

public record CreateBusinessDto(int OwnerId, string Title, string ShortDescription, string? FullDescription, Image? Image, Address Address, BusinessCategory Category);

public sealed record CreateBusinessCommand : ICommand
{
    public CreateBusinessCommand(CreateBusinessDto business)
    {
        OwnerId = business.OwnerId;
        Title = business.Title;
        ShortDescription = business.ShortDescription;
        FullDescription = business.FullDescription;
        Image = business.Image;
        Address = business.Address;
        Category = business.Category;
    }

    public int OwnerId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string ShortDescription { get; init; } = string.Empty;
    public string? FullDescription { get; init; } = string.Empty;
    public Image? Image { get; init; }
    public Address Address { get; init; }
    public BusinessCategory Category { get; init; }
}

public sealed class CreateBusinessCommandHandler(InterconDbContext context) : ICommandHandler<CreateBusinessCommand>
{
       public async Task Handle(CreateBusinessCommand command, CancellationToken cancellationToken)
       {
           var userExits = await context.Users.AnyAsync(x => x.Id == command.OwnerId, cancellationToken);
           
           if (!userExits)
           {
               throw new Exception("User not found");
           }

           //add image to daatabase
           
           var businessDb = new Business
           {
               OwnerId = command.OwnerId,
               Title = command.Title,
               ShortDescription = command.ShortDescription,
               FullDescription = command.FullDescription,
               Logo = command.Image,
               Address = command.Address,
               Category = command.Category
           };

           await context.Businesses.AddAsync(businessDb, cancellationToken);
           await context.SaveChangesAsync(cancellationToken);
       }
}

public class CreateBusinessCommandValidator : AbstractValidator<CreateBusinessCommand>
{
    public CreateBusinessCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
        RuleFor(x => x.ShortDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Address).NotEmpty();
    }
}