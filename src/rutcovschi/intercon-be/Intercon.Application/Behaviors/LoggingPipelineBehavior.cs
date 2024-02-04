﻿using Intercon.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Intercon.Application.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse?> 
    where TRequest : notnull
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse?> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse?> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting request {@RequestName}, {@DataTimeUtc}",
            typeof(TRequest).Name, DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss.fff"));

        TResponse? result;
        try
        {
            result = await next();

            _logger.LogInformation("Finished request {@RequestName}, {@DataTimeUtc}",
                typeof(TRequest).Name, DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss.fff"));
        }
        finally { }

        return result;
    }
}
