using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Judge.Data.Migrations
{
    public partial class datetime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmitDateUtc",
                schema: "dbo",
                table: "Submits",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateUtc",
                schema: "dbo",
                table: "CheckQueue",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateUtc",
                table: "Tasks",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.Sql(@"
CREATE PROCEDURE [dbo].[DequeueSubmitCheck]
AS
BEGIN

	;WITH CTE(SubmitResultId, CreationDateUtc)
	AS
	(
		SELECT TOP 1 *
		FROM dbo.CheckQueue cq WITH(UPDLOCK, READPAST)
		ORDER BY cq.CreationDateUtc
	)
	DELETE CTE
	OUTPUT
		DELETED.*
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmitDateUtc",
                schema: "dbo",
                table: "Submits",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateUtc",
                schema: "dbo",
                table: "CheckQueue",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateUtc",
                table: "Tasks",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.Sql("DROP PROCEDURE [dbo].[DequeueSubmitCheck]");
        }
    }
}
