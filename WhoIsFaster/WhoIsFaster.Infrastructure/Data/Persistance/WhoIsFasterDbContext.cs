using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WhoIsFaster.Domain.Entities;
using WhoIsFaster.Domain.Entities.RoomAggregate;

namespace Data.Persistance
{
    public class WhoIsFasterDbContext : IdentityDbContext<IdentityUser>
    {
        public WhoIsFasterDbContext(DbContextOptions<WhoIsFasterDbContext> options) : base(options) { }

        public DbSet<RegularUser> RegularUsers { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Room>().HasMany(r => r.RoomPlayers);
            modelBuilder.Entity<Room>().Ignore(r => r.WordList);
        }
    }


}