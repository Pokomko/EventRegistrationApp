using Domain.Entities;
using Domain.Enum;
using Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
    public DbSet<Role> Roles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(_authOptions.Value));

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                   .IsRequired();

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<UserRole>(
                    l => l.HasOne<Role>().WithMany().HasForeignKey(r => r.RoleId),
                    r => r.HasOne<User>().WithMany().HasForeignKey(u => u.UserId)
                );

            builder.HasOne(u => u.Participant)
                .WithOne(p => p.User)
                .HasForeignKey<Participant>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
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

        modelBuilder.Entity<Event>(builder =>
        {
            builder.Property(e => e.StartDateTime)
                .HasColumnType("datetime2(0)");

            builder.HasData(
                new Event
                {
                    Id = Guid.Parse("2c8e0f9b-18c4-45d7-b48b-09b55f585935"),
                    Title = "AI & Machine Learning Summit 2025",
                    Description = "Comprehensive conference covering the latest advances in artificial intelligence, machine learning algorithms, and their practical applications in various industries.",
                    StartDateTime = new DateTime(2025, 6, 15, 9, 0, 0),
                    Location = "Moscow, Digital October Center",
                    Category = "Technology",
                    MaxParticipants = 300,
                    ImageUrl = "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg"
                },

                new Event
                {
                    Id = Guid.Parse("2c8e0f9b-18c4-45d7-b48b-09b55f585936"),
                    Title = "Cloud Computing Workshop",
                    Description = "Hands-on workshop on cloud technologies, microservices architecture, and DevOps practices for modern software development.",
                    StartDateTime = new DateTime(2025, 4, 22, 14, 30, 0),
                    Location = "St. Petersburg, ITMO University",
                    Category = "Technology",
                    MaxParticipants = 150,
                    ImageUrl = "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg"
                },

                new Event
                {
                    Id = Guid.Parse("2c8e0f9b-18c4-45d7-b48b-09b55f585937"),
                    Title = "Cybersecurity Conference",
                    Description = "Expert discussions on cybersecurity threats, data protection strategies, and emerging security technologies for enterprises.",
                    StartDateTime = new DateTime(2025, 8, 10, 10, 0, 0),
                    Location = "Novosibirsk, Technopark Akademgorodok",
                    Category = "Technology",
                    MaxParticipants = 250,
                    ImageUrl = "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg"
                },

                new Event
                {
                    Id = Guid.Parse("e3b42b54-ec1f-4b62-ae9a-41de3aef5a58"),
                    Title = "Startup Pitch Competition",
                    Description = "Competition for innovative startups to present their ideas to investors and industry experts. Prizes include funding opportunities and mentorship.",
                    StartDateTime = new DateTime(2025, 5, 18, 16, 0, 0),
                    Location = "Kazan, IT Park",
                    Category = "Business",
                    MaxParticipants = 100,
                    ImageUrl = "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg"
                },

                new Event
                {
                    Id = Guid.Parse("e3b42b54-ec1f-4b62-ae9a-41de3aef5a59"),
                    Title = "Digital Marketing Masterclass",
                    Description = "Intensive course on modern digital marketing strategies, social media advertising, and content creation for business growth.",
                    StartDateTime = new DateTime(2025, 7, 25, 11, 0, 0),
                    Location = "Yekaterinburg, Ural Federal University",
                    Category = "Marketing",
                    MaxParticipants = 80,
                    ImageUrl = "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg"
                },

                new Event
                {
                    Id = Guid.Parse("f4c53c65-fd2e-5c73-bf0b-52ef4bf6b6a0"),
                    Title = "Blockchain & Cryptocurrency Forum",
                    Description = "Exploring the future of blockchain technology, DeFi protocols, and the impact of cryptocurrencies on traditional finance.",
                    StartDateTime = new DateTime(2025, 9, 12, 13, 0, 0),
                    Location = "Nizhny Novgorod, Lobachevsky University",
                    Category = "Finance",
                    MaxParticipants = 200,
                    ImageUrl = "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg"
                },

                new Event
                {
                    Id = Guid.Parse("a5d64d76-ae3f-6d84-ca1c-63fa5ca7c7b1"),
                    Title = "Mobile App Development Bootcamp",
                    Description = "Comprehensive bootcamp covering iOS and Android development, cross-platform frameworks, and app store optimization.",
                    StartDateTime = new DateTime(2025, 10, 8, 9, 30, 0),
                    Location = "Rostov-on-Don, Southern Federal University",
                    Category = "Technology",
                    MaxParticipants = 120,
                    ImageUrl = "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg"
                },

                new Event
                {
                    Id = Guid.Parse("b6e75e87-af4a-7e95-da2d-74aa6da8d8c2"),
                    Title = "Data Science & Analytics Summit",
                    Description = "Advanced techniques in data analysis, machine learning models, and big data processing for business intelligence.",
                    StartDateTime = new DateTime(2025, 11, 20, 10, 0, 0),
                    Location = "Vladivostok, Far Eastern Federal University",
                    Category = "Technology",
                    MaxParticipants = 180,
                    ImageUrl = "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg"
                },

                new Event
                {
                    Id = Guid.Parse("c7f86f98-aa5a-8f06-ea3e-85aa7ea9e9d3"),
                    Title = "Game Development Conference",
                    Description = "Comprehensive conference on game design, programming, graphics, and the latest trends in the gaming industry.",
                    StartDateTime = new DateTime(2025, 12, 5, 12, 0, 0),
                    Location = "Krasnodar, Kuban State University",
                    Category = "Technology",
                    MaxParticipants = 220,
                    ImageUrl = "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg"
                },

                new Event
                {
                    Id = Guid.Parse("d8a97a09-aa6a-9a17-fa4f-96aa8fa0f0e4"),
                    Title = "E-commerce & Online Business Summit",
                    Description = "Strategies for building successful online businesses, e-commerce platforms, and digital transformation in retail.",
                    StartDateTime = new DateTime(2025, 1, 15, 15, 30, 0),
                    Location = "Samara, Samara University",
                    Category = "Business",
                    MaxParticipants = 160,
                    ImageUrl = "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg"
                },

                new Event
                {
                    Id = Guid.Parse("e9a08a10-aa7a-aa28-aa5a-a7aa9aa1a1f5"),
                    Title = "UI/UX Design Workshop",
                    Description = "Hands-on workshop on user interface and user experience design, prototyping tools, and design thinking methodologies.",
                    StartDateTime = new DateTime(2025, 2, 28, 10, 30, 0),
                    Location = "Tomsk, Tomsk State University",
                    Category = "Design",
                    MaxParticipants = 90,
                    ImageUrl = "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg"
                },

                new Event
                {
                    Id = Guid.Parse("f0a19a21-aa8a-ba39-aa6a-b8aa0aa2a2a6"),
                    Title = "IoT & Smart Cities Conference",
                    Description = "Exploring Internet of Things technologies, smart city solutions, and connected devices for urban development.",
                    StartDateTime = new DateTime(2025, 3, 14, 14, 0, 0),
                    Location = "Krasnoyarsk, Siberian Federal University",
                    Category = "Technology",
                    MaxParticipants = 140,
                    ImageUrl = "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg"
                },

                new Event
                {
                    Id = Guid.Parse("a1a20a32-aa9a-ca40-aa7a-c9aa1aa3a3a7"),
                    Title = "FinTech Innovation Forum",
                    Description = "Latest innovations in financial technology, digital banking, payment systems, and regulatory compliance in fintech.",
                    StartDateTime = new DateTime(2025, 4, 30, 11, 0, 0),
                    Location = "Perm, Perm State University",
                    Category = "Finance",
                    MaxParticipants = 170,
                    ImageUrl = "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg"
                }
                );
        });


        modelBuilder.Entity<ParticipantEvent>(builder =>
        {
            builder.HasKey(pe => new { pe.ParticipantId, pe.EventId });
        });

        modelBuilder.Entity<ParticipantEvent>(builder =>
        {
            builder.HasOne(pe => pe.Participant)
                .WithMany(p => p.ParticipantEvents)
                .HasForeignKey(pe => pe.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ParticipantEvent>(builder =>
        {
            builder.HasOne(pe => pe.Event)
                .WithMany(e => e.ParticipantEvents)
                .HasForeignKey(pe => pe.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
