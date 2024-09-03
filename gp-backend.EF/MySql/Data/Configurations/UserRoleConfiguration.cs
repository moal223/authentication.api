using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace gp_backend.EF.MySql.Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            var adminUserRole = new IdentityUserRole<string>
            {
                UserId = "71c7d19b-93ef-42de-a43c-7a2178dd6d48",
                RoleId = "cbcc5d1f-572b-44ea-bfe2-dfa5d0d1bc8f"
            };

            builder.HasData(adminUserRole);
        }
    }
}
