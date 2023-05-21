using System.ComponentModel.DataAnnotations;

namespace Users.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string firstName { get; set; } = "";
        public string lastName { get; set; } = "";
        public string email { get; set; } = "";
        public byte[] passwordHash { get; set; } = new byte[0];
        public byte[] passwordSalt { get; set; } = new byte[0];
    }
}