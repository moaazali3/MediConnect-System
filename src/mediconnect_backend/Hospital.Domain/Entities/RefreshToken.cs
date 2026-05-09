namespace Hospital.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }
        public Guid Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
