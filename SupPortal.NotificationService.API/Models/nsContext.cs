using Microsoft.EntityFrameworkCore;
using SupPortal.NotificationService.API.Models.Entities;

namespace SupPortal.NotificationService.API.Models;

public class nsContext : DbContext
{
    public nsContext(DbContextOptions<nsContext> options) : base(options)
    {
    }

    public DbSet<Mail> Mails { get; set; }
    public DbSet<MailInbox> MailInboxs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Mail>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.SenderAddress)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(e => e.Username)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Subject)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(e => e.Body)
                  .IsRequired();
        });



        modelBuilder.Entity<MailInbox>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.EventPayload)
                  .IsRequired();

            entity.Property(e => e.Processed)
                  .IsRequired();
            entity.Property(e => e.EventError)
               .IsRequired(false);
        });


    }
}
