using Kanbersky.Gumball.Core.Extensions;
using Kanbersky.Gumball.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanbersky.Gumball.Core.Middlewares
{
    public class ExceptionMiddleware
    {
        #region fields

        private readonly RequestDelegate _next;
        private static readonly ILogger Logger = Log.ForContext<ExceptionMiddleware>();

        #endregion

        #region ctor

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region methods

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await InternalServerError(context, ex, ex.Message);
            }
        }

        #endregion

        #region private methods

        private static async Task InternalServerError(HttpContext context, Exception exception, string data, string contentType = "text/plain")
        {
            await Task.Run(() =>
            {
                var request = context.Request;
                var encodedPathAndQuery = request.GetEncodedPathAndQuery();

                var logModel = new LoggerModel(request.Host.Host, request.Protocol, request.Method, request.Path, encodedPathAndQuery, StatusCodes.Status500InternalServerError)
                {
                    RequestHeaders = request.Headers.ToDictionary(x => x.Key, x => (object)x.Value.ToString()),
                    RequestBody = string.Empty,
                    Exception = exception,
                    Data = data
                };

                Logger.HandleLogging(logModel).Error(LoggerTemplates.Error);
            });

            context.Response.Clear();
            context.Response.ContentType = contentType;
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("Sunucuda beklenmeyen bir hata oluştu.", Encoding.UTF8);
        }

        #endregion
    }
}
