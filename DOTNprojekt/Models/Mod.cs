using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DOTNprojekt.Models
{
    public class Mod
    {
        // Class used to create the database table for mods
        [Key]
        public int Mod_Id { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "String cannot be longer then 30 characters, or shorter then 5", MinimumLength = 5)]
        [DisplayName("Mod Name")]
        public string Mod_Name { get; set; }
        [Required]
        [DisplayName("Mod Character")]
        public string Mod_Char { get; set; }
        [Required]
        [DisplayName("Mod Category")]
        public string Mod_cat { get; set; }
        [Required]
        public User Uploader { get; set; }
        public DateTime Upload_Date { get; set; } = DateTime.Now;
        public int n_Views { get; set; } = 0;
        public int n_Downloads { get; set; } = 0;

    }
}
