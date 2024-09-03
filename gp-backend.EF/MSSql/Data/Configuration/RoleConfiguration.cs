using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace gp_backend.EF.MSSql.Data.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            var adminRole = new IdentityRole
            {
                Id = "cbcc5d1f-572b-44ea-bfe2-dfa5d0d1bc8f",
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            builder.HasData(adminRole);
        }
    }
}
