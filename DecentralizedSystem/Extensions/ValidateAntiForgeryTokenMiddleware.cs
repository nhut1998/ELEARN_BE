using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DecentralizedSystem.Extensions
{
    public class ValidateAntiForgeryTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAntiforgery _antiForgery;

        public ValidateAntiForgeryTokenMiddleware(RequestDelegate next, IAntiforgery antiForgery)
        {
            _next = next;
            _antiForgery = antiForgery;
        }

        public async Task Invoke(HttpContext context)
        {   
            //Don't validate POST for login
            if (context.Request.Path.Value.Contains("login"))
            {
                await _next(context);
                return;
            }

            if (HttpMethods.IsGet(context.Request.Method))
            {
                _antiForgery.GetAndStoreTokens(context);
            }

            if (HttpMethods.IsPost(context.Request.Method)
             || HttpMethods.IsPatch(context.Request.Method)
             || HttpMethods.IsPut(context.Request.Method))
            {
                await _antiForgery.ValidateRequestAsync(context);
            }
            await _next(context);
        }
    }
}
