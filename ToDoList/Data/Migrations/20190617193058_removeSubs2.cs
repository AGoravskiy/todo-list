using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoList.Data.Migrations
{
    public partial class removeSubs2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscribers_AspNetUsers_SubscriberId1",
                table: "UserSubscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscribers_AspNetUsers_SubscribionerId1",
                table: "UserSubscribers");

            migrationBuilder.DropIndex(
                name: "IX_UserSubscribers_SubscriberId1",
                table: "UserSubscribers");

            migrationBuilder.DropIndex(
                name: "IX_UserSubscribers_SubscribionerId1",
                table: "UserSubscribers");

            migrationBuilder.DropColumn(
                name: "SubscriberId1",
                table: "UserSubscribers");

            migrationBuilder.DropColumn(
                name: "SubscribionerId1",
                table: "UserSubscribers");

            migrationBuilder.AlterColumn<string>(
                name: "SubscribionerId",
                table: "UserSubscribers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "SubscriberId",
                table: "UserSubscribers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscribers_SubscriberId",
                table: "UserSubscribers",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscribers_SubscribionerId",
                table: "UserSubscribers",
                column: "SubscribionerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscribers_AspNetUsers_SubscriberId",
                table: "UserSubscribers",
                column: "SubscriberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscribers_AspNetUsers_SubscribionerId",
                table: "UserSubscribers",
                column: "SubscribionerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscribers_AspNetUsers_SubscriberId",
                table: "UserSubscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscribers_AspNetUsers_SubscribionerId",
                table: "UserSubscribers");

            migrationBuilder.DropIndex(
                name: "IX_UserSubscribers_SubscriberId",
                table: "UserSubscribers");

            migrationBuilder.DropIndex(
                name: "IX_UserSubscribers_SubscribionerId",
                table: "UserSubscribers");

            migrationBuilder.AlterColumn<int>(
                name: "SubscribionerId",
                table: "UserSubscribers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubscriberId",
                table: "UserSubscribers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscriberId1",
                table: "UserSubscribers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscribionerId1",
                table: "UserSubscribers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscribers_SubscriberId1",
                table: "UserSubscribers",
                column: "SubscriberId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscribers_SubscribionerId1",
                table: "UserSubscribers",
                column: "SubscribionerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscribers_AspNetUsers_SubscriberId1",
                table: "UserSubscribers",
                column: "SubscriberId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscribers_AspNetUsers_SubscribionerId1",
                table: "UserSubscribers",
                column: "SubscribionerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
