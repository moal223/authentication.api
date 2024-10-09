using System.ComponentModel.DataAnnotations;

namespace gp_backend.Core.Models
{
    public class Wound
{
    public int Id { get; set; }
    public List<Disease>? Disease { get; set; } = [];
    public FileDescription? Image { get; set; }
    public DateTime UploadDate { get; set; }
    public ApplicationUser User { get; set; }
    public string ApplicationUserId { get; set; }
}
}
