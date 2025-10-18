using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changespecialtyfuckenumaddProfessionalSpecialtyrelationentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professionals_Specialties_SpecialtyId",
                table: "Professionals");

            migrationBuilder.DropIndex(
                name: "IX_Professionals_SpecialtyId",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                table: "Professionals");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Specialties",
                newName: "SpecialtyName");

            migrationBuilder.CreateTable(
                name: "ProfessionalSpecialties",
                columns: table => new
                {
                    ProfessionalId = table.Column<int>(type: "INTEGER", nullable: false),
                    SpecialtyId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionalSpecialties", x => new { x.ProfessionalId, x.SpecialtyId });
                    table.ForeignKey(
                        name: "FK_ProfessionalSpecialties_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfessionalSpecialties_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalSpecialties_ProfessionalId",
                table: "ProfessionalSpecialties",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalSpecialties_SpecialtyId",
                table: "ProfessionalSpecialties",
                column: "SpecialtyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfessionalSpecialties");

            migrationBuilder.RenameColumn(
                name: "SpecialtyName",
                table: "Specialties",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "SpecialtyId",
                table: "Professionals",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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
    }
}
