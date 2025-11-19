using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportTeService.Api.Migrations
{
    /// <inheritdoc />
    public partial class RestoreGuidsWithAutoGeneration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Supprimer les données existantes car on change le type d'ID de int à Guid
            migrationBuilder.Sql("DELETE FROM [LignesTransportsEval]");
            migrationBuilder.Sql("DELETE FROM [LignesTransports]");
            migrationBuilder.Sql("DELETE FROM [TransportsEval]");
            migrationBuilder.Sql("DELETE FROM [Transports]");

            // Supprimer les index uniques et autres index (s'ils existent)
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_TransportsEval_TransportId' AND object_id = OBJECT_ID('TransportsEval'))
                    DROP INDEX [IX_TransportsEval_TransportId] ON [TransportsEval];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LignesTransportsEval_LigneTransportId' AND object_id = OBJECT_ID('LignesTransportsEval'))
                    DROP INDEX [IX_LignesTransportsEval_LigneTransportId] ON [LignesTransportsEval];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LignesTransports_TransportId' AND object_id = OBJECT_ID('LignesTransports'))
                    DROP INDEX [IX_LignesTransports_TransportId] ON [LignesTransports];
            ");

            // Supprimer les contraintes de clé étrangère (si elles existent)
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_TransportsEval_Transports_TransportId')
                    ALTER TABLE [TransportsEval] DROP CONSTRAINT [FK_TransportsEval_Transports_TransportId];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_LignesTransports_Transports_TransportId')
                    ALTER TABLE [LignesTransports] DROP CONSTRAINT [FK_LignesTransports_Transports_TransportId];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_LignesTransportsEval_LignesTransports_LigneTransportId')
                    ALTER TABLE [LignesTransportsEval] DROP CONSTRAINT [FK_LignesTransportsEval_LignesTransports_LigneTransportId];
            ");

            // Supprimer les contraintes de clé primaire (si elles existent)
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_TransportsEval')
                    ALTER TABLE [TransportsEval] DROP CONSTRAINT [PK_TransportsEval];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_Transports')
                    ALTER TABLE [Transports] DROP CONSTRAINT [PK_Transports];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_LignesTransportsEval')
                    ALTER TABLE [LignesTransportsEval] DROP CONSTRAINT [PK_LignesTransportsEval];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_LignesTransports')
                    ALTER TABLE [LignesTransports] DROP CONSTRAINT [PK_LignesTransports];
            ");

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
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "TransportsEval",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.AddColumn<Guid>(
                name: "TransportId",
                table: "TransportsEval",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "LignesTransports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.AddColumn<Guid>(
                name: "TransportId",
                table: "LignesTransports",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "LignesTransportsEval",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

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

            // Recréer les colonnes ID avec int
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
                nullable: false);

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
                nullable: false);

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
