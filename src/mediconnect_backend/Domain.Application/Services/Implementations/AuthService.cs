using FluentValidation;
using Hospital.Application.DTOs.Authentication;
using Hospital.Application.ExternalServices;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UserLoginDto> _loginValidator;
        private readonly IValidator<UserRegisterDto> _regitserValidator;
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtService _jwtService;
        private readonly IEmailService _emailService;

        public AuthService(IUnitOfWork unitOfWork, IValidator<UserLoginDto> loginValidator, UserManager<AppUser> userManager, JwtService jwtService, IValidator<UserRegisterDto> registerValidator, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _loginValidator = loginValidator;
            _userManager = userManager;
            _jwtService = jwtService;
            _regitserValidator = registerValidator;
            _emailService = emailService;
        }

        public async Task ConfirmEmail(string email, string otp)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                throw new Exception("User not found!");

            var storedOtp = await _userManager.GetAuthenticationTokenAsync(user, "Default", "EmailConfirmation");

            if (storedOtp != otp)
                throw new Exception("Invalid OTP!");

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "EmailConfirmation");
        }

        public async Task<AuthDto> Login(UserLoginDto model)
        {
            var result = _loginValidator.Validate(model);

            if (!result.IsValid)
                throw new Exception(result.ToString(","));

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                throw new Exception("Email or password is incorrect!");

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if(!isEmailConfirmed)
                throw new Exception("Email is not confirmed!");

            var isAuthenticated = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!isAuthenticated)
                throw new Exception("Email or password is incorrect!");

            var userRole = await _userManager.GetRolesAsync(user);

            var token = _jwtService.GenerateToken(
                new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = userRole.First()
                });

            await _unitOfWork.RefreshTokens.DeleteWhereAsync(rt => rt.UserId == user.Id);

            var authDto = new AuthDto
            {
                Token = token,
                RefreshToken = Guid.NewGuid()
            };

            await _unitOfWork.RefreshTokens.AddAsync(new RefreshToken
            {
                UserId = user.Id,
                Token = authDto.RefreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });

            await _unitOfWork.SaveChangesAsync();

            return authDto;
        }

        public async Task<AuthDto> RefreshToken(Guid refreshToken)
        {
            var refToken = await _unitOfWork.RefreshTokens.GetAsync(rt => rt.Token == refreshToken && rt.ExpiryDate > DateTime.UtcNow);

            if (refToken == null)
                throw new Exception("Invalid refresh token!");

            var user = await _userManager.FindByIdAsync(refToken.UserId);

            if (user == null)
                throw new Exception("User not found!");

            var userRole = await _userManager.GetRolesAsync(user);

            var token = _jwtService.GenerateToken(
                new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = userRole.First()
                });

            var authDto = new AuthDto
            {
                Token = token,
                RefreshToken = Guid.NewGuid()
            };

            refToken.Token = authDto.RefreshToken;

            await _unitOfWork.SaveChangesAsync();

            return authDto;
        }

        public async Task Register(UserRegisterDto model)
        {
            var result = _regitserValidator.Validate(model);

            if (!result.IsValid)
                throw new Exception(result.ToString(","));

            var user = new AppUser
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                EmailConfirmed = false
            };

            var creationResult = await _userManager.CreateAsync(user, model.Password);

            if (!creationResult.Succeeded)
                throw new Exception(string.Join(",", creationResult.Errors.Select(e => e.Description)));

            var roleResult = await _userManager.AddToRoleAsync(user, "Patient");

            if (!roleResult.Succeeded)
                throw new Exception(string.Join(",", roleResult.Errors.Select(e => e.Description)));

            var patient = new Patient()
            {
                UserId = user.Id,
                BloodType = model.BloodType,
                Height = model.Height,
                Weight = model.Weight,
                EmergencyContact = model.EmergencyContact
            };

            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.SaveChangesAsync();

            var otp = new Random().Next(100000, 999999).ToString();

            var emailBody = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f6f9;
                            padding: 20px;
                        }}
                        .container {{
                            max-width: 500px;
                            margin: auto;
                            background: #ffffff;
                            border-radius: 10px;
                            overflow: hidden;
                            box-shadow: 0 4px 10px rgba(0,0,0,0.1);
                        }}
                        .header {{
                            background-color: #2c7be5;
                            color: white;
                            padding: 15px;
                            text-align: center;
                            font-size: 22px;
                            font-weight: bold;
                        }}
                        .content {{
                            padding: 20px;
                            text-align: center;
                            color: #333;
                        }}
                        .otp {{
                            font-size: 32px;
                            font-weight: bold;
                            color: #2c7be5;
                            letter-spacing: 6px;
                            margin: 25px 0;
                        }}
                        .footer {{
                            font-size: 12px;
                            color: #888;
                            padding: 15px;
                            text-align: center;
                            border-top: 1px solid #eee;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            Medi-Connect
                        </div>
                        <div class='content'>
                            <h2>Email Verification</h2>
                            <p>Hello {user.FirstName},</p>
                            <p>Welcome to <strong>Medi-Connect</strong>! Use the OTP below to verify your email:</p>

                            <div class='otp'>{otp}</div>

                            <p>This code is valid for a limited time. Please do not share it with anyone.</p>
                        </div>
                        <div class='footer'>
                            <p>If you didn’t request this, you can ignore this email.</p>
                            <p>&copy; {DateTime.Now.Year} Medi-Connect. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";

            await _userManager.SetAuthenticationTokenAsync(user, "Default", "EmailConfirmation", otp);

            await _emailService.SendEmailAsync(user.Email, "Email Confirmation", emailBody);
        }
    }
}
