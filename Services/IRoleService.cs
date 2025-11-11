using Fastkart.Models.Entities;

namespace Fastkart.Services
{
    public interface IRoleService
    {
        List<Roles> GetAllRole();
        void CreateRole(Roles role);
        Roles GetRole(int id);
        void EditRole(Roles role, int id);
        void DeleteRole(int id);
    }
}
