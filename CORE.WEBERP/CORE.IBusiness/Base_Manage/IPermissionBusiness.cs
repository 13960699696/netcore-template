using System.Collections.Generic;
using System.Threading.Tasks;

namespace CORE.Business.Base_Manage
{
    public interface IPermissionBusiness
    {
        Task<List<string>> GetUserPermissionValuesAsync(string userId);
        Task<List<Base_ActionDTO>> GetUserMenuListAsync(string userId);
    }
}
