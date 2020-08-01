﻿using Coldairarrow.Util;
using CORE.Business.Cache;
using CORE.Entity;
using CORE.Entity.Base_Manage;
using CORE.IBusiness;
using CORE.Util;
using EFCore.Sharding;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CORE.Business
{
    /// <summary>
    /// 操作者
    /// </summary>
    public class Operator : IOperator, IScopedDependency
    {
        readonly IBase_UserCache _userCache;
        readonly IServiceProvider _serviceProvider;
        public Operator(IBase_UserCache userCache, IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _userCache = userCache;
            UserId = httpContextAccessor?.HttpContext?.Request.GetJWTPayload()?.UserId;
        }

        private Base_UserDTO _property;
        private object _lockObj = new object();

        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// 根据用户id获取用户信息
        /// </summary>
        public Base_UserDTO Property
        {
            get
            {
                if (UserId.IsNullOrEmpty())
                    return default;

                if (_property == null)
                {
                    lock (_lockObj)
                    {
                        if (_property == null)
                        {
                            _property = AsyncHelper.RunSync(() => _userCache.GetCacheAsync(UserId));
                        }
                    }
                }

                return _property;
            }
        }

        /// <summary>
        /// 判断是否为超级管理员
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            var role = Property.RoleType;
            if (UserId == GlobalData.ADMINID || role.HasFlag(RoleTypes.超级管理员))
                return true;
            else
                return false;
        }
        /// <summary>
        /// 写入用户操作日志
        /// </summary>
        /// <param name="userLogType"></param>
        /// <param name="msg"></param>
        public void WriteUserLog(UserLogType userLogType, string msg)
        {
            var log = new Base_UserLog
            {
                Id = IdHelper.GetId(),
                CreateTime = DateTime.Now,
                CreatorId = UserId,
                CreatorRealName = Property.RealName,
                LogContent = msg,
                LogType = userLogType.ToString()
            };

            Task.Factory.StartNew(async () =>
            {
                using (var scop = _serviceProvider.CreateScope())
                {
                    var db = scop.ServiceProvider.GetService<IDbAccessor>();
                    await db.InsertAsync(log);
                }
            }, TaskCreationOptions.LongRunning);
        }
    }
}