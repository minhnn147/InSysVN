using LIB.RoleModule;
using System;
using System.Collections.Generic;

namespace LIB
{
    public interface IRoleModule : IBaseServices<RoleModuleEntity, int>
    {
        List<RoleModuleEntity> ListRoleModuleByRole(int roleId);
        bool UpdateRoleModule(int roleId, int moduleId, bool status, string name);
        List<RoleModuleEntity> GetDataRoleModule_ByRoleId(long RoleId);
        bool Save(string xml);
        bool AddModuleToRole(int RoleId, string ModulesId);
    }
}
