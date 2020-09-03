using System.ComponentModel.DataAnnotations;

namespace DownloadAutoMover.Classes
{
    public class RenameItem
    {
        [Key]
        public int RenId { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public string Rename { get; set; }
    }
}
