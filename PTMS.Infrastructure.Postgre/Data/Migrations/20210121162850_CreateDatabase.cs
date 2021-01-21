using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PTMS.Infrastructure.Postgre.Data.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ptms");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "ptms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Margins",
                schema: "ptms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Top = table.Column<string>(type: "text", nullable: true),
                    Bottom = table.Column<string>(type: "text", nullable: true),
                    Right = table.Column<string>(type: "text", nullable: true),
                    Left = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Margins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Partials",
                schema: "ptms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Rev = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    TemplateBody = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Editor = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Category = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Version = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                schema: "ptms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Scale = table.Column<int>(type: "integer", nullable: false),
                    DisplayHeaderFooter = table.Column<bool>(type: "boolean", nullable: false),
                    HeaderTemplate = table.Column<string>(type: "text", nullable: true),
                    FooterTemplate = table.Column<string>(type: "text", nullable: true),
                    PrintBackground = table.Column<bool>(type: "boolean", nullable: false),
                    Landscape = table.Column<bool>(type: "boolean", nullable: false),
                    Format = table.Column<string>(type: "text", nullable: true),
                    MarginId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Options_Margins_MarginId",
                        column: x => x.MarginId,
                        principalSchema: "ptms",
                        principalTable: "Margins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                schema: "ptms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Rev = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Creator = table.Column<string>(type: "text", nullable: true),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Editor = table.Column<string>(type: "text", nullable: true),
                    TemplateBody = table.Column<string>(type: "text", nullable: true),
                    Caching = table.Column<bool>(type: "boolean", nullable: false),
                    LoadExternalSources = table.Column<bool>(type: "boolean", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<string>(type: "text", nullable: true),
                    OptionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_Options_OptionId",
                        column: x => x.OptionId,
                        principalSchema: "ptms",
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbVersion",
                schema: "ptms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VersionNumber = table.Column<string>(type: "text", nullable: true),
                    Creator = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TemplateBody = table.Column<string>(type: "text", nullable: true),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbVersion_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalSchema: "ptms",
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbVersion_TemplateId",
                schema: "ptms",
                table: "DbVersion",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Options_MarginId",
                schema: "ptms",
                table: "Options",
                column: "MarginId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_OptionId",
                schema: "ptms",
                table: "Templates",
                column: "OptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories",
                schema: "ptms");

            migrationBuilder.DropTable(
                name: "DbVersion",
                schema: "ptms");

            migrationBuilder.DropTable(
                name: "Partials",
                schema: "ptms");

            migrationBuilder.DropTable(
                name: "Templates",
                schema: "ptms");

            migrationBuilder.DropTable(
                name: "Options",
                schema: "ptms");

            migrationBuilder.DropTable(
                name: "Margins",
                schema: "ptms");
        }
    }
}
