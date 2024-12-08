using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database_project.Migrations
{
    /// <inheritdoc />
    public partial class MakeAirlineReferenceToPlane : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Airlines_AirlineId",
                table: "Planes");

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Airlines_AirlineId",
                table: "Planes",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "AirlineId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Airlines_AirlineId",
                table: "Planes");

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Airlines_AirlineId",
                table: "Planes",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "AirlineId");
        }
    }
}
