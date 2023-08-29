using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodosAPI.Models;

namespace TodosAPI.Data
{
    public class TodosDbContext : IdentityDbContext<ApplicationUser> 
    {
        public DbSet<Todo> Todos { get; set; }
        public TodosDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
