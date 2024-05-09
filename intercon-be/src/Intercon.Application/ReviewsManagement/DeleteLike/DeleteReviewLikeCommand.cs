using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;

namespace Intercon.Application.ReviewsManagement.DeleteLike;

public sealed record DeleteReviewLikeCommand(int BusinessId, int ReviewAuthorId, int CurrentUserId)
    : ICommand<int>;

internal sealed class DeleteReviewLikeCommandHandler(IReviewLikeRepository reviewLikeRepository)
    : ICommandHandler<DeleteReviewLikeCommand, int>
{
    public async Task<int> Handle(DeleteReviewLikeCommand request, CancellationToken cancellationToken)
    {
        return await reviewLikeRepository.Delete(request.BusinessId, request.ReviewAuthorId, request.CurrentUserId);
    }
}