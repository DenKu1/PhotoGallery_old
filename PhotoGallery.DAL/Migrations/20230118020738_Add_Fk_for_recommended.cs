using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoGallery.DAL.Migrations
{
    public partial class Add_Fk_for_recommended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: -2,
                column: "ConcurrencyStamp",
                value: "0a866da7-ea0e-4873-bc3b-544b93250510");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: -1,
                column: "ConcurrencyStamp",
                value: "14e30585-fdf4-44d8-9fc5-cf16c0c695cc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1d207418-8ff1-425c-92f8-c36bef391191", "AQAAAAEAACcQAAAAEOQ5Ru5ZNi1xsdNlbTzEUCn5LGAC2T7DlTgyZkwdv1tP9OIz61IzMJYNFbyOgV4sig==" });

            migrationBuilder.CreateIndex(
                name: "IX_Recommended_RecommendedPhotoId",
                table: "Recommended",
                column: "RecommendedPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recommended_Photos_RecommendedPhotoId",
                table: "Recommended",
                column: "RecommendedPhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommended_Photos_RecommendedPhotoId",
                table: "Recommended");

            migrationBuilder.DropIndex(
                name: "IX_Recommended_RecommendedPhotoId",
                table: "Recommended");

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
        }
    }
}
