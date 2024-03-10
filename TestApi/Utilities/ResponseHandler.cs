using System.Collections.Generic;
using System;

namespace TestApi.Utilities
{
    public class UResponseHandler
    {
        private ResponseParam response;

        public UResponseHandler()
        {
            response = new ResponseParam
            {
                Successful = false,
                ResponseCode = ResponseCodes.NOT_PROCESSED,
                Message = null,
                Data = null
            };
        }

        public void CommitResponse(object data = null)
        {
            response.Successful = true;
            response.ResponseCode = ResponseCodes.SUCCESS;
            response.Message = "Successful";
            response.Data = data ?? new List<object>();

        }

        public void CommitError(string message, object data = null)
        {
            response.Successful = false;
            response.ResponseCode = ResponseCodes.UNSUCCESSFUL;
            response.Message = message;
            response.Data = data ?? new List<object>();

        }

        public void CommitForbidden(string message, object data = null)
        {
            response.Successful = false;
            response.ResponseCode = ResponseCodes.FORBIDDEN;
            response.Message = message;
            response.Data = data ?? new List<object>();

        }

        public void HandleException(Exception exception, string customMessage = null)
        {
            const string ERR_MESSAGE = "An error occurred.";

            response.Successful = false;
            response.ResponseCode = ResponseCodes.ERROR;
            response.Message = string.IsNullOrWhiteSpace(customMessage) ?
                $"{ERR_MESSAGE} | {exception.Message}" :
                $"An exception occurred. | {customMessage}";
            response.Data = new List<object>();

        }

        public ResponseParam GetResponse()
        {
            return response;
        }
    }

    public class ResponseParam
    {
        public bool Successful { get; set; }
        public string ResponseCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public class ResponseCodes
    {
        public static readonly string SUCCESS = "00";
        public static readonly string NOT_PROCESSED = "01";
        public static readonly string UNSUCCESSFUL = "02";
        public static readonly string FORBIDDEN = "03";
        public static readonly string ERROR = "99";
    }
}
