


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


        }

        public DbSet<Product> Products { get; set; }
    }
}