using LIB.Model;
using LIB.RoleModule;
using System;
using System.Collections.Generic;

namespace LIB
{
	public interface IRole:IBaseServices<RoleEntity, int>{
        List<RoleModuleEntity> GetRoleModuleByRole(string roleIds);
        List<RoleModuleEntity> GetRoleModuleByUserIds(string UserIds);
        List<RoleEntity> UserInRole(int userId);
        bool UpdateRight(int roleId, string xml);
        List<RoleEntity> GetRolesByLevel(int level);
    }
}
