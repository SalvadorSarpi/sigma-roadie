using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sigma.Roadie.Domain.DataModels
{
    public partial class RoadieEntities : DbContext
    {
        public RoadieEntities()
        {
        }

        public RoadieEntities(DbContextOptions<RoadieEntities> options)
            : base(options)
        {
        }

        public virtual DbSet<MediaFile> MediaFile { get; set; }
        public virtual DbSet<Scene> Scene { get; set; }
        public virtual DbSet<Setlist> Setlist { get; set; }
        public virtual DbSet<SetlistScene> SetlistScene { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("data source=localhost;database=roadie;integrated security=true;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaFile>(entity =>
            {
                entity.Property(e => e.MediaFileId).ValueGeneratedNever();

                entity.HasOne(d => d.Scene)
                    .WithMany(p => p.MediaFile)
                    .HasForeignKey(d => d.SceneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoredFile_Scene");
            });

            modelBuilder.Entity<Scene>(entity =>
            {
                entity.Property(e => e.SceneId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Setlist>(entity =>
            {
                entity.Property(e => e.SetlistId).ValueGeneratedNever();
            });

            modelBuilder.Entity<SetlistScene>(entity =>
            {
                entity.HasKey(e => new { e.SetlistId, e.SceneId });

                entity.HasOne(d => d.Scene)
                    .WithMany(p => p.SetlistScene)
                    .HasForeignKey(d => d.SceneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SetlistScene_Scene");

                entity.HasOne(d => d.Setlist)
                    .WithMany(p => p.SetlistScene)
                    .HasForeignKey(d => d.SetlistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SetlistScene_Setlist");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
