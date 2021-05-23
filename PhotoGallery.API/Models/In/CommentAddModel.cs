using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.API.Models.In
{
    public class CommentAddModel
    {
        [Required]
        [MaxLength(300, ErrorMessage = "Comment must be less than 300 symbols")]
        public string Text { get; set; }
    }
}
