using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Endpoints.Panel.Front.Migrations
{
    public partial class InitialModel01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookSubscriptions_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookSubscriptions_BookItems_BookItemId",
                        column: x => x.BookItemId,
                        principalTable: "BookItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ApplicationUsers",
                columns: new[] { "Id", "Created", "CreatedBy", "Email" },
                values: new object[] { new Guid("8b65574d-8394-4574-8e3b-f1a9b76e50c9"), new DateTime(2022, 10, 12, 11, 22, 14, 0, DateTimeKind.Unspecified), null, "admin@live.com" });

            migrationBuilder.InsertData(
                table: "BookItems",
                columns: new[] { "Id", "Created", "CreatedBy", "Name", "Price", "Text" },
                values: new object[,]
                {
                    { new Guid("67e577d6-c1b7-4516-ab5f-885b3df1d22b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "C#", 14.2m, "C# is a general-purpose, multi-paradigm programming language" },
                    { new Guid("49d8d8bb-a49b-4d1c-a6e6-1a3660fb9771"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Asp.net", 31.5m, "ASP.NET is an open-source, server-side web-application framework" },
                    { new Guid("291dca11-cdad-4863-8d01-265ce768cea8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Angular", 23.9m, "Angular is a TypeScript-based free and open-source web application framework" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookSubscriptions_ApplicationUserId",
                table: "BookSubscriptions",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookSubscriptions_BookItemId",
                table: "BookSubscriptions",
                column: "BookItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookSubscriptions");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "BookItems");
        }
    }
}
