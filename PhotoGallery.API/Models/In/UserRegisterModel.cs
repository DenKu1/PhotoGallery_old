using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.API.Models.In
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Please enter username")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [StringLength(50, ErrorMessage = "Email must be less than 50 characters")]
        [EmailAddress(ErrorMessage = "Incorrect email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 50 characters")]
        public string Password { get; set; }
    }
}