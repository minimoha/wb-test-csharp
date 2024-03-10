

namespace TestApi.Models;

    public class ResponseModel
    {
        public bool Successful => string.IsNullOrWhiteSpace(Error);
        public string Error { get; set; }
        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();

        public void AddError(string Error)
        {
            this.Error = Error;
        }

        public void AddErrors(string[] _Errors)
        {
            foreach (string Error in _Errors)
                Errors.Add(new ValidationError(Error));
        }

        public void AddError(string Error, string Description)
        {
            Errors.Add(new ValidationError(Error, Description));
        }
    }

    public class ValidationError
    {
        public string Error { get; set; }
        public string Description { get; set; }

        public ValidationError()
        {
            Error = ErrorConstants.ERROR_MSG;
            Description = ErrorConstants.ERROR_MSG;
        }

        public ValidationError(string error, string description)
        {
            Error = error;
            Description = description;
        }

        public ValidationError(string error)
        {
            Error = error;
            Description = error;
        }
    }

