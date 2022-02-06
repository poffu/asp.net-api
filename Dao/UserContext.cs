using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAppAPI.Dao.Model;

namespace WebAppAPI.Dao
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfiguration(new UserConfiguration());

        public DbSet<WebUserModel> WebUserModels { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<WebUserModel>
    {
        public void Configure(EntityTypeBuilder<WebUserModel> builder)
        {
            builder.HasKey(props => props.user_id);
        }
    }
}