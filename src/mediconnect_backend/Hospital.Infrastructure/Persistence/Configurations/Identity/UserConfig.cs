using Hospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.Persistence.Configurations.Identity
{
    public class UserConfig : IEntityTypeConfiguration<AppUser>
    {
        public static readonly string AdminUserId =
            "b1111111-1111-1111-1111-111111111111";

        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            var admin = new AppUser
            {
                Id = AdminUserId,
                UserName = "admin@hospital.com",
                FirstName = "Admin",
                LastName = "Hospital",
                NormalizedUserName = "ADMIN",
                Email = "admin@hospital.com",
                Gender = "Male",
                DateOfBirth = new DateOnly(1980, 1, 1),
                NormalizedEmail = "ADMIN@HOSPITAL.COM",
                EmailConfirmed = true,
                SecurityStamp = "b7f9c3a1-4e2d-4d8a-9f5e-2c6a7b1e3d90",
                ConcurrencyStamp = "d1111111-1111-1111-1111-111111111111",
                PasswordHash = "AQAAAAIAAYagAAAAEIxf/T4+JM9mGdIzJeillyHuMI4W/4VWohrIBR5adtn7GnJcQUkDWSkwgtpNiKivgw==" // Admin123!
            };

            builder.HasData(admin);
        }
    }
}
