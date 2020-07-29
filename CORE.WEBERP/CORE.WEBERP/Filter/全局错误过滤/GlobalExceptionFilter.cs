using CORE.Util;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CORE.WEBERP.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class GlobalExceptionFilter : BaseActionFilterAsync, IAsyncExceptionFilter
    {
        readonly ILogger _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            Exception ex = context.Exception;

            if (ex is BusException busEx)
            {
                _logger.LogInformation(busEx.Message);
                context.Result = Error(busEx.Message, busEx.ErrorCode);
            }
            else
            {
                _logger.LogError(ex, "");
                context.Result = Error(ex.Message);
            }

            await Task.CompletedTask;
        }
    }
}
