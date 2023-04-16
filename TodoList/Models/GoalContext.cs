using Microsoft.EntityFrameworkCore;

namespace TodoList.Models
{
    public class GoalContext : DbContext 
    {
        public DbSet<Goal> Goals { get; set; } = null!;
        public DbSet<Type> Types { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public GoalContext(DbContextOptions<GoalContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
