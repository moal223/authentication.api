using gp_backend.Api.Dtos.Disease;

namespace gp_backend.Api.Dtos.Special
{
    public class GetSpecialDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GetDiseaseDetailsDto>? Diseases { get; set; } = [];
    }
}
