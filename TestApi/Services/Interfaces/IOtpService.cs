namespace TestApi.Services.Interfaces;
public interface IOtpService
    {
        Task<ResponseParam> GetOtp(long userId);
        Task<ResponseParam> VerifyOtp(OtpDto request);
    }

