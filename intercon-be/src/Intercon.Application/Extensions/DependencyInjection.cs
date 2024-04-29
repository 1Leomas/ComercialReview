using FluentValidation;
using Intercon.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Intercon.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(AssemblyReference.Assembly);
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(PerformancePipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);

        return services;
    }
}