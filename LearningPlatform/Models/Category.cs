using System.ComponentModel.DataAnnotations;

namespace LearningPlatform.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int? ParentId { get; set; }
        public Category? ParentCategory { get; set; }

        public ICollection<Category> Subcategories { get; set; } = new List<Category>();

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
