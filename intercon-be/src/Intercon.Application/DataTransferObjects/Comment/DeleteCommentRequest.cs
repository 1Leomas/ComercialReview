using Intercon.Domain.Enums;

namespace Intercon.Application.DataTransferObjects.Comment;

public record DeleteCommentRequest
{
    public int Id { get; init; }
    public int CurrentUserId { get; init; }
    public Role CurrentUserRole { get; init; }
}