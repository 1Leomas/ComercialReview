using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Intercon.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(
                AssemblyReference.Assembly));

        services.AddAutoMapper(AssemblyReference.Assembly);

        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);

        return services;
    }
}