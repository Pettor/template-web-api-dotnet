﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Tenant;

public partial class InitialMigrations : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(name: "MultiTenancy");

        migrationBuilder.CreateTable(
            name: "Tenants",
            schema: "MultiTenancy",
            columns: table => new
            {
                Id = table.Column<string>(
                    type: "character varying(64)",
                    maxLength: 64,
                    nullable: false
                ),
                Identifier = table.Column<string>(type: "text", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                ConnectionString = table.Column<string>(type: "text", nullable: false),
                AdminEmail = table.Column<string>(type: "text", nullable: false),
                IsActive = table.Column<bool>(type: "boolean", nullable: false),
                ValidUpto = table.Column<DateTime>(
                    type: "timestamp without time zone",
                    nullable: false
                ),
                Issuer = table.Column<string>(type: "text", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tenants", x => x.Id);
            }
        );

        migrationBuilder.CreateIndex(
            name: "IX_Tenants_Identifier",
            schema: "MultiTenancy",
            table: "Tenants",
            column: "Identifier",
            unique: true
        );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Tenants", schema: "MultiTenancy");
    }
}
