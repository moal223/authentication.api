using System.ComponentModel.DataAnnotations;

namespace gp_backend.Api.Dtos.Disease
{
    public class AddDiseaseDto
    {
        [StringLength(150)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Risk { get; set; }
        public List<string> Preventions { get; set; }
        public int SpecializationId { get; set; }
    }
}
