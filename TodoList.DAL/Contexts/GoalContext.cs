using Microsoft.EntityFrameworkCore;
using TodoList.DAL.Entities;

namespace TodoList.DAL.Contexts
{
    public class GoalContext : DbContext 
    {
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Entities.Type> Types { get; set; }
        public DbSet<User> Users { get; set; }

        public GoalContext(DbContextOptions<GoalContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
