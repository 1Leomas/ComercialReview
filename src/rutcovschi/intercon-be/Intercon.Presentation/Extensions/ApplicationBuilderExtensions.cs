using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Presentation.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void EnsureDatabaseCreated(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        var context = serviceScope.ServiceProvider.GetRequiredService<InterconDbContext>();
        context.Database.Migrate();
    }
}