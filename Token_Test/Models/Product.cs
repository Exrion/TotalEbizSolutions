using Login_Test.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Token_Test.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [MaxLength(500)]
        public string ProductDescription { get; set; }
        [Precision(2)]
        public double ProductPrice { get; set; }

        //Relationships
        public User User { get; set; }
    }
}
