namespace Hospital.Application.DTOs.Authentication
{
    public class AuthDto
    {
        public string Token { get; set; }
        public Guid RefreshToken { get; set; }
    }
}
