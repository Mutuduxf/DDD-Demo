using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ServiceHost
{
    public class UowMiddleware
    {
        private readonly RequestDelegate _next;

        public UowMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, DbContext dbContext)
        {
            await _next(context);
            await dbContext.SaveChangesAsync();
        }
    }
}