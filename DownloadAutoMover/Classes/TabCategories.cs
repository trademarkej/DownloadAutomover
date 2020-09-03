using System.ComponentModel.DataAnnotations;

namespace DownloadAutoMover.Classes
{
    public class TabCategories
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Category { get; set; }

        [Required]
        public string SubFolder { get; set; }
    }
}
