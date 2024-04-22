namespace Intercon.Application.DataTransferObjects.Comment;

public record AddCommentDto
{
    public string Text { get; init; } = string.Empty;

    public int BusinessId { get; init; }
    public int ReviewAuthorId { get; init; }
    public int AuthorId { get; init; }
}