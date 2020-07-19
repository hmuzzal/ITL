using Microsoft.EntityFrameworkCore.Migrations;

namespace ITL_MakeId.Data.Migrations
{
    public partial class BloodGroupRelationshipAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BloodGroup",
                table: "IdentityCards");

            migrationBuilder.AddColumn<int>(
                name: "BloodGroupId",
                table: "IdentityCards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityCards_BloodGroupId",
                table: "IdentityCards",
                column: "BloodGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityCards_BloodGroups_BloodGroupId",
                table: "IdentityCards",
                column: "BloodGroupId",
                principalTable: "BloodGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityCards_BloodGroups_BloodGroupId",
                table: "IdentityCards");

            migrationBuilder.DropIndex(
                name: "IX_IdentityCards_BloodGroupId",
                table: "IdentityCards");

            migrationBuilder.DropColumn(
                name: "BloodGroupId",
                table: "IdentityCards");

            migrationBuilder.AddColumn<string>(
                name: "BloodGroup",
                table: "IdentityCards",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
