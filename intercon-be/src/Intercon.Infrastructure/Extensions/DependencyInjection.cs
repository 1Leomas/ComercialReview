using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.Abstractions.Services;
using Intercon.Domain.Notifications;
using Intercon.Infrastructure.Options;
using Intercon.Infrastructure.Persistence;
using Intercon.Infrastructure.Persistence.DataSeeder;
using Intercon.Infrastructure.Repositories;
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

        services.AddDbContext<InterconDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddRepositories();
        services.AddServices();

        services.AddScoped<DataBaseSeeder>();

        services.AddSignalR();

        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBusinessRepository, BusinessRepository>();
        services.AddScoped<ICommentLikeRepository, CommentLikeRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IPerformanceLogRepository, PerformanceLogRepository>();
        services.AddScoped<IReviewLikeRepository, ReviewLikeRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IBlobStorage, AzureBlobStorageService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IImageValidator, BytesImageValidator>();
        services.AddSingleton<IItemQueueService<Notification>, NotificationsQueueService>();
        services.AddScoped<ITokenService, JwtTokenService>();
    }
}