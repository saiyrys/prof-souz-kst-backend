using Events.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data
{
    public partial class ApplicationDbContext : DbContext
    { 
        public virtual DbSet<Event> Event { get; set; }

        public virtual DbSet<OutBox> OutBox { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
           base(options)
        {
            Database.EnsureCreated();
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => new { e.EventId });

                entity.HasIndex(e => new { e.Title }).IsUnique();
            });

            modelBuilder.Entity<OutBox>(entity =>
            {
                entity.HasKey(o => new { o.EventType });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
