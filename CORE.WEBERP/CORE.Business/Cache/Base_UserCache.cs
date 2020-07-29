﻿using CORE.Business.Base_Manage;
using CORE.Entity;
using CORE.Util;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CORE.Business.Cache
{
    class Base_UserCache : BaseCache<Base_UserDTO>, IBase_UserCache, ITransientDependency
    {
        readonly IServiceProvider _serviceProvider;
        public Base_UserCache(IServiceProvider serviceProvider, IDistributedCache cache)
            : base(cache)
        {
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        /// 通过用户id获取用户信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Base_UserDTO</returns>
        protected override async Task<Base_UserDTO> GetDbDataAsync(string key)
        {
            PageInput<Base_UsersInputDTO> input = new PageInput<Base_UsersInputDTO>
            {
                Search = new Base_UsersInputDTO
                {
                    all = true,
                    userId = key
                }
            };
            var list = await _serviceProvider.GetService<IBase_UserBusiness>().GetDataListAsync(input);

            return list.Data.FirstOrDefault();
        }
    }
}
