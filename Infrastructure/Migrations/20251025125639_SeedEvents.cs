using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Category", "Description", "ImageUrl", "Location", "MaxParticipants", "StartDateTime", "Title" },
                values: new object[,]
                {
                    { new Guid("2c8e0f9b-18c4-45d7-b48b-09b55f585935"), "Technology", "Comprehensive conference covering the latest advances in artificial intelligence, machine learning algorithms, and their practical applications in various industries.", "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg", "Moscow, Digital October Center", 300, new DateTime(2025, 6, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), "AI & Machine Learning Summit 2025" },
                    { new Guid("2c8e0f9b-18c4-45d7-b48b-09b55f585936"), "Technology", "Hands-on workshop on cloud technologies, microservices architecture, and DevOps practices for modern software development.", "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg", "St. Petersburg, ITMO University", 150, new DateTime(2025, 4, 22, 14, 30, 0, 0, DateTimeKind.Unspecified), "Cloud Computing Workshop" },
                    { new Guid("2c8e0f9b-18c4-45d7-b48b-09b55f585937"), "Technology", "Expert discussions on cybersecurity threats, data protection strategies, and emerging security technologies for enterprises.", "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg", "Novosibirsk, Technopark Akademgorodok", 250, new DateTime(2025, 8, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "Cybersecurity Conference" },
                    { new Guid("a1a20a32-aa9a-ca40-aa7a-c9aa1aa3a3a7"), "Finance", "Latest innovations in financial technology, digital banking, payment systems, and regulatory compliance in fintech.", "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg", "Perm, Perm State University", 170, new DateTime(2025, 4, 30, 11, 0, 0, 0, DateTimeKind.Unspecified), "FinTech Innovation Forum" },
                    { new Guid("a5d64d76-ae3f-6d84-ca1c-63fa5ca7c7b1"), "Technology", "Comprehensive bootcamp covering iOS and Android development, cross-platform frameworks, and app store optimization.", "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg", "Rostov-on-Don, Southern Federal University", 120, new DateTime(2025, 10, 8, 9, 30, 0, 0, DateTimeKind.Unspecified), "Mobile App Development Bootcamp" },
                    { new Guid("b6e75e87-af4a-7e95-da2d-74aa6da8d8c2"), "Technology", "Advanced techniques in data analysis, machine learning models, and big data processing for business intelligence.", "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg", "Vladivostok, Far Eastern Federal University", 180, new DateTime(2025, 11, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), "Data Science & Analytics Summit" },
                    { new Guid("c7f86f98-aa5a-8f06-ea3e-85aa7ea9e9d3"), "Technology", "Comprehensive conference on game design, programming, graphics, and the latest trends in the gaming industry.", "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg", "Krasnodar, Kuban State University", 220, new DateTime(2025, 12, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), "Game Development Conference" },
                    { new Guid("d8a97a09-aa6a-9a17-fa4f-96aa8fa0f0e4"), "Business", "Strategies for building successful online businesses, e-commerce platforms, and digital transformation in retail.", "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg", "Samara, Samara University", 160, new DateTime(2025, 1, 15, 15, 30, 0, 0, DateTimeKind.Unspecified), "E-commerce & Online Business Summit" },
                    { new Guid("e3b42b54-ec1f-4b62-ae9a-41de3aef5a58"), "Business", "Competition for innovative startups to present their ideas to investors and industry experts. Prizes include funding opportunities and mentorship.", "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg", "Kazan, IT Park", 100, new DateTime(2025, 5, 18, 16, 0, 0, 0, DateTimeKind.Unspecified), "Startup Pitch Competition" },
                    { new Guid("e3b42b54-ec1f-4b62-ae9a-41de3aef5a59"), "Marketing", "Intensive course on modern digital marketing strategies, social media advertising, and content creation for business growth.", "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg", "Yekaterinburg, Ural Federal University", 80, new DateTime(2025, 7, 25, 11, 0, 0, 0, DateTimeKind.Unspecified), "Digital Marketing Masterclass" },
                    { new Guid("e9a08a10-aa7a-aa28-aa5a-a7aa9aa1a1f5"), "Design", "Hands-on workshop on user interface and user experience design, prototyping tools, and design thinking methodologies.", "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg", "Tomsk, Tomsk State University", 90, new DateTime(2025, 2, 28, 10, 30, 0, 0, DateTimeKind.Unspecified), "UI/UX Design Workshop" },
                    { new Guid("f0a19a21-aa8a-ba39-aa6a-b8aa0aa2a2a6"), "Technology", "Exploring Internet of Things technologies, smart city solutions, and connected devices for urban development.", "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg", "Krasnoyarsk, Siberian Federal University", 140, new DateTime(2025, 3, 14, 14, 0, 0, 0, DateTimeKind.Unspecified), "IoT & Smart Cities Conference" },
                    { new Guid("f4c53c65-fd2e-5c73-bf0b-52ef4bf6b6a0"), "Finance", "Exploring the future of blockchain technology, DeFi protocols, and the impact of cryptocurrencies on traditional finance.", "/images/events/5f8839be-ad91-415d-94d3-5dd4bd55c9b8.jpg", "Nizhny Novgorod, Lobachevsky University", 200, new DateTime(2025, 9, 12, 13, 0, 0, 0, DateTimeKind.Unspecified), "Blockchain & Cryptocurrency Forum" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("2c8e0f9b-18c4-45d7-b48b-09b55f585935"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("2c8e0f9b-18c4-45d7-b48b-09b55f585936"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("2c8e0f9b-18c4-45d7-b48b-09b55f585937"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("a1a20a32-aa9a-ca40-aa7a-c9aa1aa3a3a7"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("a5d64d76-ae3f-6d84-ca1c-63fa5ca7c7b1"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("b6e75e87-af4a-7e95-da2d-74aa6da8d8c2"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("c7f86f98-aa5a-8f06-ea3e-85aa7ea9e9d3"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("d8a97a09-aa6a-9a17-fa4f-96aa8fa0f0e4"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("e3b42b54-ec1f-4b62-ae9a-41de3aef5a58"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("e3b42b54-ec1f-4b62-ae9a-41de3aef5a59"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("e9a08a10-aa7a-aa28-aa5a-a7aa9aa1a1f5"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("f0a19a21-aa8a-ba39-aa6a-b8aa0aa2a2a6"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("f4c53c65-fd2e-5c73-bf0b-52ef4bf6b6a0"));
        }
    }
}
