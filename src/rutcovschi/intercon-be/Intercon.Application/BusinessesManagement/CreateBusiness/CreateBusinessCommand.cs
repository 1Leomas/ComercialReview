using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Application.Extensions.Mappers;
using Intercon.Application.FilesManagement.UploadFile;
using Intercon.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Intercon.Application.BusinessesManagement.CreateBusiness;

public sealed record CreateBusinessCommand(int UserId, CreateBusinessDto Data) : ICommand<BusinessDetailsDto>;

public sealed class CreateBusinessCommandHandler(
        IMediator mediator,
        IBusinessRepository businessRepository,
        ILogger<CreateBusinessCommandHandler> logger)
    : ICommandHandler<CreateBusinessCommand, BusinessDetailsDto>
{
    public async Task<BusinessDetailsDto> Handle(CreateBusinessCommand command, CancellationToken cancellationToken)
    {
        FileDataDto? fileData = null!;

        if (command.Data.Logo is not null)
        {
            fileData = await mediator.Send(new UploadFileCommand(command.Data.Logo), cancellationToken);
        }

        var businessDb = new Business
        {
            OwnerId = command.UserId,
            Title = command.Data.Title,
            ShortDescription = command.Data.ShortDescription,
            FullDescription = command.Data.FullDescription,
            Address = command.Data.Address,
            Category = command.Data.Category,
            LogoId = fileData?.Id
        };

        await businessRepository.CreateBusinessAsync(businessDb, cancellationToken);

        return businessDb.ToDetailsDto();
    }
}