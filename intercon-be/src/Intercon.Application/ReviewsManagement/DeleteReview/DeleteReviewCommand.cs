﻿using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;

namespace Intercon.Application.ReviewsManagement.DeleteReview;

public sealed record DeleteReviewCommand(int businessId, int userId) : ICommand;

internal sealed class DeleteReviewCommandHandler
    (IReviewRepository reviewRepository) : ICommandHandler<DeleteReviewCommand>
{
    public async Task Handle(DeleteReviewCommand command, CancellationToken cancellationToken)
    {
        await reviewRepository.DeleteReviewAsync(command.businessId, command.userId, cancellationToken);
    }
}