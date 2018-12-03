using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using LIB.Model;

namespace LIB
{
    public class IplTemplate : BaseService<TemplateEntity, int>, ITemplate
    {
        public List<TemplateEntity> GetData()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                return unitOfWork.Procedure<TemplateEntity>("sp_Template_GetData", param).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
           
        }
    }
}
