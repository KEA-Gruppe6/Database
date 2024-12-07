using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database_project.Migrations
{
    /// <inheritdoc />
    public partial class AddOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirlineConfirmationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_OrderId",
                table: "Tickets",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Orders_OrderId",
                table: "Tickets",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");

            // Insert some data
            migrationBuilder.Sql("MERGE INTO Orders AS target USING (VALUES('111AAA'),('222BBB'),('333CCC'),('444DDD'),('555EEE'),('666FFF'),('777GGG'),('888HHH'),('999III'),('101JJJ')) AS source (AirlineConfirmationNumber) ON target.AirlineConfirmationNumber = source.AirlineConfirmationNumber WHEN NOT MATCHED BY TARGET THEN INSERT (AirlineConfirmationNumber) VALUES (source.AirlineConfirmationNumber);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Orders_OrderId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_OrderId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Tickets");
        }
    }
}
