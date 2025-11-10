using Fastkart.Models.EF;
using Fastkart.Models.Entities;

namespace Fastkart.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Roles> GetAllRole()
        {
            try
            {
                return _context.Roles.Where(p => !p.Deleted).ToList();
                
            }
            catch (Exception ex)
            {
                return new List<Roles>();
            }
        }

        public void CreateRole(Roles roles)
        {
            try
            {
                _context.Roles.Add(roles);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Roles GetRole(int id)
        {
            try
            {
                return _context.Roles.SingleOrDefault(p => p.Uid == id && !p.Deleted);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void EditRole(Roles roles, int id) {
            try
            {
                var existRole = _context.Roles.SingleOrDefault(p => p.Uid == id && !p.Deleted);

                if (existRole != null)
                {
                    existRole.RoleName = roles.RoleName;
                    existRole.UpdatedAt = DateTime.Now;
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DeleteRole(int id)
        {
            try
            {
                var existRole = _context.Roles.SingleOrDefault(p => p.Uid == id && !p.Deleted);

                if (existRole != null)
                {
                    existRole.Deleted = true;
                    existRole.UpdatedAt = DateTime.Now;
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
