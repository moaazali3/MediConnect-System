using Hospital.Domain.Entities;
using Hospital.Domain.Repositories.Interfaces;
using Hospital.Infrastructure.Persistence;

namespace Hospital.Infrastructure.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Patients = new Repository<Patient>(_db);
            Doctors = new Repository<Doctor>(_db);
            Specializations = new Repository<Specialization>(_db);
            Appointments = new Repository<Appointment>(_db);
            MedicalRecords = new Repository<MedicalRecord>(_db);
            DoctorSchedules = new Repository<DoctorSchedule>(_db);
            Payments = new Repository<Payment>(_db);
            RefreshTokens = new Repository<RefreshToken>(_db);
            Receptionists = new Repository<Receptionist>(_db);
        }
        public IRepository<Patient> Patients { get; }
        public IRepository<Doctor> Doctors { get; }
        public IRepository<Specialization> Specializations { get; }
        public IRepository<Appointment> Appointments { get; }
        public IRepository<MedicalRecord> MedicalRecords { get; }
        public IRepository<DoctorSchedule> DoctorSchedules { get; }
        public IRepository<Payment> Payments { get; }
        public IRepository<RefreshToken> RefreshTokens { get; }
        public IRepository<Receptionist> Receptionists { get; }



        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
