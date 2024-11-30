using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database_project.Migrations
{
    /// <inheritdoc />
    public partial class RenameDepartureToFlightroute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departures_Airports_ArrivalAirportId",
                table: "Departures");

            migrationBuilder.DropForeignKey(
                name: "FK_Departures_Airports_DepartureAirportId",
                table: "Departures");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Departures_DepartureId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Orders_OrderId",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departures",
                table: "Departures");

            migrationBuilder.RenameTable(
                name: "Departures",
                newName: "Flightroutes");

            migrationBuilder.RenameColumn(
                name: "DepartureId",
                table: "Tickets",
                newName: "FlightrouteId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_DepartureId",
                table: "Tickets",
                newName: "IX_Tickets_FlightrouteId");

            migrationBuilder.RenameColumn(
                name: "DepartureId",
                table: "Flightroutes",
                newName: "FlightrouteId");

            migrationBuilder.RenameIndex(
                name: "IX_Departures_DepartureAirportId",
                table: "Flightroutes",
                newName: "IX_Flightroutes_DepartureAirportId");

            migrationBuilder.RenameIndex(
                name: "IX_Departures_ArrivalAirportId",
                table: "Flightroutes",
                newName: "IX_Flightroutes_ArrivalAirportId");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flightroutes",
                table: "Flightroutes",
                column: "FlightrouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flightroutes_Airports_ArrivalAirportId",
                table: "Flightroutes",
                column: "ArrivalAirportId",
                principalTable: "Airports",
                principalColumn: "AirportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flightroutes_Airports_DepartureAirportId",
                table: "Flightroutes",
                column: "DepartureAirportId",
                principalTable: "Airports",
                principalColumn: "AirportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Flightroutes_FlightrouteId",
                table: "Tickets",
                column: "FlightrouteId",
                principalTable: "Flightroutes",
                principalColumn: "FlightrouteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Orders_OrderId",
                table: "Tickets",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flightroutes_Airports_ArrivalAirportId",
                table: "Flightroutes");

            migrationBuilder.DropForeignKey(
                name: "FK_Flightroutes_Airports_DepartureAirportId",
                table: "Flightroutes");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Flightroutes_FlightrouteId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Orders_OrderId",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flightroutes",
                table: "Flightroutes");

            migrationBuilder.RenameTable(
                name: "Flightroutes",
                newName: "Departures");

            migrationBuilder.RenameColumn(
                name: "FlightrouteId",
                table: "Tickets",
                newName: "DepartureId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_FlightrouteId",
                table: "Tickets",
                newName: "IX_Tickets_DepartureId");

            migrationBuilder.RenameColumn(
                name: "FlightrouteId",
                table: "Departures",
                newName: "DepartureId");

            migrationBuilder.RenameIndex(
                name: "IX_Flightroutes_DepartureAirportId",
                table: "Departures",
                newName: "IX_Departures_DepartureAirportId");

            migrationBuilder.RenameIndex(
                name: "IX_Flightroutes_ArrivalAirportId",
                table: "Departures",
                newName: "IX_Departures_ArrivalAirportId");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departures",
                table: "Departures",
                column: "DepartureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departures_Airports_ArrivalAirportId",
                table: "Departures",
                column: "ArrivalAirportId",
                principalTable: "Airports",
                principalColumn: "AirportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departures_Airports_DepartureAirportId",
                table: "Departures",
                column: "DepartureAirportId",
                principalTable: "Airports",
                principalColumn: "AirportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Departures_DepartureId",
                table: "Tickets",
                column: "DepartureId",
                principalTable: "Departures",
                principalColumn: "DepartureId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Orders_OrderId",
                table: "Tickets",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");
        }
    }
}
