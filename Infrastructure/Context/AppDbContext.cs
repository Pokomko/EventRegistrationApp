using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Event> Events => Set<Event>();
    public DbSet<Participant> Participants => Set<Participant>();
    public DbSet<ParticipantEvent> ParticipantEvents => Set<ParticipantEvent>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id); // Устанавливаем ключ для User
            builder.Property(u => u.Username)
                   .IsRequired()  // Указываем, что Username обязательно
                   .HasMaxLength(100);  // Устанавливаем максимальную длину для Username

            builder.Property(u => u.PasswordHash)
                   .IsRequired();  // Указываем, что PasswordHash обязательно

            builder.Property(u => u.Role)
                   .HasDefaultValue("User");  // Устанавливаем значение по умолчанию для Role
        });

        modelBuilder.Entity<ParticipantEvent>()
            .HasKey(pe => new { pe.ParticipantId, pe.EventId });

        modelBuilder.Entity<ParticipantEvent>()
            .HasOne(pe => pe.Participant)
            .WithMany(p => p.ParticipantEvents)
            .HasForeignKey(pe => pe.ParticipantId);

        modelBuilder.Entity<ParticipantEvent>()
            .HasOne(pe => pe.Event)
            .WithMany(e => e.ParticipantEvents)
            .HasForeignKey(pe => pe.EventId);
    }
}
