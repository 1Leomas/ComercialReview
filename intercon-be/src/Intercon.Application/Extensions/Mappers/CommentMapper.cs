using Intercon.Application.DataTransferObjects.Comment;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain.Entities;

namespace Intercon.Application.Extensions.Mappers;

public static class CommentMapper
{
    public static CommentDetailsDto ToDetailsDto(this Comment comment, int? currentUserId = null)
    {
        return new CommentDetailsDto
        {
            Id = comment.Id,
            Text = comment.Text,
            LikesCount = comment.Likes.Count,
            CurrentUserLiked = currentUserId is not null && comment.Likes.Any(x => x.UserId == currentUserId),
            IsCommentOfBusinessOwner = comment.IsCommentOfBusinessOwner,

            AuthorId = comment.AuthorId,
            ReviewAuthorId = comment.ReviewAuthorId,
            BusinessId = comment.BusinessId,

            Author = new UserPublicDetailsDto
            {
                Id = comment.Author.Id,
                FirstName = comment.Author.FirstName,
                LastName = comment.Author.LastName,
                UserName = comment.Author.UserName,
                AvatarPath = comment.Author.Avatar?.Path
            },

            CreatedDate = comment.CreatedDate,
            UpdatedDate = comment.UpdatedDate,
        };
    }
}