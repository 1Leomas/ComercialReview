using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.FilesManagement.DeleteFile;
using MediatR;

namespace Intercon.Application.BusinessesManagement.SetBusinessLogoId;

public sealed record SetBusinessLogoIdCommand(int CurrentUserId, int BusinessId, int LogoId) : ICommand;

internal sealed class SetBusinessLogoIdCommandHandler(
    IBusinessRepository businessRepository,
    IMediator mediator) : ICommandHandler<SetBusinessLogoIdCommand>
{
    public async Task Handle(SetBusinessLogoIdCommand command, CancellationToken cancellationToken)
    {
        var business = await businessRepository.GetBusinessByIdAsync(command.BusinessId, cancellationToken);

        if (business == null)
        {
            throw new InvalidOperationException("Business not found");
        }

        if (business.OwnerId != command.CurrentUserId)
        {
            throw new InvalidOperationException("Current user is not the owner of this business");
        }

        if (business.LogoId.HasValue)
        {
            await mediator.Send(new DeleteFileQuery(business.LogoId.Value), cancellationToken);
        }

        await businessRepository.SetBusinessLogoIdAsync(command.BusinessId, command.LogoId, cancellationToken);
    }
}