

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
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string StateOfResidence { get; set; }
        [Required]
        public string LGA { get; set; }
        public ResponseModel Validate()
        {
            ResponseModel response = new ResponseModel();

            if (!PhoneNumberValidator.IsValidPhoneNumber(PhoneNumber))
            {
                response.AddError("Invalid phone number format.");
                return response;
            }
            
            if (!EmailValidator.IsValidEmail(Email))
            {
                response.AddError("Invalid email format.");
                return response;
            }
            
            if (!PasswordValidator.IsValidPassword(Password))
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

    public class PhoneNumberValidator
    {
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^(?:\+?234|0)?\d{10,13}$";

            Regex regex = new Regex(pattern);

            return regex.IsMatch(phoneNumber);
        }
    }

    public class EmailValidator
    {
        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }
    }

    public class PasswordValidator
    {
        public static bool IsValidPassword(string password)
        {
            // Define the regular expression pattern for password validation
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";

            // Create a Regex object with the pattern
            Regex regex = new Regex(pattern);

            // Check if the password matches the pattern
            return regex.IsMatch(password);
        }
    }

