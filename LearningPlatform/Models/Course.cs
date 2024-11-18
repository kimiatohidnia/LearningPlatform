using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(200)]
        [DisplayName("Image")]
        public string ImageUrl { get; set; }

        [StringLength(100)]
        public string Teacher { get; set; }

        public bool IsFree { get; set; } = true;

        [PriceValidation]
        public decimal? Price { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]

        [StringLength(50)]
        public string? KeyWord { get; set; }

        public Category? Category { get; set; }

        public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}
