using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserId_WorkspaceId",
                table: "Expenses",
                columns: new[] { "UserId", "WorkspaceId" });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_WorkspaceId",
                table: "Expenses",
                column: "WorkspaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Expenses_UserId_WorkspaceId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_WorkspaceId",
                table: "Expenses");
        }
    }
}
