using Intercon.Application.Abstractions.Messaging;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.ReviewsManagement.DeleteReview;

public sealed record DeleteReviewCommand(int businessId, int userId) : ICommand;

internal sealed class DeleteReviewCommandHandler(InterconDbContext context) : ICommandHandler<DeleteReviewCommand>
{
    private readonly InterconDbContext _context = context;

    public async Task Handle(DeleteReviewCommand command, CancellationToken cancellationToken)
    {
        await _context.Reviews
            .Where(x => x.BusinessId == command.businessId && x.AuthorId == command.userId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}