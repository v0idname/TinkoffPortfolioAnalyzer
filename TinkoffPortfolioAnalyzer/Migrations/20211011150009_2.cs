using Microsoft.EntityFrameworkCore.Migrations;

namespace TinkoffPortfolioAnalyzer.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityInfo_Snapshots_SnapshotEntityId",
                table: "SecurityInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SecurityInfo",
                table: "SecurityInfo");

            migrationBuilder.RenameColumn(
                name: "SnapshotEntityId",
                table: "SecurityInfo",
                newName: "AvailSecSnapshotId");

            migrationBuilder.RenameIndex(
                name: "IX_SecurityInfo_SnapshotEntityId",
                table: "SecurityInfo",
                newName: "IX_SecurityInfo_AvailSecSnapshotId");

            migrationBuilder.AlterColumn<string>(
                name: "Ticker",
                table: "SecurityInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SecurityInfo",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SecurityInfo",
                table: "SecurityInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityInfo_Snapshots_AvailSecSnapshotId",
                table: "SecurityInfo",
                column: "AvailSecSnapshotId",
                principalTable: "Snapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityInfo_Snapshots_AvailSecSnapshotId",
                table: "SecurityInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SecurityInfo",
                table: "SecurityInfo");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SecurityInfo");

            migrationBuilder.RenameColumn(
                name: "AvailSecSnapshotId",
                table: "SecurityInfo",
                newName: "SnapshotEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_SecurityInfo_AvailSecSnapshotId",
                table: "SecurityInfo",
                newName: "IX_SecurityInfo_SnapshotEntityId");

            migrationBuilder.AlterColumn<string>(
                name: "Ticker",
                table: "SecurityInfo",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SecurityInfo",
                table: "SecurityInfo",
                column: "Ticker");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityInfo_Snapshots_SnapshotEntityId",
                table: "SecurityInfo",
                column: "SnapshotEntityId",
                principalTable: "Snapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
