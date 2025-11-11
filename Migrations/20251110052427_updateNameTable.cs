using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fastkart.Migrations
{
    /// <inheritdoc />
    public partial class updateNameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Function_FunctionId",
                table: "Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_Permission_PermissionType_PermissionTypeId",
                table: "Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Roles_RoleId",
                table: "Permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PermissionType",
                table: "PermissionType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permission",
                table: "Permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Function",
                table: "Function");

            migrationBuilder.RenameTable(
                name: "PermissionType",
                newName: "PermissionTypes");

            migrationBuilder.RenameTable(
                name: "Permission",
                newName: "Permissions");

            migrationBuilder.RenameTable(
                name: "Function",
                newName: "Functions");

            migrationBuilder.RenameIndex(
                name: "IX_Permission_RoleId",
                table: "Permissions",
                newName: "IX_Permissions_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Permission_PermissionTypeId",
                table: "Permissions",
                newName: "IX_Permissions_PermissionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Permission_FunctionId",
                table: "Permissions",
                newName: "IX_Permissions_FunctionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermissionTypes",
                table: "PermissionTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Functions",
                table: "Functions",
                column: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Functions_FunctionId",
                table: "Permissions",
                column: "FunctionId",
                principalTable: "Functions",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_PermissionTypes_PermissionTypeId",
                table: "Permissions",
                column: "PermissionTypeId",
                principalTable: "PermissionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Roles_RoleId",
                table: "Permissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Functions_FunctionId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_PermissionTypes_PermissionTypeId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Roles_RoleId",
                table: "Permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PermissionTypes",
                table: "PermissionTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Functions",
                table: "Functions");

            migrationBuilder.RenameTable(
                name: "PermissionTypes",
                newName: "PermissionType");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "Permission");

            migrationBuilder.RenameTable(
                name: "Functions",
                newName: "Function");

            migrationBuilder.RenameIndex(
                name: "IX_Permissions_RoleId",
                table: "Permission",
                newName: "IX_Permission_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Permissions_PermissionTypeId",
                table: "Permission",
                newName: "IX_Permission_PermissionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Permissions_FunctionId",
                table: "Permission",
                newName: "IX_Permission_FunctionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermissionType",
                table: "PermissionType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permission",
                table: "Permission",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Function",
                table: "Function",
                column: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Function_FunctionId",
                table: "Permission",
                column: "FunctionId",
                principalTable: "Function",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_PermissionType_PermissionTypeId",
                table: "Permission",
                column: "PermissionTypeId",
                principalTable: "PermissionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Roles_RoleId",
                table: "Permission",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
