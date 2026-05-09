using FluentValidation;
using Hospital.Application.DTOs.Receptionist;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Application.Services.Implementations
{
    public class ReceptionistService : IReceptionistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IValidator<CreateReceptionistDto> _createReceptionistValidator;
        private readonly IValidator<UpdateReceptionistDto> _updateReceptionistValidator;

        public ReceptionistService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IValidator<CreateReceptionistDto> createReceptionistValidator, IValidator<UpdateReceptionistDto> updateReceptionistValidator)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _createReceptionistValidator = createReceptionistValidator;
            _updateReceptionistValidator = updateReceptionistValidator;
        }

        public async Task ChangeDoctor(string receptionistId, string doctorId)
        {
            var receptionist = await _unitOfWork.Receptionists.GetAsync(r => r.UserId == receptionistId);

            if (receptionist == null)
                throw new Exception("Receptionist not found.");

            receptionist.DoctorId = doctorId;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CreateReceptionist(CreateReceptionistDto model)
        {
            var result = _createReceptionistValidator.Validate(model);

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
                EmailConfirmed = true
            };

            var creationResult = await _userManager.CreateAsync(user, model.Password);

            if (!creationResult.Succeeded)
                throw new Exception(string.Join(",", creationResult.Errors.Select(e => e.Description)));

            var roleResult = await _userManager.AddToRoleAsync(user, "Receptionist");

            if (!roleResult.Succeeded)
                throw new Exception(string.Join(",", roleResult.Errors.Select(e => e.Description)));

            var receptionist = new Receptionist
            {
                UserId = user.Id,
                DoctorId = model.DoctorId
            };

            await _unitOfWork.Receptionists.AddAsync(receptionist);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteReceptionist(string receptionistId)
        {
            var user = await _userManager.FindByIdAsync(receptionistId);

            if (user == null)
                throw new Exception("Receptionist not found.");

            await _userManager.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<GetReceptionistDto> GetReceptionistByDoctorId(string doctorId)
        {
            var receptionist = await _unitOfWork.Receptionists
                .GetAsync(
                filter: r => r.DoctorId == doctorId,
                selector: r => new GetReceptionistDto
                {
                    Id = r.UserId,
                    FirstName = r.AppUser.FirstName,
                    LastName = r.AppUser.LastName,
                    doctorName = r.Doctor.AppUser.FirstName + " " + r.Doctor.AppUser.LastName,
                    PhoneNumber = r.AppUser.PhoneNumber
                });

            if (receptionist == null)
                throw new Exception("Receptionist not found for the given doctor ID.");

            return receptionist;
        }

        public async Task UpdateReceptionst(string receptionstId, UpdateReceptionistDto model)
        {
            var result = _updateReceptionistValidator.Validate(model);

            if (!result.IsValid)
                throw new Exception(result.ToString(","));

            var user = await _userManager.FindByIdAsync(receptionstId);

            if (user == null)
                throw new Exception("Receptionist not found.");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Gender = model.Gender;
            user.DateOfBirth = model.DateOfBirth;
            user.Address = model.Address;

            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
