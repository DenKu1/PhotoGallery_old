using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.BLL.DTO
{
    public class AlbumUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "Name should be less than 50 characters")]
        [RegularExpression(@"^(?!\s*$).+$", ErrorMessage = "Name cannot be blank")]        
        public string Name { get; set; }

        [MaxLength(200, ErrorMessage = "Name should be less than 200 characters")]
        [RegularExpression(@"^(?!\s*$).+$", ErrorMessage = "Description cannot be blank")]
        public string Description { get; set; }
    }
}
