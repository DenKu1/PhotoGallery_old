using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.WEB.Models.In
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Please enter username")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 50 characters")]
        public string Password { get; set; }
    }
}