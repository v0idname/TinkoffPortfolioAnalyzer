using Microsoft.EntityFrameworkCore.Migrations;

namespace TinkoffPortfolioAnalyzer.Migrations
{
    public partial class secSnapManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityInfo_Snapshots_AvailSecSnapshotId",
                table: "SecurityInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SecurityInfo",
                table: "SecurityInfo");

            migrationBuilder.DropIndex(
                name: "IX_SecurityInfo_AvailSecSnapshotId",
                table: "SecurityInfo");

            migrationBuilder.DropColumn(
                name: "AvailSecSnapshotId",
                table: "SecurityInfo");

            migrationBuilder.RenameTable(
                name: "SecurityInfo",
                newName: "AvailSecurities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AvailSecurities",
                table: "AvailSecurities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AvailSecSnapshotSecurityInfo",
                columns: table => new
                {
                    SecuritiesId = table.Column<int>(type: "int", nullable: false),
                    SnapshotsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailSecSnapshotSecurityInfo", x => new { x.SecuritiesId, x.SnapshotsId });
                    table.ForeignKey(
                        name: "FK_AvailSecSnapshotSecurityInfo_AvailSecurities_SecuritiesId",
                        column: x => x.SecuritiesId,
                        principalTable: "AvailSecurities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvailSecSnapshotSecurityInfo_Snapshots_SnapshotsId",
                        column: x => x.SnapshotsId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvailSecSnapshotSecurityInfo_SnapshotsId",
                table: "AvailSecSnapshotSecurityInfo",
                column: "SnapshotsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailSecSnapshotSecurityInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AvailSecurities",
                table: "AvailSecurities");

            migrationBuilder.RenameTable(
                name: "AvailSecurities",
                newName: "SecurityInfo");

            migrationBuilder.AddColumn<int>(
                name: "AvailSecSnapshotId",
                table: "SecurityInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SecurityInfo",
                table: "SecurityInfo",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityInfo_AvailSecSnapshotId",
                table: "SecurityInfo",
                column: "AvailSecSnapshotId");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityInfo_Snapshots_AvailSecSnapshotId",
                table: "SecurityInfo",
                column: "AvailSecSnapshotId",
                principalTable: "Snapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
