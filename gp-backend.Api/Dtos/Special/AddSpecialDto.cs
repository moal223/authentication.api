using System.ComponentModel.DataAnnotations;

namespace gp_backend.Api.Dtos.Special
{
    public class AddSpecialDto
    {
        [StringLength(150)]
        public string Name { get; set; }
    }
}
