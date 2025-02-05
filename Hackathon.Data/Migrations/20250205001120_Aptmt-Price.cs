using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.Data.Migrations
{
    /// <inheritdoc />
    public partial class AptmtPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                schema: "Hackathon",
                table: "Appointment",
                type: "FLOAT",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                schema: "Hackathon",
                table: "Appointment");
        }
    }
}
