﻿using Intercon.Domain.Abstractions;

namespace Intercon.Domain.Entities;

public class ResetPasswordCode
{
    public int Id { get; init; }
    public string Code { get; set; } = null!;
    public DateTime ValidUntilDate { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}