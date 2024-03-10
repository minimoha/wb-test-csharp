

namespace TestApi.Utilities;

    public class RequestResponseLoggingMiddleware
    {
        private HttpResponse httpResponse = null;
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            requestStream.Position = 0;
            string text = await new StreamReader(requestStream).ReadToEndAsync();
            string queryParameter = context.Request.QueryString.ToString();

            string formattedQueryParameter = !string.IsNullOrWhiteSpace(queryParameter) ? queryParameter?.Split('?')[1]?.Replace("&", " ") : string.Empty;

            text = $"{formattedQueryParameter} {text}";
            _logger.LogInformation($"Http Request :  {context.Request.Method} {context.Request.Path} {text}");
            context.Request.Body.Position = 0;
        }

        private async Task LogResponse(HttpContext context)
        {
            httpResponse = context.Response;
            var originalBodyStream = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(context.Response.Body).ReadToEndAsync();

            responseBody.Seek(0, SeekOrigin.Begin);


            await responseBody.CopyToAsync(originalBodyStream);


            _logger.LogInformation($"Http Response : {context.Request.Method} {context.Request.Path} {text}");
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }


