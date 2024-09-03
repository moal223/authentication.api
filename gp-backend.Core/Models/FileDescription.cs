using System.ComponentModel.DataAnnotations;

namespace gp_backend.Core.Models
{
    public class FileDescription
    {
        [Key]
        public int Id { get; set; }
        public string? ContentDisposition { get; set; }
        public string? ContentType { get; set; }
        public FileContent? Content { get; set; }
    }
}
