using System.Collections.Generic;
using System.Threading.Tasks;
using CORE.Business.Base_Manage;
using CORE.IBusiness;
using CORE.WEBERP.Filter;
using Microsoft.AspNetCore.Mvc;

namespace CORE.WEBERP.Controllers.Base_Manage
{
    /// <summary>
    /// 首页控制器
    /// </summary>
    [Route("/Base_Manage/[controller]/[action]")]
    public class HomeController : BaseApiController
    {
        readonly IHomeBusiness _homeBus;
        readonly IPermissionBusiness _permissionBus;
        readonly IOperator _operator;
        /// <summary>
        /// 首页控制器构造函数
        /// </summary>
        /// <param name="homeBus"></param>
        /// <param name="permissionBus"></param>
        /// <param name="operator"></param>
        public HomeController(
            IHomeBusiness homeBus,
            IPermissionBusiness permissionBus,
            IOperator @operator
            )
        {
            _homeBus = homeBus;
            _permissionBus = permissionBus;
            _operator = @operator;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="input">用户账号，密码</param>
        /// <returns>返回Token</returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<string> SubmitLogin(LoginInputDTO input)
        {
            return await _homeBus.SubmitLoginAsync(input);
        }
        /// <summary>
        /// 获取权限信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> GetOperatorInfo()
        {
            var theInfo = _operator.Property;
            var permissions = await _permissionBus.GetUserPermissionValuesAsync(_operator.UserId);
            var resObj = new
            {
                UserInfo = theInfo,
                Permissions = permissions
            };

            return resObj;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input">用户旧密码，新密码</param>
        /// <returns></returns>
        [HttpPost]
        public async Task ChangePwd(ChangePwdInputDTO input)
        {
            await _homeBus.ChangePwdAsync(input);
        }
        /// <summary>
        /// 获取权限菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_ActionDTO>> GetOperatorMenuList()
        {
            return await _permissionBus.GetUserMenuListAsync(_operator.UserId);
        }
    }
}
