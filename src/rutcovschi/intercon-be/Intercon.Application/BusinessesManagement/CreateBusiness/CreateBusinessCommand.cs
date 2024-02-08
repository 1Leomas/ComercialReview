using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Application.Extensions;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.BusinessesManagement.CreateBusiness;

public sealed record CreateBusinessCommand(CreateBusinessDto business) : ICommand
{
    public CreateBusinessDto Data { get; init; } = business;
}

public sealed class CreateBusinessCommandHandler(InterconDbContext context) : ICommandHandler<CreateBusinessCommand>
{
    public async Task Handle(CreateBusinessCommand command, CancellationToken cancellationToken)
    {
        var userExits = await context.Users.AnyAsync(x => x.Id == command.Data.OwnerId, cancellationToken);
       
        if (!userExits)
        {
            throw new Exception("User not found");
        }

        // maybe make a separate request for this
        if (command.Data.Image != null)
        {
            await context.Images.AddAsync(command.Data.Image, cancellationToken);
        }

        var businessDb = command.Data.ToEntity();

        await context.Businesses.AddAsync(businessDb, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}