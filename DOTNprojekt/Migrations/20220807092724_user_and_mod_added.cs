using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DOTNprojekt.Migrations
{
    public partial class user_and_mod_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    E_Mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    n_Uploads = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_Id);
                });

            migrationBuilder.CreateTable(
                name: "Mods",
                columns: table => new
                {
                    Mod_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mod_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mod_Char = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mod_cat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploaderUser_Id = table.Column<int>(type: "int", nullable: false),
                    Upload_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    n_Views = table.Column<int>(type: "int", nullable: false),
                    n_Downloads = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mods", x => x.Mod_Id);
                    table.ForeignKey(
                        name: "FK_Mods_Users_UploaderUser_Id",
                        column: x => x.UploaderUser_Id,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mods_UploaderUser_Id",
                table: "Mods",
                column: "UploaderUser_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mods");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
