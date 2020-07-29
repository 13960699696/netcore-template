using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace CORE.WEBERP.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidFilterAttribute : BaseActionFilterAsync
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var msgList = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);

                context.Result = Error(string.Join(",", msgList));
            }

            await Task.CompletedTask;
        }
    }
}
