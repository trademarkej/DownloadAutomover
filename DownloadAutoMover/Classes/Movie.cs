using System.ComponentModel.DataAnnotations;

namespace DownloadAutoMover.Classes
{
    public class Movie
    {
        [Required]
        public string Group { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Resolution { get; set; }
        [Required]
        public string Codec { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string Audio { get; set; }
        [Required]
        public string Quality { get; set; }
        [Required]
        public int Category { get; set; }
        public string Path { get; set; }
    }
}
