using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoList.Data.Migrations
{
    public partial class SingleTaskFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleTask_TasksToDo_TaskToDoId",
                table: "SingleTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SingleTask",
                table: "SingleTask");

            migrationBuilder.RenameTable(
                name: "SingleTask",
                newName: "SingleTasks");

            migrationBuilder.RenameColumn(
                name: "Task",
                table: "SingleTasks",
                newName: "Label");

            migrationBuilder.RenameIndex(
                name: "IX_SingleTask_TaskToDoId",
                table: "SingleTasks",
                newName: "IX_SingleTasks_TaskToDoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SingleTasks",
                table: "SingleTasks",
                column: "SingleTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_SingleTasks_TasksToDo_TaskToDoId",
                table: "SingleTasks",
                column: "TaskToDoId",
                principalTable: "TasksToDo",
                principalColumn: "TaskToDoId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleTasks_TasksToDo_TaskToDoId",
                table: "SingleTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SingleTasks",
                table: "SingleTasks");

            migrationBuilder.RenameTable(
                name: "SingleTasks",
                newName: "SingleTask");

            migrationBuilder.RenameColumn(
                name: "Label",
                table: "SingleTask",
                newName: "Task");

            migrationBuilder.RenameIndex(
                name: "IX_SingleTasks_TaskToDoId",
                table: "SingleTask",
                newName: "IX_SingleTask_TaskToDoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SingleTask",
                table: "SingleTask",
                column: "SingleTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_SingleTask_TasksToDo_TaskToDoId",
                table: "SingleTask",
                column: "TaskToDoId",
                principalTable: "TasksToDo",
                principalColumn: "TaskToDoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
