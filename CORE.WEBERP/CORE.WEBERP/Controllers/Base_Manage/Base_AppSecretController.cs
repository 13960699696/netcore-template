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
    /// 应用密钥
    /// </summary>
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_AppSecretController : BaseApiController
    {
        #region DI
        /// <summary>
        /// 应用密钥构造函数
        /// </summary>
        /// <param name="appSecretBus"></param>
        public Base_AppSecretController(IBase_AppSecretBusiness appSecretBus)
        {
            _appSecretBus = appSecretBus;
        }

        IBase_AppSecretBusiness _appSecretBus { get; }

        #endregion

        #region 获取
        /// <summary>
        /// 获取接口密钥列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Base_AppSecret>> GetDataList(PageInput<AppSecretsInputDTO> input)
        {
            return await _appSecretBus.GetDataListAsync(input);
        }
        /// <summary>
        /// 获取接口密钥
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_AppSecret> GetTheData(IdInputDTO input)
        {
            return await _appSecretBus.GetTheDataAsync(input.id) ?? new Base_AppSecret();
        }

        #endregion

        #region 提交

        /// <summary>
        /// 保存接口密钥
        /// </summary>
        /// <param name="theData">保存的数据</param>
        [HttpPost]
        public async Task SaveData(Base_AppSecret theData)
        {
            if (theData.Id.IsNullOrEmpty())
            {
                InitEntity(theData);

                await _appSecretBus.AddDataAsync(theData);
            }
            else
            {
                await _appSecretBus.UpdateDataAsync(theData);
            }
        }

        /// <summary>
        /// 批量删除接口密钥
        /// </summary>
        /// <param name="ids">id数组,JSON数组</param>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _appSecretBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}
