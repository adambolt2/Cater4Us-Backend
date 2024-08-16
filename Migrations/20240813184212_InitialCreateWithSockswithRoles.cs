using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cater4Us_Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithSockswithRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FoodName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FoodDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Allergens = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FoodType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockCount = table.Column<int>(type: "int", nullable: true),
                    OutofStock = table.Column<bool>(type: "bit", nullable: true),
                    PricePerPiece = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerificationCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodModel");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
