using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOTNprojekt.Models
{
    public class User
    {
        // Class used to create the database table of users
        [Key]
        public int User_Id { get; set; }
        [Required]
        [DisplayName("User Name")]
        public string User_Name { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Error, Password must be at least 3 characters long")]
        [MaxLength(25, ErrorMessage = "Error, Password can't be longer then 25 characters")]
        public string Password { get; set; }
        [DisplayName("E-mail")]
        public string E_Mail { get; set; } = "";
        public int n_Uploads { get; set; }
    }
}
