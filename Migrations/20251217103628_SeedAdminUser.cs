using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentalHealthSystem.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedOn", "Email", "HashSalt", "IsDeleted", "IsDeletedBy", "IsDeletedOn", "LastModifiedBy", "LastModifiedOn", "PasswordHash", "Role", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "admin@mentalhealthsystem.com", "U8KvVHr2LwBnUXJh0Hqz7Q==", false, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Xx0c8BqF3p1YwN1/v6K3rGZE8JqE3p1YwN1/v6K3rA==", "Admin", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));
        }
    }
}
