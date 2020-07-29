using CORE.Entity;
using CORE.Entity.Base_Manage;
using CORE.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CORE.Business.Base_Manage
{
    public interface IBase_UserBusiness
    {
        Task<PageResult<Base_UserDTO>> GetDataListAsync(PageInput<Base_UsersInputDTO> input);
        Task<List<SelectOption>> GetOptionListAsync(OptionListInputDTO input);
        Task AddDataAsync(UserEditInputDTO input);
        Task<Base_UserDTO> GetTheDataAsync(string id);
        Task UpdateDataAsync(UserEditInputDTO input);
        Task DeleteDataAsync(List<string> ids);
    }

    [Map(typeof(Base_User))]
    public class UserEditInputDTO : Base_User
    {
        public string newPwd { get; set; }
        public List<string> RoleIdList { get; set; }
    }

    public class Base_UsersInputDTO
    {
        public bool all { get; set; }
        public string userId { get; set; }
        public string keyword { get; set; }
    }
}