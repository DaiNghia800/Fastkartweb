using Fastkart.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Fastkart.Services.IServices
{
    public interface IRoleService
    {
        List<Roles> GetAllRole();
        void CreateRole(Roles role);
        Roles GetRole(int id);
        void EditRole(Roles role, int id);
        void DeleteRole(int id);
        void UpdatePermission(JsonElement data);
        IEnumerable<object> GetPermissions();
    }
}
