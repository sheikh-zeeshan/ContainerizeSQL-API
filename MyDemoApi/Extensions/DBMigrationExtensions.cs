using Microsoft.EntityFrameworkCore;
using MyDemoApi.DataBase;


namespace MyDemoApi.Extensions;
public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder builder)
    {

        using (IServiceScope scope = builder.ApplicationServices.CreateScope())
        {

            using (myDBContext dbContext = scope.ServiceProvider.GetRequiredService<myDBContext>())
            {

                if (!dbContext.Database.CanConnect())
                    dbContext.Database.EnsureCreated();

                var pendingMigrations = dbContext.Database.GetPendingMigrations();

                if (pendingMigrations.Any())
                    dbContext.Database.Migrate();
            }
        }
    }
}