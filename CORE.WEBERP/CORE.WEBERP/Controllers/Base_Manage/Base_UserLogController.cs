using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORE.Business.Base_Manage;
using CORE.Entity.Base_Manage;
using CORE.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CORE.WEBERP.Controllers.Base_Manage
{
    /// <summary>
    /// 系统日志控制器
    /// </summary>
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_UserLogController : BaseApiController
    {
        #region DI
        /// <summary>
        /// 系统日志控制器构造函数
        /// </summary>
        /// <param name="logBus"></param>
        public Base_UserLogController(IBase_UserLogBusiness logBus)
        {
            _logBus = logBus;
        }

        IBase_UserLogBusiness _logBus { get; }

        #endregion

        #region 获取
        /// <summary>
        /// 获取系统日志列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Base_UserLog>> GetLogList(PageInput<UserLogsInputDTO> input)
        {
            input.SortField = "CreateTime";
            input.SortType = "desc";

            return await _logBus.GetLogListAsync(input);
        }
        /// <summary>
        /// 获取系统日志类型列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<SelectOption> GetLogTypeList()
        {
            return EnumHelper.ToOptionList(typeof(UserLogType));
        }

        #endregion
    }
}
