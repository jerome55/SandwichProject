using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ClientProject.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    chkCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Sandwiches",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sandwiches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Vegetables",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vegetables", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    companyid = table.Column<int>(nullable: true),
                    firstName = table.Column<string>(nullable: true),
                    lastName = table.Column<string>(nullable: true),
                    login = table.Column<string>(nullable: true),
                    mail = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    wallet = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.id);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_companyid",
                        column: x => x.companyid,
                        principalTable: "Companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderLines",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    quantity = table.Column<int>(nullable: false),
                    sandwichid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLines", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrderLines_Sandwiches_sandwichid",
                        column: x => x.sandwichid,
                        principalTable: "Sandwiches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Employeeid = table.Column<int>(nullable: true),
                    dateOfDelivery = table.Column<DateTime>(nullable: false),
                    totalAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_Orders_Employees_Employeeid",
                        column: x => x.Employeeid,
                        principalTable: "Employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderLineVegetables",
                columns: table => new
                {
                    orderLineId = table.Column<int>(nullable: false),
                    vegetableId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLineVegetables", x => new { x.orderLineId, x.vegetableId });
                    table.ForeignKey(
                        name: "FK_OrderLineVegetables_OrderLines_orderLineId",
                        column: x => x.orderLineId,
                        principalTable: "OrderLines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLineVegetables_Vegetables_vegetableId",
                        column: x => x.vegetableId,
                        principalTable: "Vegetables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_companyid",
                table: "Employees",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Employeeid",
                table: "Orders",
                column: "Employeeid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_sandwichid",
                table: "OrderLines",
                column: "sandwichid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLineVegetables_orderLineId",
                table: "OrderLineVegetables",
                column: "orderLineId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLineVegetables_vegetableId",
                table: "OrderLineVegetables",
                column: "vegetableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "OrderLineVegetables");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "OrderLines");

            migrationBuilder.DropTable(
                name: "Vegetables");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Sandwiches");
        }
    }
}
