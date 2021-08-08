using System.ComponentModel.DataAnnotations;

namespace JwtToken.Data
{
    public class UserModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}