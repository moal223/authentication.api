using gp_backend.Core.Models;
using gp_backend.EF.MySql.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace gp_backend.EF.MySql.Data
{
    public class MySqlDbContext : IdentityDbContext<ApplicationUser>
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
        }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Disease> Diseases { get; set; }
        public virtual DbSet<FileDescription> Images { get; set; }
        public virtual DbSet<Wound> Wounds { get; set; }
        public virtual DbSet<FeedBack> FeedBacks { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
    }
}
