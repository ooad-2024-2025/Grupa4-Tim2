using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SarajevoGuide.Models;

namespace SarajevoGuide.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Recenzija> Recenzija { get; set; } = null!;
        public DbSet<RegistrovaniKorisnik> RegistrovaniKorisnik { get; set; } = null!;
        public DbSet<Kupovina> Kupovina { get; set; } = null!;
        public DbSet<Event> Event { get; set; } = null!;
        public DbSet<Bookmark> Bookmark { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recenzija>().ToTable("Recenzija");
            modelBuilder.Entity<RegistrovaniKorisnik>().ToTable("RegistrovaniKorisnik");
            modelBuilder.Entity<Kupovina>().ToTable("Kupovina");
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<Bookmark>().ToTable("Bookmark");

            base.OnModelCreating(modelBuilder);
        }
    }
}
