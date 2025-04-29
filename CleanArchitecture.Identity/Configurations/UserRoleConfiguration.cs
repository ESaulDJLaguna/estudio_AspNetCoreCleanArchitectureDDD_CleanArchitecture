using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations
{
	public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
		{
			builder.HasData(
				new IdentityUserRole<string>
				{
					RoleId = "3e035ea5-232a-4ba2-9dc3-5c507ca77095",
					UserId = "c2f25781-772a-4038-a598-970d4bb39363"
				},
				new IdentityUserRole<string>
				{
					RoleId = "21cbdd6e-7eda-45b2-a1df-1397acef94b2",
					UserId = "3747618c-0a63-4c29-bd58-1fc91a4257fa"
				}
			);
		}
	}
}
