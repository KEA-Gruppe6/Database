using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database_project.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketIdToLuggage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Luggage_Tickets_TicketId",
                table: "Luggage");

            migrationBuilder.AlterColumn<long>(
                name: "TicketId",
                table: "Luggage",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Luggage_Tickets_TicketId",
                table: "Luggage",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Luggage_Tickets_TicketId",
                table: "Luggage");

            migrationBuilder.AlterColumn<long>(
                name: "TicketId",
                table: "Luggage",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Luggage_Tickets_TicketId",
                table: "Luggage",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId");
        }
    }
}
