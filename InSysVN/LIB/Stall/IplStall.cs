using System;
using System.Data;
using Dapper;

namespace LIB.Stall
{

    public class IplStall : BaseService<StallEntity, int>, IStall
    {
        public IplStall() {
        }
        public bool GetOrSetStall(string IpAddress,ref string StallName)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@IpAddress", IpAddress);
                param.Add("@StallName","",DbType.String,ParameterDirection.Output,50);
                if (unitOfWork.ProcedureExecute("sp_Stall_GetOrSetStall", param))
                {
                    StallName = param.Get<string>("@StallName");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }
    }
}
