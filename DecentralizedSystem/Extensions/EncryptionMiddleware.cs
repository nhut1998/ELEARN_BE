using DecentralizedSystem.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace DecentralizedSystem.Extensions
{
    public class EncryptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        private readonly IRSAHelper _rSAHelper;
        private readonly IAESHelper _aESHelper;

        public EncryptionMiddleware(RequestDelegate next, IRSAHelper rSAHelper, IAESHelper aESHelper, IConfiguration configuration)
        {
            _next = next;
            _config = configuration;
            _rSAHelper = rSAHelper;
            _aESHelper = aESHelper;
        }

        // Whenever we call any action method then call this before call the action method
        public async Task Invoke(HttpContext httpContext)
        {
            var aESKey = httpContext.Request.Headers["aes-key"];
            if (!string.IsNullOrEmpty(aESKey))
            {
                var keyValue = _rSAHelper.Decrypt(aESKey);

                if (httpContext.Request.QueryString.HasValue)
                {
                    string decryptedString = _aESHelper.Decrypt(httpContext.Request.QueryString.Value.Substring(1), keyValue);
                    httpContext.Request.QueryString = new QueryString($"?{decryptedString}");
                }

                if (new string[] { "POST", "PUT", "PATCH" }.Any(a=> a == httpContext.Request.Method))
                {
                    using var reader = new StreamReader(httpContext.Request.Body);
                    var str = await reader.ReadToEndAsync();

                    var strDecrypt = _aESHelper.Decrypt(str, keyValue);

                    httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(strDecrypt));
                }
            }

            await _next(httpContext);
        }
    }
}