using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMSApiServer.Migrations
{
    public partial class MigrationName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_Company_CompanyName",
                table: "Job");

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "CompanyName",
                keyValue: null,
                column: "CompanyName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Job",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Company_CompanyName",
                table: "Job",
                column: "CompanyName",
                principalTable: "Company",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_Company_CompanyName",
                table: "Job");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Job",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Company_CompanyName",
                table: "Job",
                column: "CompanyName",
                principalTable: "Company",
                principalColumn: "Name");
        }
    }
}
