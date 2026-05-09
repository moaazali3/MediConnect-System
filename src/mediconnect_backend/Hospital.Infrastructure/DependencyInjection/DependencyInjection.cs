using Hospital.Application.ExternalServices;
using Hospital.Domain.Entities;
using Hospital.Domain.Repositories.Interfaces;
using Hospital.Infrastructure.ExternalServices;
using Hospital.Infrastructure.Persistence;
using Hospital.Infrastructure.Repositories.Implementations;
using Hospital.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            #region AppDbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("ProductionConnection")));
            #endregion

            #region Identity
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            #endregion

            #region UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            #region Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            #endregion

            #region External Services
            services.Configure<SendGridSettings>(configuration.GetSection("SendGridSettings"));
            services.AddScoped<IEmailService, EmailService>();
            #endregion

            return services;
        }
    }
}
