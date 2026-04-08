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

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.CommonName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.OfficialName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Capital)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Region)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(e => e.Subregion)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(e => e.FlagPngUrl)
                    .IsRequired()
                    .HasMaxLength(600);

                entity.Property(e => e.FlagSvgUrl)
                    .IsRequired()
                    .HasMaxLength(600);

                entity.Property(e => e.Latitude)
                    .IsRequired();

                entity.Property(e => e.Longitude)
                    .IsRequired();

                entity.Property(e => e.IsArchived)
                    .HasDefaultValue(false)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .IsRequired();

                entity.HasIndex(e => e.CountryCode)
                    .IsUnique();
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.ToTable("Favorites");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.CommonName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.FlagPngUrl)
                    .IsRequired()
                    .HasMaxLength(600);

                entity.Property(e => e.AddedAt)
                    .IsRequired();

                entity.HasIndex(e => e.CountryCode)
                    .IsUnique();
            });

            modelBuilder.Entity<UserNote>(entity =>
            {
                entity.ToTable("UserNotes");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.NoteText)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.CreatedAt)
                    .IsRequired();
            });
        }
    }
}