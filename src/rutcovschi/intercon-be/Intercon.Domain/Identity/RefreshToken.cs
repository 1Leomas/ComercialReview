using Intercon.Domain.Abstractions;
using Intercon.Domain.Entities;

namespace Intercon.Domain.Identity;

public class RefreshToken : Entity
{
    public string Token { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    public int UserId { get; set; }

    public User User { get; set; } = null!;
}