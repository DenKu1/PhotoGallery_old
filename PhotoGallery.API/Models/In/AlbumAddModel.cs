using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.API.Models.In
{
    public class AlbumAddModel
    {
        [Required(ErrorMessage = "Name should be specified")]
        [MaxLength(50, ErrorMessage = "Name should be less than 50 characters")]
        public string Name { get; set; }

        [MaxLength(200, ErrorMessage = "Name should be less than 200 characters")]
        public string Description { get; set; }
    }
}
