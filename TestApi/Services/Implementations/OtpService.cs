

namespace TestApi.Services.Implementations;

    public class OtpService : IOtpService
    {
        private readonly AppDbContext _context;
        public OtpService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseParam> GetOtp(long userId)
        {
            var responseHandler = new UResponseHandler();
            try
            {
                if (userId <= 0)
                {
                    responseHandler.CommitError(ErrorConstants.INVALID_ID);
                    return responseHandler.GetResponse();
                }


                if (!await _context.Customers.AnyAsync(a => a.Id == userId))
                {
                    responseHandler.CommitError("Invalid User ID.");
                    return responseHandler.GetResponse();
                }

                OtpHistory _otp = new OtpHistory();
                _otp.UserId = userId;
                _otp.DateCreated = DateTime.Now;
                _otp.OTP = RandomDigits();
                _otp.Expiry = DateTime.Now.AddMinutes(10);

                await _context.AddAsync(_otp);
                await _context.SaveChangesAsync();

                var response = new OtpDto
                {
                    UserId = userId,
                    OTP = _otp.OTP
                };

                responseHandler.CommitResponse(response);
            }
            catch (Exception ex)
            {
                responseHandler.HandleException(ex);
            }

            return responseHandler.GetResponse();

        }
        
        
        public async Task<ResponseParam> VerifyOtp(OtpDto request)
        {
            var responseHandler = new UResponseHandler();
            try
            {
                if (request.UserId <= 0)
                {
                    responseHandler.CommitError(ErrorConstants.INVALID_ID);
                    return responseHandler.GetResponse();
                }
                
                if (string.IsNullOrWhiteSpace(request.OTP))
                {
                    responseHandler.CommitError(ErrorConstants.INVALID_INPUT);
                    return responseHandler.GetResponse();
                }

                var _otp = await _context.OtpHistory.OrderByDescending(x => x.DateCreated).FirstOrDefaultAsync(o => o.OTP == request.OTP && o.UserId == request.UserId && !o.IsUsed);

                if (_otp is null)
                {
                    responseHandler.CommitError(ErrorConstants.NO_RECORD);
                    return responseHandler.GetResponse();
                }
                
                if (_otp.Expiry < DateTime.Now)
                {
                    responseHandler.CommitError("OTP Expired.");
                    return responseHandler.GetResponse();
                }

                _otp.IsUsed = !_otp.IsUsed;

                await _context.SaveChangesAsync();

                responseHandler.CommitResponse();
            }
            catch (Exception ex)
            {
                responseHandler.HandleException(ex);
            }

            return responseHandler.GetResponse();
        }

        private string RandomDigits(int length = 8)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
    }

