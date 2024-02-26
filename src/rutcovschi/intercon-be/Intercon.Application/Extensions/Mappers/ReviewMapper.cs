using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.ReviewsManagement.CreateReview;
using Intercon.Application.ReviewsManagement.GetAllReviews;
using Intercon.Application.ReviewsManagement.GetReviewDetails;
using Intercon.Domain.Entities;

namespace Intercon.Application.Extensions.Mappers;

public static class ReviewMapper
{
    public static Review ToEntity(this ReviewDetailsDto reviewDetails)
    {
        return new Review()
        {
            
        };
    }

    public static ReviewDetailsDto ToDto(this Review review)
    {
        return new ReviewDetailsDto(
            BusinessId : review.BusinessId, 
            AuthorId : review.AuthorId, 
            Author : new ReviewAuthorDto(
                FirstName : review.Author.FirstName,
                LastName : review.Author.LastName,
                UserName : review.Author.UserName,
                Avatar: review.Author.Avatar?.Data),
            Grade : review.Grade,
            ReviewText : review.ReviewText);
    }

    public static ReviewShortDto ToShortDto(this Review review) => new(
        BusinessId: review.BusinessId,
        AuthorId: review.AuthorId,
        Grade: review.Grade,
        ReviewText: review.ReviewText);

    //public static Review ToEntity(this CreateReviewDto review)
    //{
    //    return new Review()
    //    {
    //        BusinessId = review.BusinessId,
    //        AuthorId = review.AuthorId,
    //        Grade = review.Grade,
    //        ReviewText = review.ReviewText
    //    };
    //}
}