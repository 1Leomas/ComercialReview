using FluentValidation;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.Extensions;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.Users.CreateUser;    

public sealed record CreateUserCommand : ICommand<int>
{
    public CreateUserCommand(CreateUserDto userDto) 
    {
        UserDto = userDto;
    }

    public CreateUserDto UserDto { get; set; }
}


public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, int>
{
    private readonly InterconDbContext _context;

    public CreateUserCommandHandler(InterconDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userDb = command.UserDto.ToEntity();

        await _context.Users.AddAsync(userDb, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return userDb.Id;
    }
}

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(InterconDbContext context)
    {
        RuleFor(x => x.UserDto.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.UserDto.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.UserDto.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.UserDto.Password).NotEmpty().MinimumLength(8);

        RuleFor(x => x.UserDto.Email).MustAsync(async (email, _) =>
        {
            return await context.Users.AllAsync(x => x.Email != email);
        }).WithMessage("The email must be unique");
    }
}
