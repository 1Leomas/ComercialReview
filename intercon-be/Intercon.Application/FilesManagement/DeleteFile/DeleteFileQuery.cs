using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Exceptions;

namespace Intercon.Application.FilesManagement.DeleteFile;

public sealed record DeleteFileQuery(int Id) : IQuery;

internal sealed class DeleteFileQueryHandler(
    IFileRepository fileRepository,
    IBlobStorage blobStorage) : IQueryHandler<DeleteFileQuery>
{
    public async Task Handle(DeleteFileQuery request, CancellationToken cancellationToken)
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