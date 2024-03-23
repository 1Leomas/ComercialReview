using MediatR;

namespace Intercon.Application.Abstractions.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}

public interface IQuery : IRequest
{
}