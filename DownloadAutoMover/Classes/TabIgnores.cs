using System.ComponentModel.DataAnnotations;

namespace DownloadAutoMover.Classes
{
    public class TabIgnores
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Ignores { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
