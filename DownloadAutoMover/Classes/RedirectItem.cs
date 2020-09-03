using System.ComponentModel.DataAnnotations;

namespace DownloadAutoMover.Classes
{
    public class RedirectItem
    {
        [Key]
        public int RedId { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public int Type { get; set; }
    }
}
