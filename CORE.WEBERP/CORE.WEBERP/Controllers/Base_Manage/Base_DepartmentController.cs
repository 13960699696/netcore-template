using System.Collections.Generic;
using System.Threading.Tasks;
using CORE.Business.Base_Manage;
using CORE.Entity.Base_Manage;
using CORE.Util;
using Microsoft.AspNetCore.Mvc;

namespace CORE.WEBERP.Controllers.Base_Manage
{
    /// <summary>
    /// 部门控制器
    /// </summary>
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_DepartmentController : BaseApiController
    {
        #region DI
        /// <summary>
        /// 部门控制器构造函数
        /// </summary>
        /// <param name="departmentBus"></param>
        public Base_DepartmentController(IBase_DepartmentBusiness departmentBus)
        {
            _departmentBus = departmentBus;
        }

        IBase_DepartmentBusiness _departmentBus { get; }

        #endregion

        #region 获取
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_Department> GetTheData(IdInputDTO input)
        {
            return await _departmentBus.GetTheDataAsync(input.id) ?? new Base_Department();
        }
        /// <summary>
        /// 获取部门目录树
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_DepartmentTreeDTO>> GetTreeDataList(DepartmentsTreeInputDTO input)
        {
            return await _departmentBus.GetTreeDataListAsync(input);
        }

        #endregion

        #region 提交
        /// <summary>
        /// 保存，编辑部门数据
        /// </summary>
        /// <param name="theData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SaveData(Base_Department theData)
        {
            if (theData.Id.IsNullOrEmpty())
            {
                InitEntity(theData);

                await _departmentBus.AddDataAsync(theData);
            }
            else
            {
                await _departmentBus.UpdateDataAsync(theData);
            }
        }
        /// <summary>
        /// 批量删除部门数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _departmentBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}
