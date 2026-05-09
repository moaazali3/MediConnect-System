using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.Persistence.Configurations.Identity
{
    public class RoleConfig : IEntityTypeConfiguration<IdentityRole>
    {
        public static readonly string AdminRoleId =
            "a1111111-1111-1111-1111-111111111111";

        public static readonly string DoctorRoleId =
            "a2222222-2222-2222-2222-222222222222";

        public static readonly string PatientRoleId =
            "a3333333-3333-3333-3333-333333333333";

        public static readonly string ReceptionistRoleId =
            "a4444444-4444-4444-4444-444444444444";


        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(

                new IdentityRole
                {
                    Id = AdminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "c1111111-1111-1111-1111-111111111111"
                },

                new IdentityRole
                {
                    Id = DoctorRoleId,
                    Name = "Doctor",
                    NormalizedName = "DOCTOR",
                    ConcurrencyStamp = "c2222222-2222-2222-2222-222222222222"
                },

                new IdentityRole
                {
                    Id = PatientRoleId,
                    Name = "Patient",
                    NormalizedName = "PATIENT",
                    ConcurrencyStamp = "c3333333-3333-3333-3333-333333333333"
                },

                new IdentityRole
                {
                    Id = ReceptionistRoleId,
                    Name = "Receptionist",
                    NormalizedName = "RECEPTIONIST",
                    ConcurrencyStamp = "c4444444-4444-4444-4444-444444444444"
                }

            );
        }
    }
}
