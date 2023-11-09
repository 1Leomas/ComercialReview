using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Intercon.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddDbContext<DbContext>(options =>
        //{ 
        //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        //});

        //add scoped life time for all classes and interfaces from Infrastructure module
        services.Scan(
            selector => selector
                .FromAssemblies(AssemblyReference.Assembly)
                .AddClasses(false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        return services;
    }
}