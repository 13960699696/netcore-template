using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORE.Business.Base_Manage;
using CORE.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CORE.WEBERP.Controllers.Base_Manage
{
    /// <summary>
    /// 应用密钥
    /// </summary>
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_RoleController : BaseApiController
    {
        #region DI
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleBus"></param>
        public Base_RoleController(IBase_RoleBusiness roleBus)
        {
            _roleBus = roleBus;
        }

        IBase_RoleBusiness _roleBus { get; }

        #endregion

        #region 获取
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Base_RoleInfoDTO>> GetDataList(PageInput<RolesInputDTO> input)
        {
            return await _roleBus.GetDataListAsync(input);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_RoleInfoDTO> GetTheData(IdInputDTO input)
        {
            return await _roleBus.GetTheDataAsync(input.id) ?? new Base_RoleInfoDTO();
        }

        #endregion

        #region 提交
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SaveData(Base_RoleInfoDTO input)
        {
            if (input.Id.IsNullOrEmpty())
            {
                InitEntity(input);

                await _roleBus.AddDataAsync(input);
            }
            else
            {
                await _roleBus.UpdateDataAsync(input);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _roleBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}
