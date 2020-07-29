using CORE.Entity.Base_Manage;
using CORE.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CORE.Business.Base_Manage
{
    public interface IBase_AppSecretBusiness
    {
        Task<PageResult<Base_AppSecret>> GetDataListAsync(PageInput<AppSecretsInputDTO> input);
        Task<Base_AppSecret> GetTheDataAsync(string id);
        Task<string> GetAppSecretAsync(string appId);
        Task AddDataAsync(Base_AppSecret newData);
        Task UpdateDataAsync(Base_AppSecret theData);
        Task DeleteDataAsync(List<string> ids);
    }

    public class AppSecretsInputDTO
    {
        public string keyword { get; set; }
    }
}