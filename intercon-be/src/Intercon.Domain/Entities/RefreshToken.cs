using Intercon.Domain.Abstractions;

namespace Intercon.Domain.Entities;

public class RefreshToken : Entity, IEntity
{
    public int Id { get; init; }
    public string Token { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}