using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.Abstractions.Services;
using Intercon.Application.Exceptions;

namespace Intercon.Application.FilesManagement.DeleteFile;

public sealed record DeleteFileCommand(int Id) : IQuery;

internal sealed class DeleteFileCommandHandler(
    IFileRepository fileRepository,
    IBlobStorage blobStorage) : IQueryHandler<DeleteFileCommand>
{
    public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var file = await fileRepository.GetByIdAsync(request.Id, cancellationToken);

        if (file == null)
        {
            throw new NotFoundException($"File with id {request.Id} not found.");
        }

        var deleteBlobResult = await blobStorage.DeleteAsync(file.Path, cancellationToken);

        if (!deleteBlobResult)
        {
            throw new Exception($"Failed to delete file with id {request.Id}.");
        }

        var deleteFromDbResult = await fileRepository.DeleteAsync(request.Id, cancellationToken);

        if (!deleteFromDbResult)
        {
            throw new Exception($"Failed to delete file with id {request.Id}.");
        }
    }
}