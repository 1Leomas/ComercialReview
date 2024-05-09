using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;

namespace Intercon.Application.ReviewsManagement.AddLike;

public sealed record AddReviewLikeCommand(int BusinessId, int ReviewAuthorId, int CurrentUserId)
    : ICommand<int>;

internal sealed class AddReviewLikeCommandHandler(IReviewLikeRepository reviewLikeRepository)
    : ICommandHandler<AddReviewLikeCommand, int>
{
    public async Task<int> Handle(AddReviewLikeCommand request, CancellationToken cancellationToken)
    {
        return await reviewLikeRepository.Add(request.BusinessId, request.ReviewAuthorId, request.CurrentUserId);
    }
}