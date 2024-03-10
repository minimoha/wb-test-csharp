
namespace TestApi.Models;
    public class OtpHistory
    {
        public long Id { get; set; }
        public string OTP { get; set; }
        public long UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime Expiry { get; set; }
        public bool IsUsed { get; set; } = false;
    }

    public class OtpDto
    {
        public long UserId { get; set; }
        public string OTP { get; set; }
    }
    
    public class ResendOtpDto
    {
        public long UserId { get; set; }
    }
