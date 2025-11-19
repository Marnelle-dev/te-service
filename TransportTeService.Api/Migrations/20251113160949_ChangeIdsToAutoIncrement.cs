using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportTeService.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIdsToAutoIncrement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Supprimer les données existantes car on change le type d'ID de Guid à int
            migrationBuilder.Sql("DELETE FROM [LignesTransportsEval]");
            migrationBuilder.Sql("DELETE FROM [LignesTransports]");
            migrationBuilder.Sql("DELETE FROM [TransportsEval]");
            migrationBuilder.Sql("DELETE FROM [Transports]");

            // Supprimer les index uniques et autres index
            migrationBuilder.DropIndex(
                name: "IX_TransportsEval_TransportId",
                table: "TransportsEval");

            migrationBuilder.DropIndex(
                name: "IX_LignesTransportsEval_LigneTransportId",
                table: "LignesTransportsEval");

            migrationBuilder.DropIndex(
                name: "IX_LignesTransports_TransportId",
                table: "LignesTransports");

            // Supprimer les contraintes de clé étrangère
            migrationBuilder.DropForeignKey(
                name: "FK_TransportsEval_Transports_TransportId",
                table: "TransportsEval");

            migrationBuilder.DropForeignKey(
                name: "FK_LignesTransports_Transports_TransportId",
                table: "LignesTransports");

            migrationBuilder.DropForeignKey(
                name: "FK_LignesTransportsEval_LignesTransports_LigneTransportId",
                table: "LignesTransportsEval");

            // Supprimer les contraintes de clé primaire
            migrationBuilder.DropPrimaryKey(
                name: "PK_TransportsEval",
                table: "TransportsEval");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transports",
                table: "Transports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LignesTransportsEval",
                table: "LignesTransportsEval");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LignesTransports",
                table: "LignesTransports");

            // Supprimer les colonnes ID
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TransportsEval");

            migrationBuilder.DropColumn(
                name: "TransportId",
                table: "TransportsEval");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LignesTransports");

            migrationBuilder.DropColumn(
                name: "TransportId",
                table: "LignesTransports");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LignesTransportsEval");

            migrationBuilder.DropColumn(
                name: "LigneTransportId",
                table: "LignesTransportsEval");

            // Recréer les colonnes ID avec IDENTITY
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Transports",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TransportsEval",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "TransportId",
                table: "TransportsEval",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LignesTransports",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "TransportId",
                table: "LignesTransports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LignesTransportsEval",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "LigneTransportId",
                table: "LignesTransportsEval",
                type: "int",
                nullable: true);

            // Rendre les colonnes non-nullable (les tables sont vides maintenant)
            migrationBuilder.Sql("UPDATE [TransportsEval] SET [TransportId] = 0 WHERE [TransportId] IS NULL");
            migrationBuilder.Sql("UPDATE [LignesTransports] SET [TransportId] = 0 WHERE [TransportId] IS NULL");
            migrationBuilder.Sql("UPDATE [LignesTransportsEval] SET [LigneTransportId] = 0 WHERE [LigneTransportId] IS NULL");
            
            migrationBuilder.Sql("ALTER TABLE [TransportsEval] ALTER COLUMN [TransportId] int NOT NULL");
            migrationBuilder.Sql("ALTER TABLE [LignesTransports] ALTER COLUMN [TransportId] int NOT NULL");
            migrationBuilder.Sql("ALTER TABLE [LignesTransportsEval] ALTER COLUMN [LigneTransportId] int NOT NULL");

            // Recréer les contraintes de clé primaire
            migrationBuilder.AddPrimaryKey(
                name: "PK_Transports",
                table: "Transports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransportsEval",
                table: "TransportsEval",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LignesTransports",
                table: "LignesTransports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LignesTransportsEval",
                table: "LignesTransportsEval",
                column: "Id");

            // Recréer les contraintes de clé étrangère
            migrationBuilder.AddForeignKey(
                name: "FK_TransportsEval_Transports_TransportId",
                table: "TransportsEval",
                column: "TransportId",
                principalTable: "Transports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LignesTransports_Transports_TransportId",
                table: "LignesTransports",
                column: "TransportId",
                principalTable: "Transports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LignesTransportsEval_LignesTransports_LigneTransportId",
                table: "LignesTransportsEval",
                column: "LigneTransportId",
                principalTable: "LignesTransports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Supprimer les index uniques et autres index
            migrationBuilder.DropIndex(
                name: "IX_TransportsEval_TransportId",
                table: "TransportsEval");

            migrationBuilder.DropIndex(
                name: "IX_LignesTransportsEval_LigneTransportId",
                table: "LignesTransportsEval");

            migrationBuilder.DropIndex(
                name: "IX_LignesTransports_TransportId",
                table: "LignesTransports");

            // Supprimer les contraintes de clé étrangère
            migrationBuilder.DropForeignKey(
                name: "FK_TransportsEval_Transports_TransportId",
                table: "TransportsEval");

            migrationBuilder.DropForeignKey(
                name: "FK_LignesTransports_Transports_TransportId",
                table: "LignesTransports");

            migrationBuilder.DropForeignKey(
                name: "FK_LignesTransportsEval_LignesTransports_LigneTransportId",
                table: "LignesTransportsEval");

            // Supprimer les contraintes de clé primaire
            migrationBuilder.DropPrimaryKey(
                name: "PK_TransportsEval",
                table: "TransportsEval");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transports",
                table: "Transports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LignesTransportsEval",
                table: "LignesTransportsEval");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LignesTransports",
                table: "LignesTransports");

            // Supprimer les colonnes ID
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TransportsEval");

            migrationBuilder.DropColumn(
                name: "TransportId",
                table: "TransportsEval");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LignesTransports");

            migrationBuilder.DropColumn(
                name: "TransportId",
                table: "LignesTransports");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LignesTransportsEval");

            migrationBuilder.DropColumn(
                name: "LigneTransportId",
                table: "LignesTransportsEval");

            // Recréer les colonnes ID avec Guid
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Transports",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "TransportsEval",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TransportId",
                table: "TransportsEval",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "LignesTransports",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TransportId",
                table: "LignesTransports",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "LignesTransportsEval",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "LigneTransportId",
                table: "LignesTransportsEval",
                type: "uniqueidentifier",
                nullable: false);

            // Recréer les contraintes de clé primaire
            migrationBuilder.AddPrimaryKey(
                name: "PK_Transports",
                table: "Transports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransportsEval",
                table: "TransportsEval",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LignesTransports",
                table: "LignesTransports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LignesTransportsEval",
                table: "LignesTransportsEval",
                column: "Id");

            // Recréer les contraintes de clé étrangère
            migrationBuilder.AddForeignKey(
                name: "FK_TransportsEval_Transports_TransportId",
                table: "TransportsEval",
                column: "TransportId",
                principalTable: "Transports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LignesTransports_Transports_TransportId",
                table: "LignesTransports",
                column: "TransportId",
                principalTable: "Transports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LignesTransportsEval_LignesTransports_LigneTransportId",
                table: "LignesTransportsEval",
                column: "LigneTransportId",
                principalTable: "LignesTransports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // Recréer les index uniques et autres index
            migrationBuilder.CreateIndex(
                name: "IX_TransportsEval_TransportId",
                table: "TransportsEval",
                column: "TransportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LignesTransportsEval_LigneTransportId",
                table: "LignesTransportsEval",
                column: "LigneTransportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LignesTransports_TransportId",
                table: "LignesTransports",
                column: "TransportId");
        }
    }
}
