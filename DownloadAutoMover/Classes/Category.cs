using System.ComponentModel.DataAnnotations;

namespace DownloadAutoMover.Classes
{
    public class Category
    {
        [Key]
        public int CatId { get; set; }
        [Required]
        public string Value { get; set; }
    }
}