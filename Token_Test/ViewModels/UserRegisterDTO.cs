using System.ComponentModel.DataAnnotations;

namespace Login_Test.ViewModels
{
    public class UserRegisterDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string GivenName { get; set; }
    }
}
