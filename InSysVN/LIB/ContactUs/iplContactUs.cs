using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.ContactUs
{
    public class iplContactUs: BaseService<ContactUsEntity, int>, IContactUs
    {
        public long InsertContact(ContactUsEntity entity)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                p.Add("@FullName", entity.FullName);
                p.Add("@Email", entity.Email);
                p.Add("@Phone", entity.Phone);
                p.Add("@Content", entity.Content);
                var flag = unitOfWork.ProcedureExecute("sp_ContactUs_Insert",p);
                if (flag)
                    return p.Get<long>("@Id");
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return 0;
            }
        }
    }
}
