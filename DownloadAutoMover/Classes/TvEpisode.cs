using System.ComponentModel.DataAnnotations;

namespace DownloadAutoMover.Classes
{
    public class TvEpisode
    {
        [Key]
        public string Episode { get; set; }
        [Required]
        public string Season { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Codec { get; set; }
        [Required]
        public string Group { get; set; }
        [Required]
        public string Quality { get; set; }
    }
}
