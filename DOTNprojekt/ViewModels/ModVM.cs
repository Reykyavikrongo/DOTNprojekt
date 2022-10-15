using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOTNprojekt.ViewModels
{
    public class ModVM
    {
        // Class used to interface between the "Upload" view and the functions in "ModController"
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
        [Required(ErrorMessage ="You must select a file")]
        public IFormFile Mod_File { get; set; }


    }
}