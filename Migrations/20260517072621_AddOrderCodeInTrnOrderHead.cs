using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enterprises.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderCodeInTrnOrderHead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderCode",
                table: "TrnOrderHeads",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderCode",
                table: "TrnOrderHeads");
        }
    }
}
