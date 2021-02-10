using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SimpleChatApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "users",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'4', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'60', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_messages_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "users",
                columns: new[] { "id", "username" },
                values: new object[,]
                {
                    { 1, "Test User" },
                    { 2, "John Doe" },
                    { 3, "Jane Doe" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "messages",
                columns: new[] { "id", "text", "user_id" },
                values: new object[,]
                {
                    { 1, "Test message number 0 by Test User", 1 },
                    { 28, "Test message number 7 by Jane Doe", 3 },
                    { 27, "Test message number 6 by Jane Doe", 3 },
                    { 26, "Test message number 5 by Jane Doe", 3 },
                    { 25, "Test message number 4 by Jane Doe", 3 },
                    { 24, "Test message number 3 by Jane Doe", 3 },
                    { 23, "Test message number 2 by Jane Doe", 3 },
                    { 22, "Test message number 1 by Jane Doe", 3 },
                    { 21, "Test message number 0 by Jane Doe", 3 },
                    { 20, "Test message number 9 by John Doe", 2 },
                    { 19, "Test message number 8 by John Doe", 2 },
                    { 18, "Test message number 7 by John Doe", 2 },
                    { 17, "Test message number 6 by John Doe", 2 },
                    { 16, "Test message number 5 by John Doe", 2 },
                    { 15, "Test message number 4 by John Doe", 2 },
                    { 14, "Test message number 3 by John Doe", 2 },
                    { 13, "Test message number 2 by John Doe", 2 },
                    { 12, "Test message number 1 by John Doe", 2 },
                    { 11, "Test message number 0 by John Doe", 2 },
                    { 10, "Test message number 9 by Test User", 1 },
                    { 9, "Test message number 8 by Test User", 1 },
                    { 8, "Test message number 7 by Test User", 1 },
                    { 7, "Test message number 6 by Test User", 1 },
                    { 6, "Test message number 5 by Test User", 1 },
                    { 5, "Test message number 4 by Test User", 1 },
                    { 4, "Test message number 3 by Test User", 1 },
                    { 3, "Test message number 2 by Test User", 1 },
                    { 2, "Test message number 1 by Test User", 1 },
                    { 29, "Test message number 8 by Jane Doe", 3 },
                    { 30, "Test message number 9 by Jane Doe", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "ix_messages_user_id",
                schema: "public",
                table: "messages",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_username",
                schema: "public",
                table: "users",
                column: "username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "messages",
                schema: "public");

            migrationBuilder.DropTable(
                name: "users",
                schema: "public");
        }
    }
}
