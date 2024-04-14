using Intercon.Application.DataTransferObjects.Comment;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain.Entities;

namespace Intercon.Application.Extensions.Mappers;

public static class CommentMapper
{
    public static CommentDetailsDto ToDetailsDto(this Comment comment)
    {
        return new CommentDetailsDto
        {
            Id = comment.Id,
            Text = comment.Text,

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