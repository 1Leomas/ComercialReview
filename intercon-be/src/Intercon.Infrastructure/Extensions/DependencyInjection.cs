using Intercon.Application.Abstractions;
using Intercon.Infrastructure.Options;
using Intercon.Infrastructure.Persistence;
using Intercon.Infrastructure.Persistence.DataSeeder;
using Intercon.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Intercon.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AzureBlobStorageSettings>(configuration.GetSection(nameof(AzureBlobStorageSettings)));

#if !DEBUG
        var connectionString = string.Empty;

        using (var sr = new StreamReader("../../.dbConnectionString"))
        {
            try
            {
                connectionString = sr.ReadToEnd();
            }
            catch (Exception)
            {
                throw new ArgumentException("Missing '.dbConnectionString' file in root folder");
            }
        }
#endif
        services.AddDbContext<InterconDbContext>(options =>
        {
#if DEBUG
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
#else
            options.UseSqlServer(connectionString);
#endif
        });

        //services.AddScoped<ITokenService, JwtTokenService>();

        //add scoped life time for all classes and interfaces from Infrastructure module
        services.Scan(
            selector => selector
                .FromAssemblies(AssemblyReference.Assembly)
                .AddClasses(false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        services.AddScoped<DataBaseSeeder>();

        return services;
    }
}