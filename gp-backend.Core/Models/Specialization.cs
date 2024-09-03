using System.ComponentModel.DataAnnotations;

namespace gp_backend.Core.Models
{
    public class Specialization
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        public int Years { get; set; }
        public List<ApplicationUser>? ApplicationUsers { get; set; } = [];
        public List<Disease>? Diseases { get; set; } = [];
    }
}
