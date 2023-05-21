using System.ComponentModel.DataAnnotations;

namespace Users.Dtos.User
{
    public class SignInDto
    {
        public string firstName { get; set; } = "";
        public string lastName { get; set; } = "";
        public string email { get; set; } = "";
        public string password { get; set; } = "";
        public string token { get; set; } = "";
    }

    public class SignInResponseDto
    {
        public string firstName { get; set; } = "";
        public string lastName { get; set; } = "";
        public string email { get; set; } = "";
        public string token { get; set; } = "";
    }

    public class SignInRquestDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; } = "";

        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; } = "";
    }
}
