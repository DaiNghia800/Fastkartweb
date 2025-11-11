using CloudinaryDotNet.Actions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Fastkart.Models.Entities
{
    public class Permission
    {
        public int Uid { get; set; }
        public int RoleId { get; set; }
        public int FunctionId { get; set; }
        public int PermissionTypeId { get; set; }
        public bool Allowed { get; set; }
        public Roles Role { get; set; }
        public Function Function { get; set; }
        public PermissionType PermissionType { get; set; }
    }
}
