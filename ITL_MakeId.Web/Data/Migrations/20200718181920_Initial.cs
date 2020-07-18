using Microsoft.EntityFrameworkCore.Migrations;

namespace ITL_MakeId.Web.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Designation = table.Column<string>(nullable: true),
                    BloodGroup = table.Column<string>(nullable: true),
                    CardNumber = table.Column<string>(nullable: true),
                    ImagePathOfUser = table.Column<string>(nullable: true),
                    ImagePathOfUserSignature = table.Column<string>(nullable: true),
                    ImagePathOfAuthorizedSignature = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    CompanyAddress = table.Column<string>(nullable: true),
                    CompanyLogoPath = table.Column<string>(nullable: true),
                    CardInfo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityCards", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityCards");
        }
    }
}
