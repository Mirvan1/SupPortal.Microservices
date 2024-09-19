using Microsoft.EntityFrameworkCore;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.ApplicationCore.Services;
using SupPortal.TicketService.API.Infrastructure.Data;
using SupPortal.TicketService.API.Infrastructure.Repository;

namespace SupPortal.TicketService.API.Infrastructure.Extension;

public static class DIRegistration
{
    public static IServiceCollection AddDIRegistration(this IServiceCollection services)
    {
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddHttpContextAccessor();

        services.AddScoped<ITicketOutboxRepository, TicketOutboxRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthSettings, AuthSettings>();
        services.AddScoped<IPublishJobService, PublishJobService>();

        return services;
    }

    public static IServiceCollection AddDBRegistration(this IServiceCollection services, IConfiguration configuration)
    {
       
        return services;
    }
}
