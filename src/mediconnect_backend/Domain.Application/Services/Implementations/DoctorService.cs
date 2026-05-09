using FluentValidation;
using Hospital.Application.DTOs.Doctor;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;
using Hospital.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Application.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateDoctorDto> _createDoctorValidator;
        private readonly IValidator<UpdateDoctorDto> _updateDoctorValidator;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public DoctorService(IUnitOfWork unitOfWork, IValidator<CreateDoctorDto> createDoctorValidator, UserManager<AppUser> userManager = null, IValidator<UpdateDoctorDto> updateDoctorValidator = null, IWebHostEnvironment env = null)
        {
            _unitOfWork = unitOfWork;
            _createDoctorValidator = createDoctorValidator;
            _userManager = userManager;
            _updateDoctorValidator = updateDoctorValidator;
            _env = env;
        }

        public async Task CreateDoctor(CreateDoctorDto model)
        {
            var result = _createDoctorValidator.Validate(model);

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

            var roleResult = await _userManager.AddToRoleAsync(user, "Doctor");

            if (!roleResult.Succeeded)
                throw new Exception(string.Join(",", roleResult.Errors.Select(e => e.Description)));

            var doctor = new Doctor
            {
                UserId = user.Id,
                ExperienceYears = model.ExperienceYears,
                ConsultationFee = model.ConsultationFee,
                SpecializationId = model.SpecializationId,
                IsActive = true,
                Biography = ""
            };

            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task InactiveDoctor(string doctorId)
        {
            var user = await _unitOfWork.Doctors.GetAsync(d => d.UserId == doctorId);

            if (user == null)
                throw new Exception("Doctor not found!");

            user.IsActive = false;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<GetAllDoctorsDto>> GetAllDoctors(string? specializationName = null)
        {
            var doctors = await _unitOfWork.Doctors
                .GetAllAsync(
                filter: d => (string.IsNullOrEmpty(specializationName) || d.specialization.Name == specializationName) && d.IsActive == true,
                selector: d => new GetAllDoctorsDto
                {
                    Id = d.UserId,
                    ProfilePictureUrl = d.ProfilePictureUrl,
                    FirstName = d.AppUser.FirstName,
                    LastName = d.AppUser.LastName,
                    SpecializationName = d.specialization.Name,
                    ExperienceYears = d.ExperienceYears,
                    Gender = d.AppUser.Gender
                }
                );

            return doctors;
        }

        public async Task<GetDoctorDto> GetDoctor(string doctorId, string patientid)
        {
            var doctor = await _unitOfWork.Doctors
                .GetAsync(
                filter: d => d.UserId == doctorId && d.IsActive == true,
                selector: d => new GetDoctorDto
                {
                    Id = d.UserId,
                    ProfilePictureUrl = d.ProfilePictureUrl,
                    FirstName = d.AppUser.FirstName,
                    LastName = d.AppUser.LastName,
                    ExperienceYears = d.ExperienceYears,
                    Biography = d.Biography,
                    ConsultationFee = d.ConsultationFee,
                    DateOfBirth = d.AppUser.DateOfBirth,
                    Gender = d.AppUser.Gender,
                    SpecializationName = d.specialization.Name,
                    IsAppleToAppointment = !d.Appointments.Any(a => a.PatientId == patientid && a.Status == Status.Pending),
                    DoctorSchedules = d.Schedules.Select(ds => new GetDoctorScheduleDto
                    {
                        ScheduleId = ds.ScheduleId,
                        DayOfWeek = ds.DayOfWeek.ToString(),
                        StartTime = ds.StartTime,
                        EndTime = ds.EndTime,
                        IsAvailable = ds.Doctor.Appointments.Count(a => a.DayOfWeek == ds.DayOfWeek && a.Status == Status.Pending) < 16
                    }).ToList()
                });

            return doctor;
        }

        public async Task<GetDoctorDetailsDto> GetDoctor(string doctorId)
        {
            var doctor = await _unitOfWork.Doctors
                .GetAsync(
                filter: d => d.UserId == doctorId && d.IsActive == true,
                selector: d => new GetDoctorDetailsDto
                {
                    Id = d.UserId,
                    ProfilePictureUrl = d.ProfilePictureUrl,
                    FirstName = d.AppUser.FirstName,
                    LastName = d.AppUser.LastName,
                    ExperienceYears = d.ExperienceYears,
                    Biography = d.Biography,
                    ConsultationFee = d.ConsultationFee,
                    DateOfBirth = d.AppUser.DateOfBirth,
                    Gender = d.AppUser.Gender,
                    SpecializationName = d.specialization.Name,
                    DoctorSchedules = d.Schedules.Select(ds => new GetDoctorScheduleDto
                    {
                        ScheduleId = ds.ScheduleId,
                        DayOfWeek = ds.DayOfWeek.ToString(),
                        StartTime = ds.StartTime,
                        EndTime = ds.EndTime,
                        IsAvailable = ds.Doctor.Appointments.Count(a => a.DayOfWeek == ds.DayOfWeek && a.Status == Status.Pending) < 16
                    }).ToList()
                });

            return doctor;
        }

        public async Task<List<GetDoctorNamesDto>> GetDoctorNames()
        {
            var doctors = await _unitOfWork.Doctors
                .GetAllAsync(
                filter: d => d.Receptionist == null,
                selector: d => new GetDoctorNamesDto
                {
                    DoctorId = d.UserId,
                    DoctorName = $"{d.AppUser.FirstName} {d.AppUser.LastName}"
                });

            if (doctors == null)
                throw new Exception("No doctors found!");

            return doctors;
        }

        public async Task<List<GetDoctorWorkingTodayDto>> GetDoctorsThatHasWorkToday()
        {
            var today = DateTime.Today.DayOfWeek;

            var doctors = await _unitOfWork.DoctorSchedules.GetAllAsync(
                filter: s => s.DayOfWeek == today && s.IsAvailable,
                selector: d => new GetDoctorWorkingTodayDto
                {
                    Id = d.Doctor.UserId,
                    ProfilePictureUrl = d.Doctor.ProfilePictureUrl,
                    FirstName = d.Doctor.AppUser.FirstName,
                    LastName = d.Doctor.AppUser.LastName,
                    SpecializationName = d.Doctor.specialization.Name,
                });

            return doctors;
        }

        public async Task UpdateDoctor(string doctorId, UpdateDoctorDto model)
        {
            var result = _updateDoctorValidator.Validate(model);

            if (!result.IsValid)
                throw new Exception(result.ToString(","));

            var doctor = await _unitOfWork.Doctors
                .GetAsync(
                filter: d => d.UserId == doctorId
                );

            var user = await _userManager.FindByIdAsync(doctorId);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Gender = model.Gender;
            user.DateOfBirth = model.DateOfBirth;
            doctor.ExperienceYears = model.ExperienceYears;
            doctor.ConsultationFee = model.ConsultationFee;
            doctor.SpecializationId = model.SpecializationId;

            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<string> UploadProfilePictureAsync(string doctorId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Invalid file");

            var doctor = await _unitOfWork.Doctors.FindByIdAsync(doctorId);

            if (doctor == null)
                throw new Exception("Doctor not found");


            var extension = Path.GetExtension(file.FileName).ToLower();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Invalid image type");

            if (file.Length > 2 * 1024 * 1024)
                throw new Exception("File too large");


            var folderPath = Path.Combine(_env.WebRootPath, "images");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);


            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, fileName);


            if (!string.IsNullOrEmpty(doctor.ProfilePictureUrl))
            {
                var oldPath = Path.Combine(_env.WebRootPath, doctor.ProfilePictureUrl.TrimStart('/'));
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            doctor.ProfilePictureUrl = $"/images/{fileName}";

            await _unitOfWork.Doctors.Update(doctor);
            await _unitOfWork.SaveChangesAsync();

            return doctor.ProfilePictureUrl;
        }

        public async Task ActiveDoctor(string doctorId)
        {
            var user = await _unitOfWork.Doctors.GetAsync(d => d.UserId == doctorId);

            if (user == null)
                throw new Exception("Doctor not found!");

            user.IsActive = true;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<GetAllDoctorForAdminDto>> GetAllDoctorsForAdmin(string? specializationName = null)
        {
            var doctors = await _unitOfWork.Doctors
                .GetAllAsync(
                filter: d => (string.IsNullOrEmpty(specializationName) || d.specialization.Name == specializationName),
                selector: d => new GetAllDoctorForAdminDto
                {
                    Id = d.UserId,
                    IsActive = d.IsActive,
                    ProfilePictureUrl = d.ProfilePictureUrl,
                    FirstName = d.AppUser.FirstName,
                    LastName = d.AppUser.LastName,
                    SpecializationName = d.specialization.Name,
                    ExperienceYears = d.ExperienceYears,
                    Gender = d.AppUser.Gender
                }
                );

            return doctors;
        }
    }
}
