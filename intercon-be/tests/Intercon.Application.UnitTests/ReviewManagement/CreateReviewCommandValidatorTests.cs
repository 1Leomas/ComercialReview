using Intercon.Application.Abstractions;
using Intercon.Application.ReviewsManagement.CreateReview;
using NSubstitute;
using FluentValidation.TestHelper;

namespace Intercon.Application.UnitTests.ReviewManagement;

public class CreateReviewCommandValidatorTests
{
    private readonly CreateReviewCommandValidator _validator;

    private readonly IBusinessRepository _businessRepositoryMock;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IReviewRepository _reviewRepositoryMock;

    private readonly CancellationToken _ctx;

    public CreateReviewCommandValidatorTests()
    {
        _businessRepositoryMock = Substitute.For<IBusinessRepository>();
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _reviewRepositoryMock = Substitute.For<IReviewRepository>();
        _validator = new CreateReviewCommandValidator(
            _businessRepositoryMock,
            _userRepositoryMock,
            _reviewRepositoryMock);

        _ctx = new CancellationToken();
    }

    [Fact]
    public async Task Validator_Should_NotHaveErrors()
    {
        // Arrange

        var command = new CreateReviewCommand
        (
            BusinessId: 1,
            UserId: 1,
            Data: new CreateReviewDto(5, "Review", 1)
        );

        _businessRepositoryMock.BusinessExistsAsync(1, _ctx).Returns(true);
        _userRepositoryMock.UserExistsAsync(1, _ctx).Returns(true);
        _reviewRepositoryMock.ReviewExistsAsync(1, 1, _ctx).Returns(false);

        // Act

        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert

        result.ShouldNotHaveValidationErrorFor(x => x.BusinessId);
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Data.Grade);
        result.ShouldNotHaveValidationErrorFor(x => x.Data.ReviewText);
        result.ShouldNotHaveValidationErrorFor(x => x.Data.RecommendationType);
    }

    [Fact]
    public async Task Validator_Should_HaveError_WhenBusinessIdIsEmpty()
    {
        // Arrange

        var command = new CreateReviewCommand
        (
            BusinessId : 0,
            UserId : 1,
            Data : new CreateReviewDto(5, "Review", 1)
        );

        _businessRepositoryMock.BusinessExistsAsync(1, _ctx).Returns(true);
        _userRepositoryMock.UserExistsAsync(1, _ctx).Returns(true);

        // Act

        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert

        result.ShouldHaveValidationErrorFor(x => x.BusinessId).Only();
    }

    [Fact]
    public async Task Validator_Should_HaveError_WhenBusinessNotExists()
    {
        // Arrange

        var command = new CreateReviewCommand
        (
            BusinessId: 1,
            UserId: 1,
            Data: new CreateReviewDto(5, "Review", 1)
        );

        _businessRepositoryMock.BusinessExistsAsync(1, _ctx).Returns(false);
        _userRepositoryMock.UserExistsAsync(1, _ctx).Returns(true);

        // Act

        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert

        result.ShouldHaveValidationErrorFor(x => x.BusinessId).Only();
    }

    [Fact]
    public async Task Validator_Should_HaveError_WhenUserNotExists()
    {
        // Arrange

        var command = new CreateReviewCommand
        (
            BusinessId: 1,
            UserId: 1,
            Data: new CreateReviewDto(5, "Review", 1)
        );

        _userRepositoryMock.UserExistsAsync(1, _ctx).Returns(false);

        // Act

        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task Validator_Should_HaveError_WhenUserAlreadyWroteForBusiness()
    {
        // Arrange

        var command = new CreateReviewCommand
        (
            BusinessId: 1,
            UserId: 1,
            Data: new CreateReviewDto(5, "Review", 1)
        );

        _businessRepositoryMock.BusinessExistsAsync(1, _ctx).Returns(true);
        _userRepositoryMock.UserExistsAsync(1, _ctx).Returns(true);

        _reviewRepositoryMock.ReviewExistsAsync(1, 1, _ctx).Returns(true);

        // Act

        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert

        result.ShouldHaveValidationErrorFor(x => x.UserId).Only();
    }

    [Fact]
    public async Task Validator_Should_HaveError_WhenBadReviewText()
    {
        // Arrange

        var reviewText = new string('a', 1001);

        var command = new CreateReviewCommand
        (
            BusinessId: 1,
            UserId: 1,
            Data: new CreateReviewDto(5, reviewText, 1)
        );

        // Act

        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Data.ReviewText);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public async Task Validator_Should_HaveError_WhenGradeIsOutOfRange(int grade)
    {
        // Arrange
        var command = new CreateReviewCommand
        (
            BusinessId: 1,
            UserId: 1,
            Data: new CreateReviewDto(grade, "Review", 1)
        );

        // Act
        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Data.Grade);
    }

    [Fact]
    public async Task Validator_Should_NotHaveErrors_WhenGradeIsValid()
    {
        // Arrange
        var command = new CreateReviewCommand
        (
            BusinessId: 1,
            UserId: 1,
            Data: new CreateReviewDto(3, "Review", 1)
        );

        // Act
        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Data.Grade);
    }
}