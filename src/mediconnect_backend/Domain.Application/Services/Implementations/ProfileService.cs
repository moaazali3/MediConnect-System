using FluentValidation;
using Hospital.Application.DTOs.Profile;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Application.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IValidator<UpdatePatientProfileDto> _updatePatientProfileValidator;
        private readonly IValidator<UpdateDoctorProfileDto> _updateDoctorProfileValidator;
        private readonly IValidator<ChangePasswordDto> _changePasswordValidator;
        private readonly IValidator<UpdateReceptionistProfileDto> _updateReceptionistProfileValidator;

        public ProfileService(IUnitOfWork unitOfWork, IValidator<UpdatePatientProfileDto> updatePatientProfileValidator, UserManager<AppUser> userManager, IValidator<UpdateDoctorProfileDto> updateDoctorProfileValidator, IValidator<ChangePasswordDto> changePasswordValidator, IValidator<UpdateReceptionistProfileDto> updateReceptionistProfileValidator)
        {
            _unitOfWork = unitOfWork;
            _updatePatientProfileValidator = updatePatientProfileValidator;
            _userManager = userManager;
            _updateDoctorProfileValidator = updateDoctorProfileValidator;
            _changePasswordValidator = changePasswordValidator;
            _updateReceptionistProfileValidator = updateReceptionistProfileValidator;
        }

        public async Task ChangePassword(string id, ChangePasswordDto model)
        {
            var resultValidator = _changePasswordValidator.Validate(model);

            if (!resultValidator.IsValid)
                throw new Exception(resultValidator.ToString(","));

            var user = await _userManager.FindByIdAsync(id);

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
                throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));
        }

        public async Task<GetDoctorProfileDto> GetDoctorProfile(string id)
        {
            var doctor = await _unitOfWork.Doctors
                .GetAsync(
                filter: d => d.UserId == id,
                selector: d => new GetDoctorProfileDto
                {
                    
                    FirstName = d.AppUser.FirstName,
                    LastName = d.AppUser.LastName,
                    ProfilePictureUrl = d.ProfilePictureUrl,
                    Email = d.AppUser.Email,
                    PhoneNumber = d.AppUser.PhoneNumber,
                    DateOfBirth = d.AppUser.DateOfBirth,
                    Address = d.AppUser.Address,
                    Gender = d.AppUser.Gender,
                    SpecializationName = d.specialization.Name,
                    ConsultationFee = d.ConsultationFee,
                    ExperienceYears = d.ExperienceYears,
                    Biography = d.Biography,
                });

            return doctor;
        }

        public async Task<GetPatientProfileDto> GetPatientProfile(string id)
        {
            var patient = await _unitOfWork.Patients
                .GetAsync(
                filter: p => p.UserId == id,
                selector: p => new GetPatientProfileDto
                {
                    FirstName = p.AppUser.FirstName,
                    LastName = p.AppUser.LastName,
                    Email = p.AppUser.Email,
                    PhoneNumber = p.AppUser.PhoneNumber,
                    EmergencyContact = p.EmergencyContact,
                    DateOfBirth = p.AppUser.DateOfBirth,
                    Address = p.AppUser.Address,
                    BloodType = p.BloodType,
                    Gender = p.AppUser.Gender,
                    Height = p.Height,
                    Weight = p.Weight
                });

            return patient;
        }

        public async Task<GetReceptionistProfileDto> GetReceptionistProfile(string id)
        {
            var receptionist = await _unitOfWork.Receptionists
                .GetAsync(
                filter: r => r.UserId == id,
                selector: r => new GetReceptionistProfileDto
                {
                    FirstName = r.AppUser.FirstName,
                    LastName = r.AppUser.LastName,
                    DoctorName = r.Doctor.AppUser.FirstName + " " + r.Doctor.AppUser.LastName,
                    DoctorId = r.Doctor.UserId,
                    Gender = r.AppUser.Gender,
                    Email = r.AppUser.Email,
                    PhoneNumber = r.AppUser.PhoneNumber,
                    DateOfBirth = r.AppUser.DateOfBirth,
                    Address = r.AppUser.Address,
                });

            return receptionist;
        }

        public async Task UpdateDoctorProfile(string id, UpdateDoctorProfileDto model)
        {
            var result = _updateDoctorProfileValidator.Validate(model);

            if (!result.IsValid)
                throw new Exception(result.ToString(","));

            var doctor = await _unitOfWork.Doctors
                .GetAsync(
                filter: d => d.UserId == id
                );

            var user = await _userManager.FindByIdAsync(id);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;
            user.Address = model.Address;
            user.Gender = model.Gender;
            doctor.Biography = model.Biography;

            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdatePatientProfile(string id, UpdatePatientProfileDto model)
        {
            var result = _updatePatientProfileValidator.Validate(model);

            if (!result.IsValid)
                throw new Exception(result.ToString(","));

            var patient = await _unitOfWork.Patients
                .GetAsync(
                filter: p => p.UserId == id
                );

            var user = await _userManager.FindByIdAsync(id);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;
            user.Gender = model.Gender;
            user.Address = model.Address;
            patient.EmergencyContact = model.EmergencyContact;
            patient.BloodType = model.BloodType;
            patient.Height = model.Height;
            patient.Weight = model.Weight;

            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateReceptionistProfile(string id, UpdateReceptionistProfileDto model)
        {
            var result = _updateReceptionistProfileValidator.Validate(model);

            if (!result.IsValid)
                throw new Exception(result.ToString(","));

            var receptionist = await _unitOfWork.Receptionists
                .GetAsync(
                filter: r => r.UserId == id
                );

            var user = await _userManager.FindByIdAsync(id);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;
            user.Gender = model.Gender;
            user.Address = model.Address;

            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
