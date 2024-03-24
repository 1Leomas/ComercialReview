using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;

namespace Intercon.Application.FilesManagement.DeleteFile;

public sealed record DeleteFileQuery(int Id) : IQuery;

internal sealed class DeleteFileQueryHandler
    (IFileRepository fileRepository) : IQueryHandler<DeleteFileQuery>
{
    public async Task Handle(DeleteFileQuery request, CancellationToken cancellationToken)
    {
        await fileRepository.DeleteFileAsync(request.Id, cancellationToken);
    }
}