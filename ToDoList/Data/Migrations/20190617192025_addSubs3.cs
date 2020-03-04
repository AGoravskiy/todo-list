using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoList.Data.Migrations
{
    public partial class addSubs3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSubscribers",
                columns: table => new
                {
                    UserSubscriberId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubscriberId1 = table.Column<string>(nullable: true),
                    SubscriberId = table.Column<int>(nullable: false),
                    SubscribionerId1 = table.Column<string>(nullable: true),
                    SubscribionerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscribers", x => x.UserSubscriberId);
                    table.ForeignKey(
                        name: "FK_UserSubscribers_AspNetUsers_SubscriberId1",
                        column: x => x.SubscriberId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSubscribers_AspNetUsers_SubscribionerId1",
                        column: x => x.SubscribionerId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscribers_SubscriberId1",
                table: "UserSubscribers",
                column: "SubscriberId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscribers_SubscribionerId1",
                table: "UserSubscribers",
                column: "SubscribionerId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSubscribers");
        }
    }
}
