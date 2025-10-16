using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class specialty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Professionals");

            migrationBuilder.AddColumn<int>(
                name: "SpecialtyId",
                table: "Professionals",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_SpecialtyId",
                table: "Professionals",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Professionals_Specialties_SpecialtyId",
                table: "Professionals",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professionals_Specialties_SpecialtyId",
                table: "Professionals");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Professionals_SpecialtyId",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                table: "Professionals");

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "Professionals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
