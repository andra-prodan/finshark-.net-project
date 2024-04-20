using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedRole2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "26550a10-f0bd-4afe-8cc5-b588aab8f230");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f65ea317-949d-49a4-a1b9-7457d903e498");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1aff03c4-a93b-4524-be9d-8210be9392e0", null, "Client", "CLIENT" },
                    { "559b0991-778d-4e25-bb8b-cc19c670c350", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1aff03c4-a93b-4524-be9d-8210be9392e0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "559b0991-778d-4e25-bb8b-cc19c670c350");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "26550a10-f0bd-4afe-8cc5-b588aab8f230", null, "Admin", "ADMIN" },
                    { "f65ea317-949d-49a4-a1b9-7457d903e498", null, "Client", "CLIENT" }
                });
        }
    }
}
