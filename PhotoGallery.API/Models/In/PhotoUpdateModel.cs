using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.API.Models.In
{
    public class PhotoUpdateModel
    {
        [Required(ErrorMessage = "Name should be specified")]
        [MaxLength(50, ErrorMessage = "Photo name should be less than 50 characters")]
        public string Name { get; set; }
    }
}
