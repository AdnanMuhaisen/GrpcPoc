﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Api.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class AddOrderQuantity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "Quantity",
            table: "Orders",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Quantity",
            table: "Orders");
    }
}