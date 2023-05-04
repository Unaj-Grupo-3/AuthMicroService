

namespace Domain.Entities
{
    public class Authentication
    {
        public Guid AuthId { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int UserId { get; set; }
    }
}
