using LearningPlatform.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace LearningPlatform.Models
{
    public class UserCourse
    {
        [Key]
        public int UserCourseId { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime PurchaseDate { get; set; }
        public bool HasAccess { get; set; }
    }
}
