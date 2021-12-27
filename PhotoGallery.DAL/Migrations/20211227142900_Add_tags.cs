using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoGallery.DAL.Migrations
{
    public partial class Add_tags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhotoTag",
                columns: table => new
                {
                    PhotosId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoTag", x => new { x.PhotosId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_PhotoTag_Photos_PhotosId",
                        column: x => x.PhotosId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotoTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagUser",
                columns: table => new
                {
                    TagsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagUser", x => new { x.TagsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_TagUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagUser_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_PhotoTag_TagsId",
                table: "PhotoTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_TagUser_UsersId",
                table: "TagUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhotoTag");

            migrationBuilder.DropTable(
                name: "TagUser");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: -2,
                column: "ConcurrencyStamp",
                value: "86f969d0-1489-4f09-92f1-8ccadf9bd583");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: -1,
                column: "ConcurrencyStamp",
                value: "d5704e83-a363-4940-9e96-c97abf3bd2fa");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bf16b325-04af-4966-9924-53016d5e60ef", "AQAAAAEAACcQAAAAEEsSZRtavc+6CeyIOVDL+7QlViRqNGj1HZ4bQZ9mfnUweLAB1WonBZbp949j3xJbeg==" });
        }
    }
}
