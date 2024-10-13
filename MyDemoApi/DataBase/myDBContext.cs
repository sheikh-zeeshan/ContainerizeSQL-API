


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MyDemoApi.Entities;

namespace MyDemoApi.DataBase
{
    public class myDBContext : DbContext
    {
        public myDBContext() { }
        public myDBContext(DbContextOptions<myDBContext> options) : base(options)
        {

            // var dbCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            // if (dbCreator is null)
            // {
            //     if (!dbCreator.CanConnect())
            //         dbCreator.Create();

            //     if (!dbCreator.HasTables())
            //         dbCreator.CreateTables();
            // }


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //  optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Initial Catalog=DBName;Integrated Security=True");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Products");


            //         var decimalProps = modelBuilder.Model
            //  .GetEntityTypes()
            //  .SelectMany(t => t.GetProperties())
            //  .Where(p => (System.Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

            //         foreach (var property in decimalProps)
            //         {
            //             property.SetPrecision(18);
            //             property.SetScale(2);
            //         }


        }

        public DbSet<Product> Products { get; set; }
    }
}