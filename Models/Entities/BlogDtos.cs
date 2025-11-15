namespace Fastkart.Models.Entities
{
    public class CreateBlogCategoryDto
    {
        public string Name { get; set; }
    }

    public class BlogPostDto
    {
        public int Uid { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int AuthorUid { get; set; }
        public string AuthorName { get; set; }
        public int CategoryUid { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateBlogDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryUid { get; set; }
        public string ImageUrl { get; set; }
    }

    public class UserDto
    {
        public int Uid { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
