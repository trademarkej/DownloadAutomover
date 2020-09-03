using System.ComponentModel.DataAnnotations;

namespace DownloadAutoMover.Classes
{
    public class TabRedirects
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string RedItem { get; set; }

        [Required]
        public string SubFolder { get; set; }
    }
}
