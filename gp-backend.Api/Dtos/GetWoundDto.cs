using gp_backend.Core.Models;

namespace gp_backend.Api.Dtos
{
    public class GetWoundDto
    {
        public int Id { get; set; }
        public byte[] file { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
