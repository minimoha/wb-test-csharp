

namespace TestApi.Services.Interfaces;

    public interface ICustomerService
    {
        Task<ResponseParam> CreateOrUpdate(CustomerRequestDto request);
        Task<ResponseParam> VerifyUser(OtpDto request);
        Task<ResponseParam> ResendOtp(ResendOtpDto request);
        Task<ResponseParam> FetchStates();
        Task<ResponseParam> FetchState(string state);
        Task<ResponseParam> FetchBanks();
        Task<ResponseParam> FetchVerifiedUsers();
    Task<ResponseParam> FetchAllUsers();
    }

