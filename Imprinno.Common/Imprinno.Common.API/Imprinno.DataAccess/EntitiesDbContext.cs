using Imprinno.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imprinno.DataAccess
{
    public class EntitiesDbContext : DbContext
    {
        public EntitiesDbContext()
        {
        }

        public EntitiesDbContext(DbContextOptions<EntitiesDbContext> options)
            : base(options)
        {
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        // optionsBuilder.UseSqlServer("Server=(local);Database=Seech;Integrated Security=True;Trusted_Connection=True;");
        //        optionsBuilder.UseNpgsql("Host=localhost;port=5432;Database=postgres;Username=postgres;Password=root;Pooling=true;");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties())
                .ToList()
                .ForEach(p => p.SetColumnName(p.GetColumnName().ToLower()));

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
