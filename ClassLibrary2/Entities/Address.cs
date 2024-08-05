using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary2.Entities
{
    public class Address
    {

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AddressId { get; set; } 


        [Required]
        [MaxLength(100, ErrorMessage = "The length of the Address name should not exceed 100 characters.")]
        public string name { get; set; }

        [Required]

        public string city { get; set; }

        [Required]

        public string state { get; set; }

        [Required]

        public int zipcode { get; set; }

        [Required]

        public string country { get; set; }

        //public virtual ICollection<AppUsers> AppUsers { get; set; } // Navigation property







    }
}