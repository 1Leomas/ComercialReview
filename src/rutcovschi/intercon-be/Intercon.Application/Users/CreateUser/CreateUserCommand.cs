using FluentValidation;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.Users.CreateUser;    

public sealed record CreateUserCommand : ICommand<int>
{
    public CreateUserCommand(CreateUserDto userDto) 
    {
        FirstName = userDto.FirstName;
        LastName = userDto.LastName;
        Password = userDto.Password;
        Email = userDto.Email;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string Password { get; set; }
    public string? UserName { get; init; } = null;
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
        var userDb = new User(command.FirstName, command.LastName, command.Password, command.Email, command.UserName);

        await _context.Users.AddAsync(userDb, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return userDb.Id;
    }
}

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(InterconDbContext context)
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);

        RuleFor(x => x.Email).MustAsync(async (email, _) =>
        {
            return await context.Users.AllAsync(x => x.Email != email);
        }).WithMessage("The email must be unique");
    }
}
