using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies_Alexandra_marian.Migrations
{
    public partial class Migrare2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Distribution",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistributionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distribution", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DistributedMovie",
                columns: table => new
                {
                    DistributionID = table.Column<int>(type: "int", nullable: false),
                    MovieID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributedMovie", x => new { x.MovieID, x.DistributionID });
                    table.ForeignKey(
                        name: "FK_DistributedMovie_Distribution_DistributionID",
                        column: x => x.DistributionID,
                        principalTable: "Distribution",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistributedMovie_Movie_MovieID",
                        column: x => x.MovieID,
                        principalTable: "Movie",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistributedMovie_DistributionID",
                table: "DistributedMovie",
                column: "DistributionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributedMovie");

            migrationBuilder.DropTable(
                name: "Distribution");
        }
    }
}
