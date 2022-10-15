using DOTNprojekt.Models;
using Microsoft.EntityFrameworkCore;

namespace DOTNprojekt.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Class tasked with handling database operations
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Mod> Mods { get; set; }
    }
}
