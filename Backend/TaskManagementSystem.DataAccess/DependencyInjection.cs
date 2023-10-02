using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Core;
using Microsoft.Extensions.Configuration;
using TaskManagementSystem.Core.Abstractions;

namespace TaskManagementSystem.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var postgresConnection = configuration.GetConnectionString("PostgresConnection");
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (postgresConnection != null)
            {
                services.AddDbContext<DataContext>(x =>
                {
                    x.UseNpgsql(postgresConnection);
                });
            }
            else if (connectionString != null)
                services.AddDbContext<DataContext>(x => 
                { 
                    x.UseSqlite(connectionString, x =>
                        x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)); 
                });
            else
                services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("TemporaryInMemoryDatabase"));
            services.AddScoped<IIssuesService, IssuesService>();
            return services;
        }
    }
}
