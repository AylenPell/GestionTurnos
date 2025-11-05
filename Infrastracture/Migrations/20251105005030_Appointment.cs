using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Appointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDateTime",
                table: "Appointments");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "AppointmentTime",
                table: "Appointments",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateOnly>(
                name: "AppointmentDate",
                table: "Appointments",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDate",
                table: "Appointments");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "AppointmentTime",
                table: "Appointments",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "AppointmentDateTime",
                table: "Appointments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
