using Fastkart.Models.Entities;
using Fastkart.Models.ViewModels;

namespace Fastkart.Services
{
    public interface IUserService
    {
        List<Roles> GetAllRoles();
        List<Users> GetAllUsers();
        Users GetUserById(int userId);
        Task<bool> UpdateUser(Users userToUpdate, IFormFile? imgFile);
        Task<bool> DeleteUser(int userId);
        Task<(bool Success, string ErrorMessage)> CreateUser(UserCreateViewModel model);

    }
}
