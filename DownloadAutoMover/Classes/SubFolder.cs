using System.ComponentModel.DataAnnotations;

namespace DownloadAutoMover.Classes
{
    public class SubFolder
    {
        [Key]
        public int SubId { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
