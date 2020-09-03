using System.ComponentModel.DataAnnotations;

namespace DownloadAutoMover.Classes
{
    public class TabRenames
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Value { get; set; }

        [Required]
        public string Rename { get; set; }
    }
}
