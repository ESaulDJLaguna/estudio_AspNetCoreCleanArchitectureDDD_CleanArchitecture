using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations
{
	public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
	{
		public void Configure(EntityTypeBuilder<IdentityRole> builder)
		{
			builder.HasData(
				new IdentityRole
				{
					Id = "3e035ea5-232a-4ba2-9dc3-5c507ca77095",
					Name = "Administrator",
					NormalizedName = "ADMINISTRATOR"
				},
				new IdentityRole
				{
					Id = "21cbdd6e-7eda-45b2-a1df-1397acef94b2",
					Name = "Operator",
					NormalizedName = "OPERATOR"
				}
			);
		}
	}
}
