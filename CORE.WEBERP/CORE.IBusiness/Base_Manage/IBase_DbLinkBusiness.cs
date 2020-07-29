using CORE.Entity.Base_Manage;
using CORE.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CORE.Business.Base_Manage
{
    public interface IBase_DbLinkBusiness
    {
        Task<PageResult<Base_DbLink>> GetDataListAsync(PageInput input);
        Task<Base_DbLink> GetTheDataAsync(string id);
        Task AddDataAsync(Base_DbLink newData);
        Task UpdateDataAsync(Base_DbLink theData);
        Task DeleteDataAsync(List<string> ids);
    }
}