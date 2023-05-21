using System.ComponentModel.DataAnnotations;

namespace Users.Dtos.User
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string firstName { get; set; } = "";

        [Required(ErrorMessage = "LastName is required")]
        public string lastName { get; set; } = "";

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; } = "";

        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; } = "";

        [Required(ErrorMessage = "confirmedPassword is required")]
        public string confirmedPassword { get; set; } = "";
    }
}
