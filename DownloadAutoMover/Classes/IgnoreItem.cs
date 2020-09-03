using System.ComponentModel.DataAnnotations;

namespace DownloadAutoMover.Classes
{
    public class IgnoreItem
    {
        [Key]
        public int IgnrId { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public int Type { get; set; }
    }
}
