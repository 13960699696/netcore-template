using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORE.Business.Base_Manage;
using CORE.Entity;
using CORE.Entity.Base_Manage;
using CORE.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CORE.WEBERP.Controllers.Base_Manage
{
    /// <summary>
    /// 系统权限
    /// </summary>
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_ActionController : BaseApiController
    {
        #region DI
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionBus"></param>
        public Base_ActionController(IBase_ActionBusiness actionBus)
        {
            _actionBus = actionBus;
        }

        IBase_ActionBusiness _actionBus { get; }

        #endregion

        #region 获取
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_Action> GetTheData(IdInputDTO input)
        {
            return (await _actionBus.GetTheDataAsync(input.id)) ?? new Base_Action();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_Action>> GetPermissionList(Base_ActionsInputDTO input)
        {
            input.types = new ActionType[] { Entity.ActionType.权限 };

            return await _actionBus.GetDataListAsync(input);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_Action>> GetAllActionList()
        {
            return await _actionBus.GetDataListAsync(new Base_ActionsInputDTO
            {
                types = new ActionType[] { ActionType.菜单, ActionType.页面, ActionType.权限 }
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_ActionDTO>> GetMenuTreeList(Base_ActionsInputDTO input)
        {
            input.selectable = true;
            input.types = new ActionType[] { ActionType.菜单, ActionType.页面 };

            return await _actionBus.GetTreeDataListAsync(input);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_ActionDTO>> GetActionTreeList(Base_ActionsInputDTO input)
        {
            input.selectable = false;

            return await _actionBus.GetTreeDataListAsync(input);
        }

        #endregion

        #region 提交
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SaveData(ActionEditInputDTO input)
        {
            if (input.Id.IsNullOrEmpty())
            {
                InitEntity(input);

                await _actionBus.AddDataAsync(input);
            }
            else
            {
                await _actionBus.UpdateDataAsync(input);
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
            await _actionBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}
