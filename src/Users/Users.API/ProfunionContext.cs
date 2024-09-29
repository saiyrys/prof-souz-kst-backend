/*using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Users.API
{
    public partial class ProfunionContext : DbContext
    {
        public ProfunionContext()
        {
        }

        public ProfunionContext(DbContextOptions<ProfunionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;port=5432;Database=Profunion;Username=postgres;Password=t55061700");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum("user_role", new[] { "USER", "ADMIN", "MODER" });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Username, "users_username_key")
                    .IsUnique();

                entity.Property(e => e.Userid)
                    .HasMaxLength(255)
                    .HasColumnName("userid");

                entity.Property(e => e.Createdat)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdat")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(255)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(255)
                    .HasColumnName("lastname");

                entity.Property(e => e.Middlename)
                    .HasMaxLength(255)
                    .HasColumnName("middlename");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Salt)
                    .HasMaxLength(255)
                    .HasColumnName("salt");

                entity.Property(e => e.Updatedat)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updatedat")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
*/