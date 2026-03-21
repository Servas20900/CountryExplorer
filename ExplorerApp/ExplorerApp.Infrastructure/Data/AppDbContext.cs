using ExplorerApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ExplorerApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Country> Countries => Set<Country>();
        public DbSet<Favorite> Favorites => Set<Favorite>();
        public DbSet<UserNote> UserNotes => Set<UserNote>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Countries");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Capital)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Region)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.FlagUrl)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Population)
                    .IsRequired();
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.ToTable("Favorites");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.AddedDate)
                    .IsRequired();

                entity.HasOne(e => e.Country)
                    .WithMany(c => c.Favorites)
                    .HasForeignKey(e => e.CountryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserNote>(entity =>
            {
                entity.ToTable("UserNotes");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.NoteText)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.CreatedAt)
                    .IsRequired();

                entity.HasOne(e => e.Favorite)
                    .WithMany(f => f.UserNotes)
                    .HasForeignKey(e => e.FavoriteId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}