using CORE.WEBERP.Filter;
using Microsoft.AspNetCore.Mvc;

namespace CORE.WEBERP.Controllers
{
    /// <summary>
    /// Mvc对外接口基控制器
    /// </summary>
    [CheckJWT]
    [ApiController]
    [ApiLog]
    public class BaseApiController : BaseController
    {

    }
}
