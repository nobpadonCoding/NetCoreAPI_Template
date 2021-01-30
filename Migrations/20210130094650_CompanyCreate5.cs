using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreAPI_Template_v2.Migrations
{
    public partial class CompanyCreate5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Position_PositionId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_PositionId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "Departments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_PositionId",
                table: "Departments",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Position_PositionId",
                table: "Departments",
                column: "PositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
