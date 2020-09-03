using System.ComponentModel.DataAnnotations;

namespace DownloadAutoMover.Classes
{
    public class MediaType
    {
        [Key]
        public int MedId { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public int Type { get; set; }
    }
}
