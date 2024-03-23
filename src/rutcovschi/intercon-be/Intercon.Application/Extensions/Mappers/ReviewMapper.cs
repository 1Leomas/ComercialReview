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

    public static ReviewDetailsDto ToDto(this Review review)
    {
        return new ReviewDetailsDto(
            review.BusinessId,
            review.AuthorId,
            new ReviewAuthorDto(
                review.Author.FirstName,
                review.Author.LastName,
                review.Author.UserName,
                review.Author.Avatar?.Data),
            review.Grade,
            review.ReviewText,
            review.CreateDate,
            review.UpdateDate,
            review.WasEdited);
    }

    public static ReviewShortDto ToShortDto(this Review review)
    {
        return new ReviewShortDto(
            review.BusinessId,
            review.AuthorId,
            review.Grade,
            review.ReviewText);
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