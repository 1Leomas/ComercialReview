using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.ReviewsManagement.GetAllReviews;
using Intercon.Application.ReviewsManagement.GetReviewDetails;
using Intercon.Domain.Entities;

namespace Intercon.Application.Extensions.Mappers;

public static class ReviewMapper
{
    public static Review ToEntity(this ReviewDetailsDto reviewDetails)
    {
        return new Review();
    }

    public static ReviewDetailsDto ToDetailedDto(this Review review)
    {
        return new ReviewDetailsDto(
            review.BusinessId,
            review.AuthorId,
            new ReviewAuthorDto(
                review.Author.FirstName,
                review.Author.LastName,
                review.Author.UserName,
                review.Author.Avatar?.Path),
            review.Grade,
            review.ReviewText,
            (int)review.Recommendation,
            review.CommentsCount,
            review.CreatedDate,
            review.UpdatedDate,
            review.WasEdited);
    }

    public static ReviewShortDto ToShortDto(this Review review)
    {
        return new ReviewShortDto(
            review.BusinessId,
            review.AuthorId,
            review.Grade,
            review.ReviewText,
            (int)review.Recommendation);
    }

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