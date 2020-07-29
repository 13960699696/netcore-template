﻿using System;

namespace CORE.Entity
{
    /// <summary>
    /// 系统角色类型
    /// </summary>
    [Flags]
    public enum RoleTypes
    {
        超级管理员 = 1,
        数据运营部管理员 = 2
    }
}
