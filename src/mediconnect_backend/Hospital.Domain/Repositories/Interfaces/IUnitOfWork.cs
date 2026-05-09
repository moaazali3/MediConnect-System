using Hospital.Domain.Entities;

namespace Hospital.Domain.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Patient> Patients { get; }
        IRepository<Doctor> Doctors { get; }
        IRepository<Specialization> Specializations { get; }
        IRepository<Appointment> Appointments { get; }
        IRepository<MedicalRecord> MedicalRecords { get; }
        IRepository<DoctorSchedule> DoctorSchedules { get; }
        IRepository<Payment> Payments { get; }
        IRepository<RefreshToken> RefreshTokens { get; }
        IRepository<Receptionist> Receptionists { get; }

        Task<int> SaveChangesAsync();
    }
}
