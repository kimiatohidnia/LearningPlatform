using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models
{
    public class Chapter
    {
        [Key]
        public int ChapterId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [Url]
        public string VideoUrl { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]

        public Course? Course { get; set; }
    }
}
