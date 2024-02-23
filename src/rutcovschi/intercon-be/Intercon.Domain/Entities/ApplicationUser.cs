using Intercon.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Intercon.Domain.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Role Role { get; set; }
}
