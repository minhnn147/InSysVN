using LIB.Model;
using System;
using System.Collections.Generic;

namespace LIB
{
    public interface IModule : IBaseServices<ModuleEntity, int>
    {
        List<ModuleTreeViewModel> ModuleGetListTreeView(string spaceTab = "====/");
        bool UpdateModuleIdAndSort(string xml);
        List<ModuleEntity> GetListModuleByRoleId(int RoleId);
        List<ModuleEntity> GetDataModule();
    }
}
