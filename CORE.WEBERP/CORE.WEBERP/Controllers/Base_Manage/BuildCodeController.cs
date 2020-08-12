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
    /// 代码生成器
    /// </summary>
    [Route("/Base_Manage/[controller]/[action]")]
    public class BuildCodeController : BaseApiController
    {
        #region DI
        /// <summary>
        /// 代码生成器控制器
        /// </summary>
        /// <param name="buildCodeBus"></param>
        public BuildCodeController(IBuildCodeBusiness buildCodeBus)
        {
            _buildCodeBus = buildCodeBus;
        }

        IBuildCodeBusiness _buildCodeBus { get; }

        #endregion
        /// <summary>
        /// 获取所有数据库连接
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<Base_DbLink> GetAllDbLink()
        {
            return _buildCodeBus.GetAllDbLink();
        }
        /// <summary>
        /// 获取数据库表列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<DbTableInfo> GetDbTableList(DbTablesInputDTO input)
        {
            return _buildCodeBus.GetDbTableList(input.linkId);
        }
        /// <summary>
        /// 代码生成
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        public void Build(BuildInputDTO input)
        {
            _buildCodeBus.Build(input);
        }
    }
}
