using Fastkart.Models.EF;
using Fastkart.Models.Entities;
using Fastkart.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

        public void UpdatePermission(JsonElement data)
        {
            try
            {
                foreach (var role in data.EnumerateArray())
                {
                    int roleId = int.Parse(role.GetProperty("id").GetString());

                    var oldPermission = _context.Permissions.Where(p => p.RoleId == roleId);
                    _context.RemoveRange(oldPermission);

                    var permissions = role.GetProperty("permission").EnumerateArray();
                    foreach (var permission in permissions)
                    {
                        string permissionCode = permission.GetString();
                        string functionCode = permissionCode.Split('_')[0];
                        string permissionTypeCode = permissionCode.Split('_')[1];

                        var function = _context.Functions.FirstOrDefault(p => p.Code == functionCode);
                        if (function == null) continue;

                        var permissionType = _context.PermissionTypes.FirstOrDefault(p => p.Code == permissionTypeCode);
                        if (permissionType == null) continue;

                        _context.Permissions.Add(new Permission
                        {
                            RoleId = roleId,
                            PermissionTypeId = permissionType.Id,
                            FunctionId = function.Uid,
                            Allowed = true
                        });
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<object> GetPermissions()
        {
            try
            {
                return _context.Roles
                    .Where(r => !r.Deleted)
                    .Include(r => r.Permissions)             
                    .ThenInclude(p => p.Function)       
                    .Include(r => r.Permissions)
                    .ThenInclude(p => p.PermissionType)
                    .Select(r => new
                    {
                        id = r.Uid,
                        permissions = r.Permissions
                        .Where(p => p.Allowed)             
                        .Select(p => p.Function.Code + "_" + p.PermissionType.Code)
                        .ToList()
                    })
                    .ToList(); 
            } catch(Exception ex)
            {
                return new List<object>();
            }
        }
    }
}
