using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIseries.Migrations
{
    /// <inheritdoc />
    public partial class RenameCodePostalToCp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "utl_codepostal",
                schema: "public",
                table: "t_e_utilisateur_utl",
                newName: "utl_cp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "utl_cp",
                schema: "public",
                table: "t_e_utilisateur_utl",
                newName: "utl_codepostal");
        }
    }
}
