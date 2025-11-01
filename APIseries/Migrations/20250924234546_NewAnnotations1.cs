using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIseries.Migrations
{
    /// <inheritdoc />
    public partial class NewAnnotations1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "utl_cp",
                schema: "public",
                table: "t_e_utilisateur_utl",
                newName: "utl_codepostal");

            migrationBuilder.AlterColumn<string>(
                name: "utl_ville",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "utl_rue",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "utl_pwd",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(64)");

            migrationBuilder.AlterColumn<string>(
                name: "utl_prenom",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "utl_pays",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "France",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldDefaultValue: "France");

            migrationBuilder.AlterColumn<string>(
                name: "utl_nom",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "utl_mobile",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "utl_mail",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "utl_datecreation",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<string>(
                name: "utl_codepostal",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(5)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "utl_codepostal",
                schema: "public",
                table: "t_e_utilisateur_utl",
                newName: "utl_cp");

            migrationBuilder.AlterColumn<string>(
                name: "utl_ville",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "utl_rue",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "varchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "utl_pwd",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "varchar(64)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "utl_prenom",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "utl_pays",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "varchar(50)",
                nullable: true,
                defaultValue: "France",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "France");

            migrationBuilder.AlterColumn<string>(
                name: "utl_nom",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "utl_mobile",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "char(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "utl_mail",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "utl_datecreation",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "date",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<string>(
                name: "utl_cp",
                schema: "public",
                table: "t_e_utilisateur_utl",
                type: "char(5)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
