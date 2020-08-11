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
    /// 数据库控制器
    /// </summary>
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_DbLinkController : BaseApiController
    {
        #region DI
        /// <summary>
        /// 数据库控制器构造函数
        /// </summary>
        /// <param name="dbLinkBus"></param>
        public Base_DbLinkController(IBase_DbLinkBusiness dbLinkBus)
        {
            _dbLinkBus = dbLinkBus;
        }

        IBase_DbLinkBusiness _dbLinkBus { get; }

        #endregion

        #region 获取
        /// <summary>
        /// 获取数据库连接数据列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult<List<Base_DbLink>>> GetDataList(PageInput input)
        {
            return await _dbLinkBus.GetDataListAsync(input);
        }
        /// <summary>
        /// 获取数据库连接数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_DbLink> GetTheData(IdInputDTO input)
        {
            return await _dbLinkBus.GetTheDataAsync(input.id) ?? new Base_DbLink();
        }

        #endregion

        #region 提交

        /// <summary>
        /// 保存数据库连接数据
        /// </summary>
        /// <param name="theData">保存的数据</param>
        [HttpPost]
        public async Task SaveData(Base_DbLink theData)
        {
            if (theData.Id.IsNullOrEmpty())
            {
                InitEntity(theData);

                await _dbLinkBus.AddDataAsync(theData);
            }
            else
            {
                await _dbLinkBus.UpdateDataAsync(theData);
            }
        }

        /// <summary>
        /// 删除数据库连接数据
        /// </summary>
        /// <param name="ids">id数组,JSON数组</param>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _dbLinkBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}
