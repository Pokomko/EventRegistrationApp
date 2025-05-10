using Domain.Entities;
using Domain.Enum;
using Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;

namespace Infrastructure.Context;

public class AppDbContext : DbContext
{
    private readonly IOptions<AuthorizationOptions> _authOptions;

    public AppDbContext(DbContextOptions<AppDbContext> options,
        IOptions<AuthorizationOptions> authOptions) : base(options) {
        _authOptions = authOptions;
    }

    public DbSet<Event> Events => Set<Event>();
    public DbSet<Participant> Participants => Set<Participant>();
    public DbSet<ParticipantEvent> ParticipantEvents => Set<ParticipantEvent>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Role => Set<Role>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(_authOptions.Value));

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id); // Устанавливаем ключ для User

            builder.Property(u => u.Username)
                   .IsRequired()  // Указываем, что Username обязательно
                   .HasMaxLength(100);  // Устанавливаем максимальную длину для Username

            builder.Property(u => u.PasswordHash)
                   .IsRequired();  // Указываем, что PasswordHash обязательно

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<UserRole>(
                    l => l.HasOne<Role>().WithMany().HasForeignKey(r => r.RoleId),
                    r => r.HasOne<User>().WithMany().HasForeignKey(u => u.UserId)
                );
        });

        modelBuilder.Entity<Role>(builder =>
        {
            builder.HasKey(r => r.Id);

            builder.HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<RolePermission>(
                    l => l.HasOne<Permission>().WithMany().HasForeignKey(e => e.PermissionId),
                    r => r.HasOne<Role>().WithMany().HasForeignKey(e => e.RoleId));

            var roles = Enum
                .GetValues<RolesEnum>()
                .Select(r => new Role
                {
                    Id = (int)r,
                    Name = r.ToString(),
                });

            builder.HasData(roles);
        });

        modelBuilder.Entity<Permission>(builder => {
            builder.HasKey(p => p.Id);

            var permissions = Enum
                .GetValues<PermissionsEnum>()
                .Select(p => new Permission
                {
                    Id = (int)p,
                    Name = p.ToString(),
                });

            builder.HasData(permissions);
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
