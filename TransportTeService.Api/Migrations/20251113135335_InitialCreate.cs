using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportTeService.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoTransport = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NoBESC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NoConnaissement = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Etat = table.Column<int>(type: "int", nullable: false),
                    TransitaireId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransporteurMaritime = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Consignee = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notifier = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ManifesteArrivee = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IdentiqueCommande = table.Column<bool>(type: "bit", nullable: false),
                    ManifesteDepart = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NoVoyage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NavireNom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ETA = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ATA = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Escale = table.Column<int>(type: "int", nullable: false),
                    PosteAttribue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateApurement = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoAVE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Module = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Assurance = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TLUPE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BAD_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BAD_Ref = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NoADV = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NotifierSMS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MessageErreur = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreeLe = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreePar = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ModifieLe = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiePar = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LignesTransports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoLigne = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LigneCommande = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LivraisonPartielle = table.Column<bool>(type: "bit", nullable: false),
                    Declaration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreeLe = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreePar = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ModifieLe = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiePar = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LignesTransports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LignesTransports_Transports_TransportId",
                        column: x => x.TransportId,
                        principalTable: "Transports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransportsEval",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Partenaire = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Incoterm = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Devise = table.Column<int>(type: "int", nullable: true),
                    ModeConditionnement = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Transporteur = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Expediteur = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Consignee = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notifier = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Commentaires = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FretDevise = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FretFCFA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AssuranceDevise = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AssuranceFCFA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AutresChargesDevise = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AutresChargesFCFA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MasseBrute = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValeurFCFA = table.Column<long>(type: "bigint", nullable: true),
                    ValeurDevise = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ModeTransport = table.Column<int>(type: "int", nullable: true),
                    PaysDestination = table.Column<int>(type: "int", nullable: true),
                    DestinationFinale = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UniteDeChargement = table.Column<int>(type: "int", nullable: true),
                    NbreUnite = table.Column<int>(type: "int", nullable: true),
                    BureauDedouanement = table.Column<int>(type: "int", nullable: true),
                    Assurance = table.Column<int>(type: "int", nullable: true),
                    CreeLe = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreePar = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ModifieLe = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiePar = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportsEval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportsEval_Transports_TransportId",
                        column: x => x.TransportId,
                        principalTable: "Transports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LignesTransportsEval",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LigneTransportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Partenaire = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PositionTarifaire = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Marque = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Colisage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MasseBrute = table.Column<decimal>(type: "decimal(13,2)", nullable: true),
                    MasseNette = table.Column<decimal>(type: "decimal(13,2)", nullable: true),
                    Volume = table.Column<decimal>(type: "decimal(15,3)", nullable: true),
                    Quantite = table.Column<decimal>(type: "decimal(15,3)", nullable: true),
                    Devise = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PrixUnitaire = table.Column<decimal>(type: "decimal(18,5)", nullable: true),
                    ValeurDevise = table.Column<decimal>(type: "decimal(18,5)", nullable: true),
                    UniteStatistique = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModeConditionnement = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Nombre = table.Column<int>(type: "int", nullable: true),
                    NumerosCollis = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MarquesCollis = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Commentaires = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PaysOrigine = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ValeurFCFA = table.Column<long>(type: "bigint", nullable: true),
                    CreeLe = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreePar = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ModifieLe = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiePar = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LignesTransportsEval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LignesTransportsEval_LignesTransports_LigneTransportId",
                        column: x => x.LigneTransportId,
                        principalTable: "LignesTransports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LignesTransports_TransportId",
                table: "LignesTransports",
                column: "TransportId");

            migrationBuilder.CreateIndex(
                name: "IX_LignesTransportsEval_LigneTransportId",
                table: "LignesTransportsEval",
                column: "LigneTransportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transports_NoTransport",
                table: "Transports",
                column: "NoTransport",
                unique: true,
                filter: "[NoTransport] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransportsEval_TransportId",
                table: "TransportsEval",
                column: "TransportId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LignesTransportsEval");

            migrationBuilder.DropTable(
                name: "TransportsEval");

            migrationBuilder.DropTable(
                name: "LignesTransports");

            migrationBuilder.DropTable(
                name: "Transports");
        }
    }
}
