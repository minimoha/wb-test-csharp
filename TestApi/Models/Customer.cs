

namespace TestApi.Models;

    public class Customer
    {
        public long Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string StateOfResidence { get; set; }
        public string LGA { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsUserVerified { get; set; } = false;
    }

    public class Bank
    {
        public string bankName { get; set; }
        public string bankCode { get; set; }
    }

    public class BankResult
    {
        public List<Bank> result { get; set; }
        public object errorMessage { get; set; }
        public object errorMessages { get; set; }
        public bool hasError { get; set; }
        public DateTime timeGenerated { get; set; }
    }

    public class CustomerRequestDto
    {
        public long Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string StateOfResidence { get; set; }
        public string LGA { get; set; }
        public ResponseModel Validate()
        {
            ResponseModel response = new ResponseModel();

            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                response.AddError("Phone number is required.");
                return response;
            }
        if (string.IsNullOrWhiteSpace(Email))
            {
                response.AddError("Email is required.");
                return response;
            }
        if (string.IsNullOrWhiteSpace(Password))
            {
                response.AddError("Password is required.");
                return response;
            }
        if (string.IsNullOrWhiteSpace(StateOfResidence))
            {
                response.AddError("State Of Residence is required.");
                return response;
            }
        if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                response.AddError("LGA is required.");
                return response;
            }
        
        if (!SimpleValidator.IsValidPhoneNumber(PhoneNumber))
            {
                response.AddError("Invalid phone number format.");
                return response;
            }
            
            if (!SimpleValidator.IsValidEmail(Email))
            {
                response.AddError("Invalid email format.");
                return response;
            }
            
            if (!SimpleValidator.IsValidPassword(Password))
            {
                response.AddError("Password should have a minimum length of 8, at least one lower case, one upper case, one number and one symbol.");
                return response;
            }

            return response;
        }
        
        public ResponseModel ValidateState()
        {
            ResponseModel response = new ResponseModel();

            var states = GetStates.LoadStates();

            var state = states.FirstOrDefault(x => x.state.ToLower() == StateOfResidence.ToLower());

            if (state is null)
            {
                response.AddError("Invalid state.");
                return response;
            }

            var IsLgaExists = GetStates.LgaExists(state.lgas, LGA);

            if (!IsLgaExists)
            {
                response.AddError("LGA does not exist in selected state of residence.");
                return response;
            }
            
            return response;
        }
    }

