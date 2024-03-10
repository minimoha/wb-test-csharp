namespace TestApi.Controllers;

[Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        [Route("CreateOrUpdate")]
        public async Task<IActionResult> AddOrUpdateCustomer(CustomerRequestDto request)
        {
            var response = await _customerService.CreateOrUpdate(request);
            if (response.Successful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        
        [HttpPost]
        [Route("VerifyUser")]
        public async Task<IActionResult> VerifyUser(OtpDto request)
        {
            var response = await _customerService.VerifyUser(request);
            if (response.Successful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        [Route("FetchVerifiedUsers")]
        public async Task<IActionResult> FetchVerifiedUsers()
        {
            var response = await _customerService.FetchVerifiedUsers();
            if (response.Successful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    
        [HttpGet]
        [Route("FetchAllUsers")]
        public async Task<IActionResult> FetchAllUsers()
        {
            var response = await _customerService.FetchAllUsers();
            if (response.Successful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        [Route("ResendOtp")]
        public async Task<IActionResult> ActivateUser(ResendOtpDto request)
        {
            var response = await _customerService.ResendOtp(request);
            if (response.Successful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public BaseController()
        {

        }
    }

