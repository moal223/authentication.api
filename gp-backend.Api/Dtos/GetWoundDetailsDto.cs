using gp_backend.Core.Models;

namespace gp_backend.Api.Dtos
{
    public class GetWoundDetailsDto
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? Location { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Preventions { get; set; }
        public byte[] Image { get; set; }
        public DateTime UploadDate { get; set; }
        public string? Advice { get; set; }
        public string Risk { get; set; }
    }
}
