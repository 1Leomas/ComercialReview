using Intercon.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Intercon.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public Role Role { get; set; }
}
