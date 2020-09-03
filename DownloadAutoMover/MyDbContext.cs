using Microsoft.EntityFrameworkCore;
using DownloadAutoMover;
using DownloadAutoMover.Classes;
using System.Reflection;

namespace DownloadAutoMover
{
    public class MyDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<IgnoreItem> IgnoreItems { get; set; }
        public DbSet<MediaType> MediaTypes { get; set; }
        public DbSet<RenameItem> RenameItems { get; set; }
        public DbSet<RedirectItem> RedirectItems { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<SubFolder> SubFolders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=DownloadAutoMover.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CatId);
            });

            modelBuilder.Entity<IgnoreItem>().ToTable("IgnoreItems");
            modelBuilder.Entity<IgnoreItem>(entity =>
            {
                entity.HasKey(e => e.IgnrId);
            });

            modelBuilder.Entity<MediaType>().ToTable("MediaTypes");
            modelBuilder.Entity<MediaType>(entity =>
            {
                entity.HasKey(e => e.MedId);
            });

            modelBuilder.Entity<RedirectItem>().ToTable("RedirectItems");
            modelBuilder.Entity<RedirectItem>(entity =>
            {
                entity.HasKey(e => e.RedId);
            });

            modelBuilder.Entity<RenameItem>().ToTable("RenameItems");
            modelBuilder.Entity<RenameItem>(entity =>
            {
                entity.HasKey(e => e.RenId);
            });

            modelBuilder.Entity<Setting>().ToTable("Settings");
            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasKey(e => e.SetId);
            });

            modelBuilder.Entity<SubFolder>().ToTable("SubFolders");
            modelBuilder.Entity<SubFolder>(entity =>
            {
                entity.HasKey(e => e.SubId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
