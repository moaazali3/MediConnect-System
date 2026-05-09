namespace Hospital.Application.DTOs.Admin
{
    public class AdminDashboardDto
    {
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalAppointments { get; set; }
        public int TotalAppointmentsToday { get; set; }
        public int TotalPendingAppointmentsToday { get; set; }
        public int TotalCompletedAppointmentsToday { get; set; }
        public int TotalCancelledAppointmentsToday { get; set; }
        public int totalpendingAppointments { get; set; }
        public int totalCompletedAppointments { get; set; }
        public int totalCancelledAppointments { get; set; }
        public decimal TotalRevenueToday { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
