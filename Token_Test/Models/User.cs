using System.ComponentModel.DataAnnotations;
using Token_Test.Models;

namespace Login_Test.Models
{
    public class User
    {
        [Key]
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

        //Relationships
        public List<Product> Products { get; set; }
    }
}
