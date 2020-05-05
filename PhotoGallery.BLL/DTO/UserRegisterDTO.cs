using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.BLL.DTO
{
    public class UserRegisterDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}