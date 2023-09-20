using DecentralizedSystem.Helpers;
using DecentralizedSystem.Services.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace DecentralizedSystem.Extensions
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILoggerManager _logger;
        public AuthMiddleware(IConfiguration configuration, RequestDelegate next, ILoggerManager logger)
        {
            _configuration = configuration;
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                var header = httpContext.Request.Headers["Authorization"];
                var decryptStr = EncryptionHelper.DecryptJWT(header, _configuration);
                if (decryptStr != null)
                {
                    httpContext.Request.Headers["Authorization"] = $"Bearer {decryptStr}";
                }
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await _next(httpContext);
            }
        }
    }
}
