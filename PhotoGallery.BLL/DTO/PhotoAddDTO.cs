using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.BLL.DTO
{
    public class PhotoAddDTO
    {
        [Required]
        public int AlbumId { get; set; }

        [Required(ErrorMessage = "Name should be specified")]
        [MaxLength(50, ErrorMessage = "Photo name should be less than 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Path should be specified")]
        [MaxLength(200, ErrorMessage = "Photo path should be less than 200 characters")]
        public string Path { get; set; }
    }
}
