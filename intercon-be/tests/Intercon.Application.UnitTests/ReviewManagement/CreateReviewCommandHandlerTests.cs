using Intercon.Application.Abstractions;
using Intercon.Application.ReviewsManagement.CreateReview;
using NSubstitute;

namespace Intercon.Application.UnitTests.ReviewManagement;

public class CreateReviewCommandHandlerTests
{
    private static readonly CreateReviewCommand Command = 
        new(1, 1, new CreateReviewDto(5, "Great!", 1));

    private readonly CreateReviewCommandHandler _handler;
    private readonly IReviewRepository _reviewRepositoryMock;

    public CreateReviewCommandHandlerTests()
    {
        _reviewRepositoryMock = Substitute.For<IReviewRepository>();
        _handler = new CreateReviewCommandHandler(_reviewRepositoryMock);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CreatesReview()
    {
        // Arrange
        _reviewRepositoryMock.CreateReviewAsync(
            Command.BusinessId,
            Command.UserId,
            Command.Data,
            Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        await _handler.Handle(Command, CancellationToken.None);

        // Assert
        await _reviewRepositoryMock.Received(1).CreateReviewAsync(
            Command.BusinessId,
            Command.UserId,
            Command.Data,
            Arg.Any<CancellationToken>());
    }
}