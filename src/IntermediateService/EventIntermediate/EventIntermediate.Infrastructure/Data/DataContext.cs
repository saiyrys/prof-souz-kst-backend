using Category.Infrastructure.Models;
using EventIntermediate.Infrastructure.Models;
using Events.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventIntermediate.Infrastructure.Data
{
    public partial class DataContext : DbContext
    {
        public virtual DbSet<Event> Events { get; set; }

        public virtual DbSet<Categories> Categories { get; set; }

        public virtual DbSet<EventCategories> EventCategories { get; set; }

        public DataContext(DbContextOptions<DataContext> options) :
           base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>()
                .HasKey(c => c.categoryId);

            modelBuilder.Entity<Event>()
                 .HasKey(e => e.eventId);

            modelBuilder.Entity<EventCategories>()
                .HasKey(ec => new { ec.eventId, ec.categoriesId });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
