using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoInfrastructure.Migrations
{
    public partial class AddedNotified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Notified",
                table: "ToDoLists",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notified",
                table: "ToDoLists");
        }
    }
}
