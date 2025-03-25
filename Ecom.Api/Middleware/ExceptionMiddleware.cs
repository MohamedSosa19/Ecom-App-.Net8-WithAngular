using Ecom.Api.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _memory;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memory)
        {
            _next = next;
            _environment = environment;
            _memory = memory;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try

            {
                ApplaySecurity(context);
                if (IsRequestAllowed(context) == false)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var response = new 
                        ApiException((int)HttpStatusCode.TooManyRequests, "To Many Request Try Later");
                    await context.Response.WriteAsJsonAsync(response);
                }
                await _next(context);
            }
            catch (Exception ex)
            {

                context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = _environment.IsDevelopment() ?
                    new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new ApiException((int)HttpStatusCode.InternalServerError, ex.Message);

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
        private bool IsRequestAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress.ToString();
            var cashKey = $"Rate:{ip}";
            var dateNow = DateTime.Now;

            var (TimsTamp, Count) = _memory.GetOrCreate(cashKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (TimsTamp:dateNow,Count:0);
            });
            if(dateNow- TimsTamp < _rateLimitWindow)
            {
                if (Count > 8)
                {
                    return false;
                }
                _memory.Set(cashKey,(TimsTamp, Count + 1),_rateLimitWindow);

            }
            else
            {
                _memory.Set(cashKey, (TimsTamp, Count), _rateLimitWindow);

            }
            return true;
        }
        private void ApplaySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "noniff";
            context.Response.Headers["X-XSS-Protection"] = "1;mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";
        }
    }
}
