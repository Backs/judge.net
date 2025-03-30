using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Judge.Data.Migrations
{
    /// <inheritdoc />
    public partial class contest_analysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmitResults_Submits_SubmitId",
                schema: "dbo",
                table: "SubmitResults");

            migrationBuilder.AlterColumn<long>(
                name: "SubmitId",
                schema: "dbo",
                table: "SubmitResults",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Analysis",
                table: "Contests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submits_ProblemId",
                schema: "dbo",
                table: "Submits",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_Submits_UserId",
                schema: "dbo",
                table: "Submits",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmitResults_Submits_SubmitId",
                schema: "dbo",
                table: "SubmitResults",
                column: "SubmitId",
                principalSchema: "dbo",
                principalTable: "Submits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Submits_Tasks_ProblemId",
                schema: "dbo",
                table: "Submits",
                column: "ProblemId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submits_Users_UserId",
                schema: "dbo",
                table: "Submits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmitResults_Submits_SubmitId",
                schema: "dbo",
                table: "SubmitResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Submits_Tasks_ProblemId",
                schema: "dbo",
                table: "Submits");

            migrationBuilder.DropForeignKey(
                name: "FK_Submits_Users_UserId",
                schema: "dbo",
                table: "Submits");

            migrationBuilder.DropIndex(
                name: "IX_Submits_ProblemId",
                schema: "dbo",
                table: "Submits");

            migrationBuilder.DropIndex(
                name: "IX_Submits_UserId",
                schema: "dbo",
                table: "Submits");

            migrationBuilder.DropColumn(
                name: "Analysis",
                table: "Contests");

            migrationBuilder.AlterColumn<long>(
                name: "SubmitId",
                schema: "dbo",
                table: "SubmitResults",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SubmitResults_Submits_SubmitId",
                schema: "dbo",
                table: "SubmitResults",
                column: "SubmitId",
                principalSchema: "dbo",
                principalTable: "Submits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
