namespace Intercon.Application.DataTransferObjects.Comment;

public record EditCommentDto
{
    public int Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public int AuthorId { get; init; }
}