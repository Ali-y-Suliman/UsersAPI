namespace Users.Dtos.User
{
    public class ChangePasswordDto
    {
        public string firstName { get; set; } = "";
        public string lastName { get; set; } = "";
        public string email { get; set; } = "";
        public string oldPassword { get; set; } = "";
        public string newPassword { get; set; } = "";
        public string token { get; set; } = "";
    }
}
