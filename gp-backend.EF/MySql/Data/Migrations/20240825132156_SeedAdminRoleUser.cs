using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gp_backend.EF.MySql.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminRoleUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cbcc5d1f-572b-44ea-bfe2-dfa5d0d1bc8f", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "71c7d19b-93ef-42de-a43c-7a2178dd6d48", 0, "966b42d3-93d5-4786-a56b-24b4fcd5d3da", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN", "AQAAAAIAAYagAAAAEGRM+dqr7nVpVrrC8tBIEJez3T07mfBaW881b5eC0S/og/uwa5g4LL6E7qhdBI674g==", null, false, "4802716e-fc8e-4a2f-9828-a92f1eaf06c5", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "cbcc5d1f-572b-44ea-bfe2-dfa5d0d1bc8f", "71c7d19b-93ef-42de-a43c-7a2178dd6d48" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "cbcc5d1f-572b-44ea-bfe2-dfa5d0d1bc8f", "71c7d19b-93ef-42de-a43c-7a2178dd6d48" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cbcc5d1f-572b-44ea-bfe2-dfa5d0d1bc8f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71c7d19b-93ef-42de-a43c-7a2178dd6d48");
        }
    }
}
