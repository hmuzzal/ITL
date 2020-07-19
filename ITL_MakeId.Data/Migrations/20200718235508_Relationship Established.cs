using Microsoft.EntityFrameworkCore.Migrations;

namespace ITL_MakeId.Data.Migrations
{
    public partial class RelationshipEstablished : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Designation",
                table: "IdentityCards");

            migrationBuilder.AddColumn<int>(
                name: "DesignationId",
                table: "IdentityCards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityCards_DesignationId",
                table: "IdentityCards",
                column: "DesignationId");

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityCards_Designations_DesignationId",
                table: "IdentityCards",
                column: "DesignationId",
                principalTable: "Designations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityCards_Designations_DesignationId",
                table: "IdentityCards");

            migrationBuilder.DropIndex(
                name: "IX_IdentityCards_DesignationId",
                table: "IdentityCards");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "IdentityCards");

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "IdentityCards",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
