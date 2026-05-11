using FluentValidation;
using Hospital.Application.DTOs.Appointment;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;
using Hospital.Domain.Repositories.Interfaces;

namespace Hospital.Application.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateAppointmentDto> _createAppointmentValidator;

        public AppointmentService(IUnitOfWork unitOfWork, IValidator<CreateAppointmentDto> createAppointmentValidator)
        {
            _unitOfWork = unitOfWork;
            _createAppointmentValidator = createAppointmentValidator;
        }

        public async Task<Guid> CreateAppointment(CreateAppointmentDto model)
        {
            var result = _createAppointmentValidator.Validate(model);

            if (!result.IsValid)
                throw new Exception(result.ToString(","));

            if (model.AppointmentDate < DateOnly.FromDateTime(DateTime.Now))
                throw new Exception("Cannot book an appointment in the past.");

            // Parse DayOfWeek
            var parsedDay = Enum.Parse<DayOfWeek>(model.DayOfWeek);

            // Validate that DayOfWeek matches AppointmentDate
            if (model.AppointmentDate.DayOfWeek != parsedDay)
                throw new Exception("DayOfWeek does not match AppointmentDate.");

            // Get doctor schedule (still uses DayOfWeek)
            var doctorScheduleTime = await _unitOfWork.DoctorSchedules
                .GetAsync(
                    selector: x => new { x.StartTime, x.EndTime },
                    filter: x => x.DoctorId == model.DoctorId && x.DayOfWeek == parsedDay
                );

            if (doctorScheduleTime == null)
                throw new Exception("Doctor is not available on this day.");

            // Get existing appointments for SAME DATE (not DayOfWeek anymore)
            var appointmentStartTimes = await _unitOfWork.Appointments
                .GetAllAsync(
                    selector: x => x.StartTime,
                    filter: x => x.DoctorId == model.DoctorId &&
                                 x.AppointmentDate == model.AppointmentDate &&
                                 x.Status == Status.Pending
                );

            var lastQueueNumber = await _unitOfWork.Appointments
                .GetLastQueueNumberAsync(
                    model.DoctorId,
                    model.AppointmentDate.ToDateTime(TimeOnly.MinValue)
                );

            var appointment = new Appointment()
            {
                DoctorId = model.DoctorId,
                PatientId = model.PatientId,
                AppointmentDate = model.AppointmentDate, // FIXED
                DayOfWeek = parsedDay,
                QueueNumber = lastQueueNumber + 1,
                Status = Status.Pending
            };

            // Generate time slot
            var baseTime = doctorScheduleTime.StartTime;
            var currentLocalTime = DateTime.UtcNow.AddHours(3);
            if (model.AppointmentDate == DateOnly.FromDateTime(currentLocalTime))
            {
                var currentTimeWithBuffer = TimeOnly.FromDateTime(currentLocalTime).Add(TimeSpan.FromMinutes(30));
                if (baseTime < currentTimeWithBuffer)
                {
                    // Calculate next 30-min block from buffered time
                    int remainder = currentTimeWithBuffer.Minute % 30;
                    int minutesToAdd = remainder == 0 ? 0 : 30 - remainder;
                    baseTime = currentTimeWithBuffer.Add(TimeSpan.FromMinutes(minutesToAdd));
                }
            }

            if (!appointmentStartTimes.Any())
            {
                appointment.StartTime = baseTime;
            }
            else
            {
                var lastStartTime = appointmentStartTimes.Max();
                var nextSlot = lastStartTime.Add(TimeSpan.FromMinutes(30));
                
                // If the next slot from existing appointments is somehow in the past, use baseTime instead
                appointment.StartTime = nextSlot > baseTime ? nextSlot : baseTime;
            }

            appointment.EndTime = appointment.StartTime.Add(TimeSpan.FromMinutes(30));

            // Prevent exceeding doctor schedule
            if (appointment.EndTime > doctorScheduleTime.EndTime)
                throw new Exception("No available slots for this doctor on this date.");

            // Prevent same doctor same date
            var hasSameDoctorSameDay = await _unitOfWork.Appointments.AnyAsync(a =>
                a.PatientId == model.PatientId &&
                a.DoctorId == model.DoctorId &&
                a.Status != Status.Cancelled &&
                a.AppointmentDate == model.AppointmentDate
            );

            if (hasSameDoctorSameDay)
                throw new Exception("You already have an appointment with this doctor on this date.");

            // Prevent time conflict (ANY doctor)
            var hasConflict = await _unitOfWork.Appointments.AnyAsync(a =>
                a.PatientId == model.PatientId &&
                a.Status != Status.Cancelled &&
                a.AppointmentDate == model.AppointmentDate &&
                appointment.StartTime < a.EndTime &&
                appointment.EndTime > a.StartTime
            );

            if (hasConflict)
                throw new Exception("Patient already has an appointment at this time.");

            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            return appointment.AppointmentId;
        }

        public async Task<List<GetDoctorAppointmentsDto>> GetDoctorAppointments(string doctorId)
        {
            var appointments = await _unitOfWork.Appointments
                .GetAllAsync(
                filter: x => x.DoctorId == doctorId,
                selector: x => new GetDoctorAppointmentsDto
                {
                    AppointmentId = x.AppointmentId,
                    PatientId = x.PatientId,
                    PatientName = x.Patient.AppUser.FirstName + " " + x.Patient.AppUser.LastName,
                    AppointmentDate = x.AppointmentDate,
                    DayOfWeek = x.DayOfWeek.ToString(),
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    QueueNumber = x.QueueNumber,
                    Status = x.Status.ToString()
                });

            var sortedAppointments = appointments.OrderBy(a => a.AppointmentDate)
                                                .ThenBy(a => a.StartTime)
                                                .ToList();

            return sortedAppointments;
        }

        public async Task<List<GetPatientAppointmentsDto>> GetPatientAppointments(string patientId)
        {
            var appointments = await _unitOfWork.Appointments
                .GetAllAsync(
                filter: x => x.PatientId == patientId,
                selector: x => new GetPatientAppointmentsDto
                {
                    AppointmentId = x.AppointmentId,
                    DoctorId = x.DoctorId,
                    DoctorName = x.Doctor.AppUser.FirstName + " " + x.Doctor.AppUser.LastName,
                    AppointmentDate = x.AppointmentDate,
                    DayOfWeek = x.DayOfWeek.ToString(),
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    QueueNumber = x.QueueNumber,
                    Status = x.Status.ToString()
                });

            var sortedAppointments = appointments.OrderBy(a => a.AppointmentDate)
                                             .ThenBy(a => a.StartTime)
                                             .ToList();

            return sortedAppointments;
        }

        public async Task CompleteAppointmentStatus(string appointmentId)
        {
            var appointment = await _unitOfWork.Appointments.GetAsync(x => x.AppointmentId.ToString() == appointmentId);

            if (appointment == null)
                throw new Exception("Appointment Not Found");

            if (appointment.Status == Status.Completed)
                throw new Exception("Appointment is already completed");

            appointment.Status = Status.Completed;

            await _unitOfWork.Appointments.Update(appointment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CancelAppointmentStatus(string appointmentId)
        {
            var appointment = await _unitOfWork.Appointments.GetAsync(x => x.AppointmentId.ToString() == appointmentId);

            if (appointment == null)
                throw new Exception("Appointment Not Found");

            if (appointment.Status == Status.Cancelled)
                throw new Exception("Appointment is already cancelled");

            appointment.Status = Status.Cancelled;

            await _unitOfWork.Appointments.Update(appointment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<GetAdminAppointmentsDto>> GetTodayAppointments()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var appointments = await _unitOfWork.Appointments.GetAllAsync(
                filter: a => a.AppointmentDate == today,
                selector: a => new GetAdminAppointmentsDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientName = a.Patient.AppUser.FirstName + " " + a.Patient.AppUser.LastName,
                    DoctorName = a.Doctor.AppUser.FirstName + " " + a.Doctor.AppUser.LastName,
                    AppointmentDate = a.AppointmentDate,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status.ToString()
                }
            );

            return appointments;
        }

        public async Task<List<GetAdminAppointmentsDto>> GetTodayAppointmentsBySpecialization(string specializationName)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var appointments = await _unitOfWork.Appointments.GetAllAsync(
                filter: a => a.AppointmentDate == today && a.Doctor.specialization.Name == specializationName,
                selector: a => new GetAdminAppointmentsDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientName = a.Patient.AppUser.FirstName + " " + a.Patient.AppUser.LastName,
                    DoctorName = a.Doctor.AppUser.FirstName + " " + a.Doctor.AppUser.LastName,
                    AppointmentDate = a.AppointmentDate,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status.ToString()
                }
            );

            return appointments;
        }

        public async Task<List<GetAdminAppointmentsDto>> GetTodayAppointmentsByDoctor(string doctorId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var appointments = await _unitOfWork.Appointments.GetAllAsync(
                filter: a => a.AppointmentDate == today && a.DoctorId == doctorId,
                selector: a => new GetAdminAppointmentsDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientName = a.Patient.AppUser.FirstName + " " + a.Patient.AppUser.LastName,
                    DoctorName = a.Doctor.AppUser.FirstName + " " + a.Doctor.AppUser.LastName,
                    AppointmentDate = a.AppointmentDate,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status.ToString()
                }
            );

            return appointments;
        }

        public async Task<ExpectedAppointmentInfoDto> ExpectedNumber(string doctorId, DateTime appointmentDate)
        {
            var lastQueueNumber = await _unitOfWork.Appointments.GetLastQueueNumberAsync(doctorId, appointmentDate);
            var queueNumber = lastQueueNumber + 1;

            var parsedDay = appointmentDate.DayOfWeek;
            var doctorScheduleTime = await _unitOfWork.DoctorSchedules
                .GetAsync(
                    selector: x => new { x.StartTime, x.EndTime },
                    filter: x => x.DoctorId == doctorId && x.DayOfWeek == parsedDay
                );

            if (doctorScheduleTime == null)
                return new ExpectedAppointmentInfoDto { ExpectedNumber = queueNumber, ExpectedTime = "N/A" };

            var appointmentStartTimes = await _unitOfWork.Appointments
                .GetAllAsync(
                    selector: x => x.StartTime,
                    filter: x => x.DoctorId == doctorId &&
                                 x.AppointmentDate == DateOnly.FromDateTime(appointmentDate) &&
                                 x.Status == Status.Pending
                );

            var baseTime = doctorScheduleTime.StartTime;
            var currentLocalTime = DateTime.UtcNow.AddHours(3);
            if (DateOnly.FromDateTime(appointmentDate) == DateOnly.FromDateTime(currentLocalTime))
            {
                var currentTimeWithBuffer = TimeOnly.FromDateTime(currentLocalTime).Add(TimeSpan.FromMinutes(30));
                if (baseTime < currentTimeWithBuffer)
                {
                    int remainder = currentTimeWithBuffer.Minute % 30;
                    int minutesToAdd = remainder == 0 ? 0 : 30 - remainder;
                    baseTime = currentTimeWithBuffer.Add(TimeSpan.FromMinutes(minutesToAdd));
                }
            }

            TimeOnly expectedStartTime;
            if (!appointmentStartTimes.Any())
            {
                expectedStartTime = baseTime;
            }
            else
            {
                var lastStartTime = appointmentStartTimes.Max();
                var nextSlot = lastStartTime.Add(TimeSpan.FromMinutes(30));
                expectedStartTime = nextSlot > baseTime ? nextSlot : baseTime;
            }

            // Convert to HH:mm format
            string expectedTimeString = new DateTime(appointmentDate.Year, appointmentDate.Month, appointmentDate.Day, 
                                            expectedStartTime.Hour, expectedStartTime.Minute, 0).ToString("hh:mm tt");

            return new ExpectedAppointmentInfoDto 
            { 
                ExpectedNumber = queueNumber, 
                ExpectedTime = expectedTimeString 
            };
        }

        public async Task<List<GetAllAppointmentsDto>> GetAllAppointments()
        {
            var appointments = await _unitOfWork.Appointments.GetAllAsync(
                filter: a => true,
                selector: a => new GetAllAppointmentsDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientName = a.Patient.AppUser.FirstName + " " + a.Patient.AppUser.LastName,
                    DoctorName = a.Doctor.AppUser.FirstName + " " + a.Doctor.AppUser.LastName,
                    SpecializationName = a.Doctor.specialization.Name,
                    AppointmentDate = a.AppointmentDate,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status.ToString()
                }
            );

            if(!appointments.Any())
                throw new Exception("No appointments found.");

            return appointments;
        }

        public async Task<List<GetReceptionistAppointmentsDto>> GetReceptionistAppointments(string receptionistId)
        {
            var receptionist = await _unitOfWork.Receptionists.GetAsync(r => r.UserId == receptionistId);

            var appointments = await _unitOfWork.Appointments
                .GetAllAsync(
                filter: x => x.DoctorId == receptionist.DoctorId,
                selector: x => new GetReceptionistAppointmentsDto
                {
                    AppointmentId = x.AppointmentId,
                    DoctorId = x.DoctorId,
                    DoctorName = x.Doctor.AppUser.FirstName + " " + x.Doctor.AppUser.LastName,
                    PatientName = x.Patient.AppUser.FirstName + " " + x.Patient.AppUser.LastName,
                    PatientId = x.PatientId,
                    AppointmentDate = x.AppointmentDate,
                    DayOfWeek = x.DayOfWeek.ToString(),
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    QueueNumber = x.QueueNumber,
                    Status = x.Status.ToString()
                });

            if(!appointments.Any())
                throw new Exception("No appointments found.");

            return appointments;
        }
    }
}
