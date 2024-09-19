using Microsoft.EntityFrameworkCore;
using SupPortal.UserService.API.Models.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SupPortal.UserService.API.Data.Context;
 
public class userDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

         modelBuilder.Entity<User>()
            .HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

         modelBuilder.Entity<Role>().HasData(
            new Role { Id = (int)RoleName.Admin, Name = RoleName.Admin, Description = "Administrator role" },
            new Role { Id = (int)RoleName.Supporter, Name = RoleName.Supporter , Description = "Supporter role" },
            new Role { Id = (int)RoleName.User, Name = RoleName.User , Description = "Standart user role" }
        );
    }

     public userDbContext(DbContextOptions<userDbContext> options) : base(options)
    {
    }
}
