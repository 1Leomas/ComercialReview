namespace Intercon.Application.DataTransferObjects.User;

public class ResetPasswordRequest
{
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string ResetPasswordCode { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}