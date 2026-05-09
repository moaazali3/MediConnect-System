namespace Hospital.Application.DTOs.Admin
{
    public class RevenueByDoctorDto
    {
        public string DoctorName { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalRevenueToday { get; set; }
    }
}
