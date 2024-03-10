


namespace TestApi.Services.Implementations;

    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;
        private readonly IOtpService _otpService;
        readonly AppConfig _appConfig;

        public CustomerService(AppDbContext context, IOtpService otpService, IOptions<AppConfig> appConfig)
        {
            _context = context;
            _otpService = otpService;
            _appConfig = appConfig.Value;

        }

        public async Task<ResponseParam> CreateOrUpdate(CustomerRequestDto request)
        {
            var responseHandler = new UResponseHandler();
            try
            {
                var validate = request.Validate();
                if (!validate.Successful)
                {
                    responseHandler.CommitError(validate.Error);
                    return responseHandler.GetResponse();
                }

                var validateState = request.ValidateState();
                if (!validateState.Successful)
                {
                    responseHandler.CommitError(validateState.Error);
                    return responseHandler.GetResponse();
                }

                if (request.Id <= 0)
                {
                    if (await _context.Customers.AnyAsync(a => a.Email == request.Email || a.PhoneNumber == request.PhoneNumber))
                    {
                        responseHandler.CommitError(ErrorConstants.DUPLICATE_RECORD);
                        return responseHandler.GetResponse();
                    }
                }

                Customer _customer = await FindOrCreateCustomer(request.Id);
                _customer.PhoneNumber = request.PhoneNumber;
                _customer.Email = request.Email;
                _customer.Password = request.Password;
                _customer.StateOfResidence = request.StateOfResidence;
                _customer.LGA = request.LGA;

                await _context.SaveChangesAsync();

                var getOtp = await _otpService.GetOtp(_customer.Id);

                if (!getOtp.Successful)
                {
                    responseHandler.CommitError(getOtp.Message, getOtp.Data);
                    return responseHandler.GetResponse();
                }

                responseHandler.CommitResponse(getOtp.Data);
            }
            catch (Exception ex)
            {
                responseHandler.HandleException(ex);
            }
            return responseHandler.GetResponse();
        }


        public async Task<ResponseParam> ResendOtp(ResendOtpDto request)
        {
            var responseHandler = new UResponseHandler();
            try
            {
                if (request.UserId <= 0)
                {
                    responseHandler.CommitError(ErrorConstants.INVALID_ID);
                    return responseHandler.GetResponse();
                }

                await _context.SaveChangesAsync();

                var getOtp = await _otpService.GetOtp(request.UserId);

                if (!getOtp.Successful)
                {
                    responseHandler.CommitError(getOtp.Message, getOtp.Data);
                    return responseHandler.GetResponse();
                }

                responseHandler.CommitResponse(getOtp.Data);
            }
            catch (Exception ex)
            {
                responseHandler.HandleException(ex);
            }
            return responseHandler.GetResponse();
        }

        public async Task<ResponseParam> FetchVerifiedUsers()
        {
            var responseHandler = new UResponseHandler();
            try
            {
                var users = _context.Customers.Where(c => c.IsUserVerified);

                responseHandler.CommitResponse(await users.ToListAsync());
            }
            catch (Exception ex)
            {
                responseHandler.HandleException(ex);
            }
            return responseHandler.GetResponse();
        }
    
    public async Task<ResponseParam> FetchAllUsers()
        {
            var responseHandler = new UResponseHandler();
            try
            {
                var users = await _context.Customers.ToListAsync();

                responseHandler.CommitResponse(users);
            }
            catch (Exception ex)
            {
                responseHandler.HandleException(ex);
            }
            return responseHandler.GetResponse();
        }


        public async Task<ResponseParam> VerifyUser(OtpDto request)
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

                
                Customer _customer = await FindOrCreateCustomer(request.UserId);

                if (_customer.IsUserVerified)
                {
                    responseHandler.CommitError("User already Verified.");
                    return responseHandler.GetResponse();
                }

                var getOtp = await _otpService.VerifyOtp(request);

                if (!getOtp.Successful)
                {
                    return getOtp;
                }

                _customer.IsActive = !_customer.IsActive;
                _customer.IsUserVerified = !_customer.IsUserVerified;

                await _context.SaveChangesAsync();

                responseHandler.CommitResponse();
            }
            catch (Exception ex)
            {
                responseHandler.HandleException(ex);
            }
            return responseHandler.GetResponse();
        }


        public async Task<ResponseParam> FetchStates()
        {
            var responseHandler = new UResponseHandler();
            try
            {
                var states = GetStates.LoadStates();
                responseHandler.CommitResponse(states);
            }
            catch (Exception ex)
            {
                responseHandler.HandleException(ex);
            }
            return responseHandler.GetResponse();
        }

        public async Task<ResponseParam> FetchState(string state)
        {
            var responseHandler = new UResponseHandler();
            try
            {
                var states = GetStates.LoadStates();
                var result = states.FirstOrDefault(x => x.state.ToLower() == state.ToLower());

                if (result is null)
                {

                    responseHandler.CommitError(ErrorConstants.INVALID_INPUT);
                    return responseHandler.GetResponse();
                }
                responseHandler.CommitResponse(result);
            }
            catch (Exception ex)
            {
                responseHandler.HandleException(ex);
            }
            return responseHandler.GetResponse();
        }


        public async Task<ResponseParam> FetchBanks()
        {
            var responseHandler = new UResponseHandler();
            try
            {
                var client = new RestClient();
                var request = new RestRequest($"{_appConfig.GetBanksUrl}", Method.Get);

                RestResponse response = await client.ExecuteAsync(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    responseHandler.CommitError(response.Content);
                    return responseHandler.GetResponse();
                }
                var result = JsonConvert.DeserializeObject<BankResult>(response.Content);

                responseHandler.CommitResponse(result);
            }
            catch (Exception ex)
            {
                responseHandler.HandleException(ex);
            }
            return responseHandler.GetResponse();
        }


        private async Task<Customer> FindOrCreateCustomer(long id)
        {
            var _item = new Customer();
            try
            {
                if (id > 0)
                {
                    _item = await _context.Customers.FindAsync(id);
                    if (_item == null)
                    {
                        throw new Exception(ErrorConstants.NOT_FOUND);
                    }
                    //_item.DateModified = DateTime.Now;
                    //_item.ModifiedBy = _serviceHelper.GetCurrentUserUsername();
                }
                else
                {
                    _item = new Customer();
                    //_item.DateCreated = DateTime.Now;
                    //_item.CreatedBy = _serviceHelper.GetCurrentUserUsername();
                    await _context.Customers.AddAsync(_item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _item;
        }

    }

