using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupPortal.TicketService.API.Domain.Entities;
using System.Reflection.Emit;

namespace SupPortal.TicketService.API.Infrastructure.Data;
public class tsContext : DbContext
{
    public tsContext(DbContextOptions<tsContext> options) : base(options)
    {
    }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<TicketOutbox> TicketOutboxes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureTicket(modelBuilder.Entity<Ticket>());
        ConfigureTag(modelBuilder.Entity<Tag>());
        ConfigureComment(modelBuilder.Entity<Comment>());
        ConfigureTicketOutbox(modelBuilder.Entity<TicketOutbox>());
    }

    private void ConfigureTicket(EntityTypeBuilder<Ticket> builder)
    {

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();


        builder
           .Property(t => t.Name)
           .HasMaxLength(100)
           .IsRequired();

        builder
          .Property(t => t.Description)
          .IsRequired(false);

        builder
                .Property(t => t.Status)
                .HasConversion<int>()
                .IsRequired();



        builder
                .Property(t => t.Priority)
                .HasConversion<int>()
                .IsRequired();

        builder
       .Property(t => t.CreatedOn)
       .HasDefaultValueSql("GETDATE()");

        builder
        .HasMany(t => t.Comments)
        .WithOne(o=>o.Ticket)
        .HasForeignKey(c => c.TicketId);
       // .OnDelete(DeleteBehavior.Cascade);

        builder
         .Property(t => t.UpdateOn)
         .IsRequired(false);

        builder
      .HasMany(t => t.TicketTags)
      .WithMany(tag => tag.Tickets);

    }

    private void ConfigureTag(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        builder
              .Property(t => t.Name)
              .HasMaxLength(50)
              .IsRequired();

        builder
            .HasMany(tag => tag.Tickets)
            .WithMany(ticket => ticket.TicketTags);

        builder
         .Property(t => t.UpdateOn)
         .IsRequired(false);

        builder
       .Property(t => t.CreatedOn)
       .HasDefaultValueSql("GETDATE()");
    }


    private void ConfigureComment(EntityTypeBuilder<Comment> builder)
    {

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        builder
          .Property(t => t.Content)
          .HasMaxLength(200)
          .IsRequired();

        builder
          .Property(t => t.UpdateOn)
          .IsRequired(false);

        builder
       .Property(t => t.CreatedOn)
       .HasDefaultValueSql("GETDATE()");
    }



    private void ConfigureTicketOutbox(EntityTypeBuilder<TicketOutbox> builder)
    {
        builder.HasKey(u => u.Id);


        builder
        .Property(t => t.EventType)    
        .IsRequired();

        builder
      .Property(t => t.EventPayload)
      .IsRequired();

        builder
      .Property(t => t.OccuredOn)
      .IsRequired();

        builder
          .Property(t => t.EventStatus)
          .HasConversion<int>()
          .IsRequired();



    }
}
