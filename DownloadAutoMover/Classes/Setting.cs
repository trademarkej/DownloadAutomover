using System.ComponentModel.DataAnnotations;

namespace DownloadAutoMover.Classes
{
    public class Setting
    {
        [Key]
        public int SetId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Value { get; set; }
    }
}