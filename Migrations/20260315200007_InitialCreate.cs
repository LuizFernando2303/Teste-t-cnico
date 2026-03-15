using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeGastos.Migrations
{
  /// <inheritdoc />
  public partial class InitialCreate : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Transactions",
          columns: table => new
          {
            Id = table.Column<int>(type: "INTEGER", nullable: false)
                  .Annotation("Sqlite:Autoincrement", true),
            Description = table.Column<string>(type: "TEXT", maxLength: 400, nullable: false),
            Value = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
            Type = table.Column<int>(type: "INTEGER", nullable: false),
            CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
            UserId = table.Column<int>(type: "INTEGER", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Transactions", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Users",
          columns: table => new
          {
            Id = table.Column<int>(type: "INTEGER", nullable: false)
                  .Annotation("Sqlite:Autoincrement", true),
            Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
            Email = table.Column<string>(type: "TEXT", nullable: false),
            Age = table.Column<int>(type: "INTEGER", nullable: false),
            PasswordHash = table.Column<string>(type: "TEXT", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Users", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Categories",
          columns: table => new
          {
            Id = table.Column<int>(type: "INTEGER", nullable: false)
                  .Annotation("Sqlite:Autoincrement", true),
            UserId = table.Column<int>(type: "INTEGER", nullable: false),
            Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
            Purpose = table.Column<int>(type: "INTEGER", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Categories", x => x.Id);
            table.ForeignKey(
                      name: "FK_Categories_Users_UserId",
                      column: x => x.UserId,
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_Categories_UserId",
          table: "Categories",
          column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Categories");

      migrationBuilder.DropTable(
          name: "Transactions");

      migrationBuilder.DropTable(
          name: "Users");
    }
  }
}
