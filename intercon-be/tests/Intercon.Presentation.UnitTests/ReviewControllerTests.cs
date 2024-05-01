using Intercon.Application.ReviewsManagement.CreateReview;
using Intercon.Application.ReviewsManagement.GetAllReviews;
using Intercon.Presentation.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Security.Claims;
using System.Security.Principal;
using Intercon.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Intercon.Presentation.UnitTests
{
    public class ReviewControllerTests
    {
        private readonly IMediator _mediatorMock;
        private readonly ReviewController _controller;

        public ReviewControllerTests()
        {
            _mediatorMock = Substitute.For<IMediator>();
            _controller = new ReviewController(_mediatorMock);
            
        }

        [Fact]
        public async Task GetAllReviews_ReturnsOkResult()
        {
            // Arrange
            

            // Act
            await _controller.GetAllReviews(default);

            // Assert
            var result = await _mediatorMock.Received(1).Send(
                Arg.Any<GetAllReviewsQuery>(), 
                Arg.Any<CancellationToken>());

            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateReview_ReturnsOkResult()
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            var reviewToAdd = new CreateReviewDto(6, "text", 2);

            var command = new CreateReviewCommand(1, 1, reviewToAdd);

            _mediatorMock.Send(command, cancellationToken).Returns(Task.CompletedTask);


            var controller = new ReviewController(_mediatorMock)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = CreateTestUser("2")
                    }
                }
            };

            // Act
            var result = await controller.CreateReview(1, reviewToAdd, cancellationToken);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        private static ClaimsPrincipal CreateTestUser(string role)
        {
            var identity = new GenericIdentity("testUser");
            var principal = new ClaimsPrincipal(identity);
            var claimsIdentity = principal.Identity as ClaimsIdentity;
            claimsIdentity.AddClaim(new Claim(JwtClaimType.UserId, "1"));
            claimsIdentity.AddClaim(new Claim(JwtClaimType.Role, role));
            return principal;
        }
    }
}