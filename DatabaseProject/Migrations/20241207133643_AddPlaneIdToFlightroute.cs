using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database_project.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaneIdToFlightroute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PlaneId",
                table: "Flightroutes",
                type: "bigint",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Flightroutes_PlaneId",
                table: "Flightroutes",
                column: "PlaneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flightroutes_Planes_PlaneId",
                table: "Flightroutes",
                column: "PlaneId",
                principalTable: "Planes",
                principalColumn: "PlaneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flightroutes_Planes_PlaneId",
                table: "Flightroutes");

            migrationBuilder.DropIndex(
                name: "IX_Flightroutes_PlaneId",
                table: "Flightroutes");

            migrationBuilder.DropColumn(
                name: "PlaneId",
                table: "Flightroutes");
        }
    }
}
