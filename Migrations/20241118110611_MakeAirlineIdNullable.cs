using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database_project.Migrations
{
    /// <inheritdoc />
    public partial class MakeAirlineIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Airlines_AirlineId",
                table: "Planes");

            migrationBuilder.AlterColumn<long>(
                name: "AirlineId",
                table: "Planes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Airlines_AirlineId",
                table: "Planes",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "AirlineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Airlines_AirlineId",
                table: "Planes");

            migrationBuilder.AlterColumn<long>(
                name: "AirlineId",
                table: "Planes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Airlines_AirlineId",
                table: "Planes",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "AirlineId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
