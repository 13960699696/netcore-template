using CORE.Entity.test;
using CORE.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CORE.Business.test
{
    public interface Ibase_buildtestBusiness
    {
        Task<PageResult<base_buildtest>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<base_buildtest> GetTheDataAsync(string id);
        Task AddDataAsync(base_buildtest data);
        Task UpdateDataAsync(base_buildtest data);
        Task DeleteDataAsync(List<string> ids);
    }
}