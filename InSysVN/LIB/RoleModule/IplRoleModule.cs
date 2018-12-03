using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using LIB.RoleModule;

namespace LIB
{
    public class IplRoleModule : BaseService<RoleModuleEntity, int>, IRoleModule
    {
        public IplRoleModule() { }

        public List<RoleModuleEntity> GetDataRoleModule_ByRoleId(long RoleId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@RoleId", RoleId);
                return unitOfWork.Procedure<RoleModuleEntity>("sp_RoleModule_ByRoleId", param).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

        }
        public List<RoleModuleEntity> ListRoleModuleByRole(int roleId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@roleId", roleId);
                return unitOfWork.Procedure<RoleModuleEntity>("sp_GetAllRoleForModule", param).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

        }
        public bool UpdateRoleModule(int roleId, int moduleId, bool status, string name)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@roleId", roleId);
                param.Add("@moduleId", moduleId);
                param.Add("@action", name);
                param.Add("@access", status);
                return unitOfWork.ProcedureExecute("sp_RoleModule_UpdateDynamic", param);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }

        }
        public bool Save(string xml)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@xml", xml);
                return unitOfWork.ProcedureExecute("sp_RoleModule_Update", param);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }
        public bool AddModuleToRole(int RoleId, string ModulesId)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@RoleId", RoleId);
                param.Add("@ModulesId", ModulesId);
                return unitOfWork.ProcedureExecute("sp_RoleModule_UpdateModule", param);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }

        }
    }
}
