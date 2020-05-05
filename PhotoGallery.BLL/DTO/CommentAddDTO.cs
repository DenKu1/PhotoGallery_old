using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.BLL.DTO
{
    public class CommentAddDTO
    {
        [Required]
        public int PhotoId { get; set; }

        [Required]
        [MaxLength(300, ErrorMessage = "Comment must be less than 300 symbols")]
        [RegularExpression(@"^(?!\s*$).+$", ErrorMessage = "Comment cannot be blank")]
        public string Text { get; set; }
    }
}
