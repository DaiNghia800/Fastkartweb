using System;
using System.Data;

namespace Fastkart.Models.Entities
{
    public class Pages
    {
        public int Uid { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public string Status { get; set; }
        public DateTime? PublishedAt { get; set; }
        public int AuthorUid { get; set; }
        public Users Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
    }
}