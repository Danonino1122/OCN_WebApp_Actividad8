using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OCN_WebApp.Migrations;

/// <inheritdoc />
public partial class _20261117170356_InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Problematicas",
            columns: table => new
            {
                ProblematicaId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                NombreProblematica = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Problematicas", x => x.ProblematicaId);
            });

        migrationBuilder.CreateTable(
            name: "Zonas",
            columns: table => new
            {
                ZonaId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                NombreZona = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Zonas", x => x.ZonaId);
            });

        migrationBuilder.CreateTable(
            name: "RegistrosOCN",
            columns: table => new
            {
                RegistroOCNId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ZonaId = table.Column<int>(type: "int", nullable: false),
                ProblematicaId = table.Column<int>(type: "int", nullable: false),
                Intensidad = table.Column<int>(type: "int", nullable: false),
                Frecuencia = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RegistrosOCN", x => x.RegistroOCNId);
                table.ForeignKey(
                    name: "FK_RegistrosOCN_Problematicas_ProblematicaId",
                    column: x => x.ProblematicaId,
                    principalTable: "Problematicas",
                    principalColumn: "ProblematicaId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RegistrosOCN_Zonas_ZonaId",
                    column: x => x.ZonaId,
                    principalTable: "Zonas",
                    principalColumn: "ZonaId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_RegistrosOCN_ProblematicaId",
            table: "RegistrosOCN",
            column: "ProblematicaId");

        migrationBuilder.CreateIndex(
            name: "IX_RegistrosOCN_ZonaId",
            table: "RegistrosOCN",
            column: "ZonaId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "RegistrosOCN");

        migrationBuilder.DropTable(
            name: "Problematicas");

        migrationBuilder.DropTable(
            name: "Zonas");
    }
}
