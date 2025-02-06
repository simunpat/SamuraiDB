using Microsoft.EntityFrameworkCore;
using EntityFrameworkOpgave.DAL.Models;

namespace EntityFrameworkOpgave.DAL.Data
{
    public class SamuraiDbContext : DbContext
    {
        public SamuraiDbContext(DbContextOptions<SamuraiDbContext> options) : base(options)
        {
        }

        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Horse> Horses { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Battle> Battles { get; set; }
    }
}