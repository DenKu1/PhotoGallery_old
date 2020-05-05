using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.BLL.DTO
{
    public class UserLoginDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}