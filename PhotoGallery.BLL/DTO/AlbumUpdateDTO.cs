using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.BLL.DTO
{
    public class AlbumUpdateDTO
    {
        [Required(ErrorMessage = "Album id should be specified")]
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "Name should be less than 50 characters")]
        public string Name { get; set; }

        [MaxLength(200, ErrorMessage = "Name should be less than 200 characters")]
        public string Description { get; set; }
    }
}
