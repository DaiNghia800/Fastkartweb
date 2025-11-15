using Fastkart.Controllers.Admin;
using Fastkart.Models.Entities;

namespace Fastkart.Services.IServices
{
    // Sử dụng các DTO đã được định nghĩa trong namespace Controllers.Admin
    using Fastkart.Controllers.Admin;

    public interface IBlogService
    {
        // === Public BlogController ===
        Task<object> GetBlogsPublicAsync();
        Task<object> GetBlogDetailPublicAsync(int id);
        Task<object> GetCategoriesPublicAsync();

        // === Admin BlogCategoriesController ===
        Task<IEnumerable<BlogCategories>> GetCategoriesAdminAsync();
        Task<BlogCategories> GetCategoryAdminAsync(int id);
        Task<BlogCategories> CreateCategoryAsync(CreateBlogCategoryDto dto);
        Task<BlogCategories> UpdateCategoryAsync(int id, CreateBlogCategoryDto dto);
        Task<bool> DeleteCategoryAsync(int id); // Trả về true nếu thành công, false/exception nếu thất bại

        // === Admin BlogAdminController ===
        Task<IEnumerable<BlogPostDto>> GetBlogsAdminAsync();
        Task<BlogPostDto> GetBlogAdminAsync(int id);
        Task<BlogPosts> CreateBlogAsync(CreateBlogDto dto, int authorId);
        Task<BlogPosts> UpdateBlogAsync(int id, CreateBlogDto dto);
        Task<bool> DeleteBlogAsync(int id);
        Task<IEnumerable<UserDto>> GetBlogAuthorsAsync();
        Task<UserDto> GetCurrentUserAsync(int id);
    }
}