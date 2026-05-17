using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enterprises.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "TrnOrderHeads",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "TrnOrderDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "MstItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "MstAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "TrnOrderHeads");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "TrnOrderDetails");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "MstItems");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "MstAccounts");
        }
    }
}
