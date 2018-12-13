using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB
{
    public class IplNews : BaseService<NewsEntity, long>, INews
    {
        public List<NewsEntity> GetAllNews()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                List<NewsEntity> list = unitOfWork.Procedure<NewsEntity>("sp_News_GetAllData", param).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
        public NewsEntity GetNewsByID(int Id)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("@Id", Id);
            return unitOfWork.Procedure<NewsEntity>("sp_News_GetById", p).SingleOrDefault();
        }
        public bool CheckByTitle(string title)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("@title", title);
            var data = unitOfWork.Procedure<NewsEntity>("sp_News_CheckByTitle",p).ToList();
            if (data != null && data.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public NewsEntity Insert_Update(NewsEntity newsentity)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@Id", newsentity.ID);
                p.Add("@Title", newsentity.Title);
                p.Add("@Content", newsentity.Content);
                return unitOfWork.Procedure<NewsEntity>("sp_News_InsertUpdate", p).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
        public bool UpdateNewsImage(string base64Image, long NewsId, string PathServer, string PathFile)
        {
            try
            {
                UploadFile.UploadImageWithBase64(base64Image, PathServer, PathFile);
                var param = new DynamicParameters();
                param.Add("@Id", NewsId);
                param.Add("@pathimage", PathFile);
                return unitOfWork.ProcedureExecute("SP_News_UpdateImage", param);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }

        }
        public bool UpdateIsActive(int newsId)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@NewsId", newsId);
                var flag = unitOfWork.ProcedureExecute("sp_News_UpdateIsActive", p);
                return flag;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }
        public bool DeleteByNewsId(int id)
        {
            try
            {
                bool res = false;
                var p = new DynamicParameters();
                p.Add("@Id", id);
                res = (bool)unitOfWork.ProcedureExecute("sp_News_Delete", p);
                return res;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
    }
}
