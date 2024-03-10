namespace TestApi.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        readonly ICustomerService _customerService;

        public StateController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route("GetStates")]
        public async Task<IActionResult> FetchStates()
        {
            var response = await _customerService.FetchStates();
            if (response.Successful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        

        [HttpGet]
        [Route("GetState/{state}")]
        public async Task<IActionResult> FetchState(string state)
        {
            var response = await _customerService.FetchState(state);
            if (response.Successful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        [Route("GetBanks")]
        public async Task<IActionResult> FetchBanks()
        {
            var response = await _customerService.FetchBanks();
            if (response.Successful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }

