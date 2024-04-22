﻿namespace Intercon.Application.Exceptions;

public abstract class BaseException : Exception
{
    protected BaseException(string? message) : base(string.IsNullOrEmpty(message) ? "Internal exception" : message) { }
}