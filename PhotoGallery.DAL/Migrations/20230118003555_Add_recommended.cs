using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoGallery.DAL.Migrations
{
    public partial class Add_recommended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recommended",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoId = table.Column<int>(type: "int", nullable: true),
                    RecommendedPhotoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommended", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recommended_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: -2,
                column: "ConcurrencyStamp",
                value: "820b8e14-628d-45d3-b1a6-325676807d03");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: -1,
                column: "ConcurrencyStamp",
                value: "aa2bc262-0585-4456-95aa-9a7091b4a7e0");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6193ddae-d445-419a-96fb-fb5b13e8daea", "AQAAAAEAACcQAAAAENJ7S1QquKRWvNknsU1N8+/wggkZF3Fs4+KywiY4xQs9WD3aAPHVJ0uHsd91COLKkw==" });

            migrationBuilder.CreateIndex(
                name: "IX_Recommended_PhotoId",
                table: "Recommended",
                column: "PhotoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recommended");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: -2,
                column: "ConcurrencyStamp",
                value: "e7c2b6eb-2bd4-45b4-890e-c227199d8f4a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: -1,
                column: "ConcurrencyStamp",
                value: "b185aaaf-506d-451a-844f-d3d4505e1c29");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3e119f8f-f914-47f5-a8b8-bd221f38d1f4", "AQAAAAEAACcQAAAAEHSw0F48/iKAV1+h9gXl6SdR9TkYuKe5PLTsN+1LfxDHvdIjjG7t9ryDvNmjxcXL7Q==" });
        }
    }
}
