using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoInfrastructure.Migrations
{
    public partial class AddedSharedListFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDoSharedLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoSharedLists", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDoSharedLists");
        }
    }
}
