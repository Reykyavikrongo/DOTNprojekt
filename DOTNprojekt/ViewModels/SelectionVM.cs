using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOTNprojekt.ViewModels
{
    // Class used to contain data that is selected on the "Search" tab
    public class SelectionVM
    {
        public string selectedCharacter { get; set; }

        public string selectedCategory { get; set; }
    }
}