using Microsoft.EntityFrameworkCore;
 
namespace SerajAssociates.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<Project> Project {get;set;}
        public DbSet<Message> Messages {get;set;}
        public DbSet<Admin> Admin {get;set;}
    }
}