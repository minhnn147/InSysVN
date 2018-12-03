using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Images
{
    public class IplImages : BaseService<ImagesEntity,long>, IImages
    {
        public bool InsertOrUpdate(ImagesEntity images)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", images.ImagePath);
                return unitOfWork.ProcedureExecute("SP_Images_Insert", param);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }
    }
}
