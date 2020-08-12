using CORE.Business.test;
using CORE.Entity.test;
using CORE.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CORE.WEBERP.Controllers.test
{
    /// <summary>
    /// test控制器
    /// </summary>
    [Route("/test/[controller]/[action]")]
    public class base_buildtestController : BaseApiController
    {
        #region DI
        /// <summary>
        /// test控制器构造函数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_buildtestController(Ibase_buildtestBusiness base_buildtestBus)
        {
            _base_buildtestBus = base_buildtestBus;
        }

        Ibase_buildtestBusiness _base_buildtestBus { get; }

        #endregion

        #region 获取
        /// <summary>
        /// 获取test数据列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<base_buildtest>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _base_buildtestBus.GetDataListAsync(input);
        }
        /// <summary>
        /// 获取test数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<base_buildtest> GetTheData(IdInputDTO input)
        {
            return await _base_buildtestBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交
        /// <summary>
        /// 保存test数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SaveData(base_buildtest data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _base_buildtestBus.AddDataAsync(data);
            }
            else
            {
                await _base_buildtestBus.UpdateDataAsync(data);
            }
        }
        /// <summary>
        /// 删除test数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _base_buildtestBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}