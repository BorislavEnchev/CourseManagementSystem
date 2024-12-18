using Newtonsoft.Json;
using System.Net;

namespace CourseManagementSystem.API.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public CustomExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                // Custom exceptions
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError // Default to 500 if unexpected
            };

            var response = new
            {
                context.Response.StatusCode,
                exception.Message,
                // Include stack trace only in development mode
                StackTrace = _env.IsDevelopment() ? exception.StackTrace : null
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }


}
