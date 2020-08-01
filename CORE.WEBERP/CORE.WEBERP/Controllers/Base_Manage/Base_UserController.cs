using System.Collections.Generic;
using System.Threading.Tasks;
using CORE.Business.Base_Manage;
using CORE.Entity;
using CORE.Util;
using Microsoft.AspNetCore.Mvc;

namespace CORE.WEBERP.Controllers.Base_Manage
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_UserController : BaseApiController
    {
        #region DI
        /// <summary>
        /// 用户控制器构造函数
        /// </summary>
        /// <param name="userBus"></param>
        public Base_UserController(IBase_UserBusiness userBus)
        {
            _userBus = userBus;
        }

        IBase_UserBusiness _userBus { get; }

        #endregion

        #region 获取
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="input">根据[(bool)是否延迟查询，(string)用户id，(string)关键字]来查询</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Base_UserDTO>> GetDataList(PageInput<Base_UsersInputDTO> input)
        {
            return await _userBus.GetDataListAsync(input);
        }
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="input">用户id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_UserDTO> GetTheData(IdInputDTO input)
        {
            return await _userBus.GetTheDataAsync(input.id) ?? new Base_UserDTO();
        }
        /// <summary>
        /// 获取用户列表(id范围)
        /// </summary>
        /// <param name="input">根据[(List)用户id，(string)关键字]来查询</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<SelectOption>> GetOptionList(OptionListInputDTO input)
        {
            return await _userBus.GetOptionListAsync(input);
        }

        #endregion

        #region 提交
        /// <summary>
        /// 保存、编辑用户数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SaveData(UserEditInputDTO input)
        {
            if (!input.newPwd.IsNullOrEmpty())
                input.Password = input.newPwd.ToMD5String();
            if (input.Id.IsNullOrEmpty())
            {
                InitEntity(input);

                await _userBus.AddDataAsync(input);
            }
            else
            {
                await _userBus.UpdateDataAsync(input);
            }
        }
        /// <summary>
        /// 批量删除用户数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _userBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}
