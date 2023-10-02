using TaskManagementSystem.Core.Abstractions;

namespace TaskManagementSystem.WebApi
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedDatabase(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            initializer.SeedDb();

            return builder;
        }
    }
}
