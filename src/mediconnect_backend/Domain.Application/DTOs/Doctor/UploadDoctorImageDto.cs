using Microsoft.AspNetCore.Http;

namespace Hospital.Application.DTOs.Doctor
{
    public class UploadDoctorImageDto
    {
        public IFormFile File { get; set; }
    }
}
