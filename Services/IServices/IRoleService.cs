using Fastkart.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Fastkart.Services.IServices
{
    public interface IRoleService
    {
        List<Roles> GetAllRole();
        List<Roles> GetRolePermission();
        void CreateRole(Roles role);
        Roles GetRole(int id);
        void EditRole(Roles role, int id);
        int DeleteRole(int id);
        void UpdatePermission(JsonElement data);
        IEnumerable<object> GetPermissions();
        bool checkRoleName(int id, string name);
    }
}
