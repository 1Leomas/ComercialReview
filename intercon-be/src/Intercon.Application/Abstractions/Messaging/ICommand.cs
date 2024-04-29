﻿using MediatR;

namespace Intercon.Application.Abstractions.Messaging;

public interface ICommandBase
{
}

public interface ICommand : IRequest, ICommandBase
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>, ICommandBase
{
}