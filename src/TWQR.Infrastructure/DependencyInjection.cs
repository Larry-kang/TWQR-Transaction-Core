using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TWQR.Domain.Interfaces;
using TWQR.Infrastructure.Persistence;
using TWQR.Infrastructure.Repositories;

namespace TWQR.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection") ?? "Data Source=twqr.db"));

            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
