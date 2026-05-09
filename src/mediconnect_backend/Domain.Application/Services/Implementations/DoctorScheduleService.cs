using FluentValidation;
using Hospital.Application.DTOs.Doctor;
using Hospital.Application.DTOs.DoctorSchedule;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Repositories.Interfaces;

namespace Hospital.Application.Services.Implementations
{
    public class DoctorScheduleService : IDoctorScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateDoctorScheduleDto> _createDoctorScheduleValidator;

        public DoctorScheduleService(IUnitOfWork unitOfWork, IValidator<CreateDoctorScheduleDto> createDoctorScheduleValidator)
        {
            _unitOfWork = unitOfWork;
            _createDoctorScheduleValidator = createDoctorScheduleValidator;
        }

        public async Task CreateDoctorSchedule(CreateDoctorScheduleDto model, string doctorId)
        {
            var result = _createDoctorScheduleValidator.Validate(model);

            if (!result.IsValid)
                throw new Exception(result.ToString(","));

            foreach (var item in model.DoctorSchedules)
            {
                var dayOfWeek = Enum.Parse<DayOfWeek>(item.DayOfWeek);

                var exists = await _unitOfWork.DoctorSchedules.AnyAsync(s =>
                    s.DoctorId == doctorId &&
                    s.DayOfWeek == dayOfWeek
                );

                if (exists)
                    throw new Exception($"Schedule already exists for {dayOfWeek}");

                var doctorSchedule = new DoctorSchedule
                {
                    DoctorId = doctorId,
                    DayOfWeek = dayOfWeek,
                    StartTime = item.StartTime,
                    EndTime = item.StartTime.Add(TimeSpan.FromHours(8)),
                    IsAvailable = true
                };

                await _unitOfWork.DoctorSchedules.AddAsync(doctorSchedule);
            }

            var doctor = await _unitOfWork.Doctors.FindByIdAsync(doctorId);
            doctor.IsActive = true;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteDoctorSchedule(string doctorId)
        {
            var schedules = await _unitOfWork.DoctorSchedules
                .GetAllAsync(filter: x => x.DoctorId == doctorId);

            foreach (var schedule in schedules)
            {
                await _unitOfWork.DoctorSchedules.Delete(schedule);
            }

            var doctor = await _unitOfWork.Doctors.FindByIdAsync(doctorId);
            doctor.IsActive = false;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<GetDoctorScheduleDto>> GetDoctorSchedule(string doctorId)
        {
            var schedules = await _unitOfWork.DoctorSchedules
                .GetAllAsync(
                filter: x => x.DoctorId == doctorId,
                selector: x => new GetDoctorScheduleDto
                {
                    ScheduleId = x.ScheduleId,
                    DayOfWeek = x.DayOfWeek.ToString(),
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    IsAvailable = x.IsAvailable
                });

            if (schedules == null)
                throw new Exception("Doctor schedule not found.");

            return schedules;
        }

        public async Task UpdateDoctorSchedule(CreateDoctorScheduleDto model, string doctorId)
        {
            var result = _createDoctorScheduleValidator.Validate(model);

            if (!result.IsValid)
                throw new Exception(result.ToString(","));


            var existingSchedules = await _unitOfWork.DoctorSchedules
                .GetAllAsync(filter: x => x.DoctorId == doctorId);

            foreach (var schedule in existingSchedules)
            {
                await _unitOfWork.DoctorSchedules.Delete(schedule);
            }

            foreach (var item in model.DoctorSchedules)
            {
                var doctorSchedule = new DoctorSchedule
                {
                    DoctorId = doctorId,
                    DayOfWeek = Enum.Parse<DayOfWeek>(item.DayOfWeek),
                    StartTime = item.StartTime,
                    EndTime = item.StartTime.Add(TimeSpan.FromHours(8)),
                    IsAvailable = true
                };

                await _unitOfWork.DoctorSchedules.AddAsync(doctorSchedule);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
