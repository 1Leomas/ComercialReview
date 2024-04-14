using Intercon.Application.DataTransferObjects.User;

namespace Intercon.Application.DataTransferObjects.Comment;

public record CommentDetailsDto
{
    public int Id { get; init; }
    public string Text { get; init; } = string.Empty;

    public int BusinessId { get; init; }
    public int ReviewAuthorId { get; init; }
    public int AuthorId { get; init; }

    public UserPublicDetailsDto Author { get; init; } = null!;

    public DateTime CreatedDate { get; init; }
    public DateTime UpdatedDate { get; init; }
    public bool WasEdited => UpdatedDate != CreatedDate;
}