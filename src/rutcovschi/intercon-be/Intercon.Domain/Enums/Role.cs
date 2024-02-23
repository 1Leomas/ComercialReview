using System.Data;

namespace Intercon.Domain.Enums;

public enum Role
{
    User,
    Admin,
    SuperAdmin
}

public static class RolesExtensions
{
    public static string ToStringValue(this Role role)
    {
        return role.ToString();
    }
}