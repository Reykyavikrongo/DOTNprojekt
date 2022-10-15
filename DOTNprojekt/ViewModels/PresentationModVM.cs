namespace DOTNprojekt.ViewModels
{
    public class PresentationModVM
    {
        // Class used to send data to the view that shows the chosen mods through the search option
        public string Name { get; set; }
        public string ZipName { get; set; }
        public bool image { get; set; }
        public string UploaderName { get; set; }
        public int Downloads { get; set; }
    }
}
