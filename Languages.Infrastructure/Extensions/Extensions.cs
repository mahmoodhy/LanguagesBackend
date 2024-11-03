using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Infrastructure.DataAccess;
using Core.Interfaces.Repository;
using Core.Interfaces;
using Infrastructure.Repository;
using Infrastructure.UnitofWork;


namespace Infrastructure.Extensions
{
    public static class Extensions
    {

        public static IServiceCollection AddDatabaseContext<T>(this IServiceCollection services, IConfiguration config, string DatabaseConnection) where T : DbContext
        {
            if (string.IsNullOrWhiteSpace(DatabaseConnection))
                DatabaseConnection = "Default";
            var connectionString = config.GetConnectionString(DatabaseConnection);
            services.AddMSSQL<T>(connectionString);
            return services;
        }
        private static IServiceCollection AddMSSQL<T>(this IServiceCollection services, string connectionString) where T : DbContext
        {
            services.AddDbContext<T>(m => m.UseSqlServer(connectionString, e => e.MigrationsAssembly(typeof(T).Assembly.FullName)));
            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<T>();
            //dbContext.Database.Migrate();
            return services;
        }
        
    }
}
