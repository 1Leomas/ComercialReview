using FluentValidation.TestHelper;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.BusinessesManagement.CreateBusiness;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.ComplexTypes;
using Intercon.Domain.Enums;
using NSubstitute;

namespace Intercon.Application.UnitTests.BusinessManagement;

public class CreateBusinessCommandValidatorTests
{
    private readonly CreateBusinessCommandValidator _validator;

    private readonly IBusinessRepository _businessRepositoryMock;
    private readonly IUserRepository _userRepositoryMock;

    private readonly CancellationToken _ctx;

    public CreateBusinessCommandValidatorTests()
    {
        _businessRepositoryMock = Substitute.For<IBusinessRepository>();
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _validator = new CreateBusinessCommandValidator(
            businessRepository: _businessRepositoryMock,
            userRepository: _userRepositoryMock);

        _ctx = new CancellationToken();
    }

    [Fact]
    public async Task Validator_Should_HaveError_WhenUserIdIsEmpty()
    {
        // Arrange
        var command = new CreateBusinessCommand(
            UserId: 0,
            Data: new CreateBusinessDto(
                "Title",
                "ShortDescription",
                null,
                null,
                null,
                new Address(),
                BusinessCategory.Bakery
            )
        );

        // Act
        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task Validator_Should_HaveError_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepositoryMock.UserExistsAsync(Arg.Any<int>(), _ctx).Returns(false);
        var command = new CreateBusinessCommand(
            UserId: 1,
            Data: new CreateBusinessDto(
                "Title",
                "ShortDescription",
                null,
                null,
                null,
                new Address(),
                BusinessCategory.Bakery
            )
        );

        // Act
        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task Validator_Should_HaveError_WhenUserHasExistingBusiness()
    {
        // Arrange
        _businessRepositoryMock.UserHasBusinessAsync(Arg.Any<int>(), _ctx).Returns(true);
        var command = new CreateBusinessCommand(
            UserId: 1,
            Data: new CreateBusinessDto(
                "Title",
                "ShortDescription",
                null,
                null,
                null,
                new Address(),
                BusinessCategory.Bakery
            )
        );

        // Act
        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task Validator_Should_HaveError_WhenTitleIsEmpty()
    {
        // Arrange
        var command = new CreateBusinessCommand(
            UserId: 1,
            Data: new CreateBusinessDto(
                "",
                "ShortDescription",
                null,
                null,
                null,
                new Address(),
                BusinessCategory.Bakery
            )
        );

        // Act
        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Data.Title);
    }

    [Fact]
    public async Task Validator_Should_HaveError_WhenShortDescriptionIsEmpty()
    {
        // Arrange
        var command = new CreateBusinessCommand(
            UserId: 1,
            Data: new CreateBusinessDto(
                "Title",
                "",
                null,
                null,
                null,
                new Address(),
                BusinessCategory.Bakery
            )
        );

        // Act
        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Data.ShortDescription);
    }

    [Fact]
    public async Task Validator_Should_NotHaveErrors()
    {
        // Arrange
        _businessRepositoryMock.UserHasBusinessAsync(Arg.Any<int>(), _ctx).Returns(false);
        _userRepositoryMock.UserExistsAsync(Arg.Any<int>(), _ctx).Returns(true);
        var command = new CreateBusinessCommand(
            UserId: 1,
            Data: new CreateBusinessDto(
                "Title",
                "ShortDescription",
                null,
                null,
                null,
                new Address(),
                BusinessCategory.Bakery
            )
        );

        // Act
        var result = await _validator.TestValidateAsync(command, null, _ctx);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Data.Title);
        result.ShouldNotHaveValidationErrorFor(x => x.Data.ShortDescription);
        result.ShouldNotHaveValidationErrorFor(x => x.Data.Address);

        Assert.True(result.IsValid);
    }
}
