using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Token_Test.Migrations
{
    public partial class UsersAndProductsPatch01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_UserName1",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserName1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserName1",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserName",
                table: "Products",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_UserName",
                table: "Products",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_UserName",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserName",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserName1",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserName1",
                table: "Products",
                column: "UserName1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_UserName1",
                table: "Products",
                column: "UserName1",
                principalTable: "Users",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
