using Microsoft.EntityFrameworkCore.Migrations;

namespace ChaVoV1.Migrations
{
    public partial class AddingSomeCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryText" },
                values: new object[] { 1, "Coding" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryText" },
                values: new object[] { 2, "Cooking" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryText" },
                values: new object[] { 3, "Driving" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
