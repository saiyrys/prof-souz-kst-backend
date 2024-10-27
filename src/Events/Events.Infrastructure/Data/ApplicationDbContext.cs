using Events.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data
{
    public partial class ApplicationDbContext : DbContext
    { 
        public virtual DbSet<Event> events { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
           base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => new { e.eventId });

                entity.HasIndex(e => new { e.title }).IsUnique();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
