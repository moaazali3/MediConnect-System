using FluentValidation;
using Hospital.Application.ExternalServices;
using Hospital.Application.Services.Implementations;
using Hospital.Application.Services.Interfaces;
using Hospital.Application.Validators.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Hospital.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            #region JWT
            services.AddScoped<JwtService>();
            var jwtSettings = configuration.GetSection("JWT");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                };
            });
            #endregion

            #region Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IDoctorScheduleService, DoctorScheduleService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IMedicalRecordService, MedicalRecordService>();
            services.AddScoped<IReceptionistService, ReceptionistService>();
            #endregion

            #region FluentValidation
            services.AddValidatorsFromAssembly(typeof(UserLoginDtoValidator).Assembly);
            #endregion

            return services;
        }
    }
}
