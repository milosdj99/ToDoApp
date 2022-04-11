using Microsoft.EntityFrameworkCore;
using ToDoCore;
using ToDoInfrastructure.Configurations;

namespace ToDoInfrastructure
{
    public class ToDoDbContext : DbContext 
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) { }

        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<ToDoItem> ToDoItems { get; set; }

        public DbSet<ToDoShareList> ToDoSharedLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ToDoItemConfiguration());
            modelBuilder.ApplyConfiguration(new ToDoListConfiguration());
            modelBuilder.ApplyConfiguration(new ToDoShareListConfiguration());
        }
    }

}