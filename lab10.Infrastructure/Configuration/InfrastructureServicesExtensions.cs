using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using lab10.Domain.Interfaces;
using lab10.Infrastructure.Data;
using lab10.Infrastructure.Repositories;

namespace lab10.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database Connection - MySQL
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<TicketeraDbContext>(options =>
        {
            options.UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString));
        });

        // Register Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IResponseRepository, ResponseRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        // Unit of Work (Optional)
        // services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
