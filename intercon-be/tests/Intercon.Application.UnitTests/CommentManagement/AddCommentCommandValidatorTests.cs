using FluentValidation.TestHelper;
using Intercon.Application.Abstractions;
using Intercon.Application.CommentsManagement.AddComment;
using Intercon.Application.DataTransferObjects.Comment;
using NSubstitute;

namespace Intercon.Application.UnitTests.CommentManagement;

public class AddCommentCommandValidatorTests
{
    private readonly AddCommentCommandValidator _validator;

    private readonly IReviewRepository _reviewRepositoryMock;

    private readonly CancellationToken _ctx;

    public AddCommentCommandValidatorTests()
    {
        _reviewRepositoryMock = Substitute.For<IReviewRepository>();
        _validator = new AddCommentCommandValidator(
            reviewRepository: _reviewRepositoryMock);

        _ctx = new CancellationToken();
    }

    [Fact]
    public async Task Validator_Should_HaveError_WhenTextIsEmpty()
    {
        // Arrange
        var command = new AddCommentCommand
        (
            CommentDto: new AddCommentDto { Text = string.Empty, BusinessId = 1, ReviewAuthorId = 1 }
        );

        // Act
        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CommentDto.Text);
    }

    [Fact]
    public async Task Validator_Should_HaveError_WhenTextIsTooLong()
    {
        // Arrange
        var commentText = new string('a', 501);
        var command = new AddCommentCommand
        (
            CommentDto: new AddCommentDto { Text = commentText, BusinessId = 1, ReviewAuthorId = 1 }
        );

        // Act
        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CommentDto.Text);
    }

    [Fact]
    public async Task Validator_Should_NotHaveErrors()
    {
        // Arrange
        _reviewRepositoryMock.ReviewExistsAsync(1, 1, _ctx).Returns(true);
        var command = new AddCommentCommand
        (
            CommentDto: new AddCommentDto { Text = "Comment", BusinessId = 1, ReviewAuthorId = 1 }
        );

        // Act
        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.CommentDto.Text);
        result.ShouldNotHaveValidationErrorFor(x => x.CommentDto.BusinessId);
        result.ShouldNotHaveValidationErrorFor(x => x.CommentDto.ReviewAuthorId);

        Assert.True(result.IsValid);
    }
}