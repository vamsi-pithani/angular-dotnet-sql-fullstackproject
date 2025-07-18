using Microsoft.EntityFrameworkCore;
using DotnetApi.Models;

namespace DotnetApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<User> Users { get; set; }
        // Add other DbSets as needed
    }
}
