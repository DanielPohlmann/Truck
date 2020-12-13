using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volvo.Trucks.API.Migrations
{
    public partial class Trucks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Model",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    ModelYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Model", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Truck",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModelId = table.Column<Guid>(nullable: false),
                    ManufactureYear = table.Column<int>(type: "int", nullable: false),
                    VIN_Number = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Truck", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Truck_Model_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Model",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Model",
                columns: new[] { "Id", "ModelYear", "Name" },
                values: new object[,]
                {
                    { new Guid("8132b9df-d14b-45ae-8b7f-9fe7ecaef8a0"), 2020, "FH" },
                    { new Guid("b6e9987a-d4c5-48a6-82fd-a470a135dbfa"), 2020, "FM" },
                    { new Guid("a6f734da-f964-41f7-b2c5-1f71f0baba47"), 2019, "FH" },
                    { new Guid("b708de2e-85dc-418a-85e0-0fabfe2c7903"), 2019, "FM" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Truck_ModelId",
                table: "Truck",
                column: "ModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Truck");

            migrationBuilder.DropTable(
                name: "Model");
        }
    }
}
