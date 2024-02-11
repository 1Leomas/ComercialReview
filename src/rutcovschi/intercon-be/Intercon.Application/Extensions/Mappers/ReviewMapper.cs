using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.ReviewsManagement.GetReview;
using Intercon.Domain.Entities;

namespace Intercon.Application.Extensions.Mappers;

public static class ReviewMapper
{
    public static Review ToEntity(this ReviewDto review)
    {
        return new Review()
        {
            
        };
    }

    public static ReviewDto ToDto(this Review review)
    {
        return new ReviewDto(
            Id : review.Id, 
            BusinessId : review.BusinessId, 
            AuthorId : review.AuthorId, 
            Author : new UserDto() 
            { 
                Id = review.AuthorId,
                FirstName = review.Author.FirstName,
                LastName = review.Author.LastName,
                Email = review.Author.Email
            },
            Grade : review.Grade,
            ReviewText : review.ReviewText);
    }
}