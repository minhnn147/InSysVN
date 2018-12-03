using Dapper;
using Framework.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LIB
{
    public class IplCategory : BaseService<CategoryEntity, long>, ICategory
    {
        #region Methods
        public List<CategoryEntity> GetAllWithLevel(string txtSearch)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@txtSearch", txtSearch);
                List<CategoryEntity> data = unitOfWork.Procedure<CategoryEntity>("sp_Category_GetAllWithLevel", param).ToList();
                return data;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
        public List<CategoryEntity> GetAllData()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                List<CategoryEntity> list = unitOfWork.Procedure<CategoryEntity>("sp_Category_GetAllData", param).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
        public List<CategoryEntity> GetDataWithPage(Select2Param obj, ref int total)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@search", obj.term);
                param.Add("@page", obj.page);
                param.Add("@total", DbType.Int32, direction: ParameterDirection.Output);
                List<CategoryEntity> data = unitOfWork.Procedure<CategoryEntity>("sp_Category_GetDataWithPage", param).ToList();
                total = param.Get<int>("total");
                return data;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

        }
        public CategoryEntity Insert_Update(CategoryEntity category)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", category.Id);
                param.Add("@Name", category.Name);
                param.Add("@Description", category.Description);
                return unitOfWork.Procedure<CategoryEntity>("sp_Category_Insert_Update", param).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CategoryEntity GetByID(int Id)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", Id);
                return unitOfWork.Procedure<CategoryEntity>("sp_Category_GetById", param).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CategoryEntity GetByCode(string Code)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Code", Code);
                return unitOfWork.Procedure<CategoryEntity>("sp_Category_GetByCode", param).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteById(int Id)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", Id);
                return unitOfWork.ProcedureExecute("sp_Category_Delete", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool CheckExistCode(int? Id,string Code)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", Id);
                param.Add("@Code", Code);
                if (unitOfWork.Procedure<int>("sp_Category_CheckExitstCode", param).SingleOrDefault() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a record to the Category table.
        /// </summary>

        /// <summary>
        /// Updates a record in the Category table.
        /// </summary>


        public bool UpdateCateImage(string base64Image, int CateId, string PathServer, string PathFile)
        {
            try
            {
                UploadFile.UploadImageWithBase64(base64Image, PathServer, PathFile);
                var param = new DynamicParameters();
                param.Add("@Id", CateId);
                param.Add("@pathimage", PathFile);
                return unitOfWork.ProcedureExecute("SP_Category_UpdateImage", param);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }

        }
        public bool Update(int id)
        {
            bool res = false;
            try
            {
                var p = new DynamicParameters();
                p.Add("@id", id);
                res = (bool)unitOfWork.ProcedureExecute("ptgroup_Category_ChangeStatus", p);
                return res;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        public bool SetOnHomePage(int id, int value)
        {
            bool res = false;
            try
            {
                var p = new DynamicParameters();
                p.Add("@id", id);
                p.Add("@value", value);
                res = (bool)unitOfWork.ProcedureExecute("ptgroupCategory_SetOnHomePage", p);
                return res;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// Deletes a record from the Category table by its primary key.
        /// </summary>
        public bool Delete(int id)
        {
            try
            {
                bool res = false;
                var p = new DynamicParameters();
                p.Add("@Id", id);
                res = (bool)unitOfWork.ProcedureExecute("ptgroupCategory_Delete", p);
                return res;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Selects a single record from the Category table.
        /// </summary>
        public CategoryEntity ViewDetail(int id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", id);
                var data = unitOfWork.Procedure<CategoryEntity>("ptgroupCategory_ViewDetail", p).SingleOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

        }
        public CategoryEntity ViewDetail(string Name, int ParentId)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Name", Name);
                p.Add("@ParentId", ParentId);
                var data = unitOfWork.Procedure<CategoryEntity>("ptgroup_Category_ViewByName", p).SingleOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

        }
        /// <summary>
        /// Selects all records from the Category table.
        /// </summary>
        public List<CategoryEntity> ListAll()
        {
            try
            {
                var data = unitOfWork.Procedure<CategoryEntity>("ptgroupCategory_ListAll");
                return data.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
        public List<CategoryEntity> ListAllNotTree()
        {
            try
            {
                var data = unitOfWork.Procedure<CategoryEntity>("ptgroupCategory_ListAllNotTree");
                return data.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }


        public List<CategoryEntity> ListConfig()
        {
            try
            {
                var data = unitOfWork.Procedure<CategoryEntity>("ptgroupCategory_ListConfig");
                return data.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }



        /// <summary>
        /// Selects all records from the Category table.
        /// </summary>
        public List<CategoryEntity> ListAllPaging(int pageIndex, int pageSize, ref int totalRow)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@pageIndex", pageIndex);
                p.Add("@pageSize", pageSize);
                p.Add("@totalRow", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var data = unitOfWork.Procedure<CategoryEntity>("ptgroupCategory_ListAllPaging", p);
                totalRow = p.Get<int>("@totalRow");
                return data.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
        public List<CategoryEntity> ParentCate()
        {
            try
            {
                var data = unitOfWork.Procedure<CategoryEntity>("Sp_Category_GetParent");
                return data.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
        public List<CategoryEntity> ChildCate(int id = 0)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@parentId", id);
                var data = unitOfWork.Procedure<CategoryEntity>("ptgroupCategory_GetChildCate", p);
                return data.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
        public List<CategoryEntity> BindTreeview()
        {
            try
            {
                var p = new DynamicParameters();
                var data = unitOfWork.Procedure<CategoryEntity>("Category_ListAllCateByTreeView", p);
                return data.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
        public List<CategoryEntity> GetTopHot()
        {
            try
            {
                var data = unitOfWork.Procedure<CategoryEntity>("Sp_Category_GetTopHot");
                return data.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
        public List<CategoryEntity> GetHotProductTreeView()
        {
            try
            {
                var p = new DynamicParameters();
                var data = unitOfWork.Procedure<CategoryEntity>("Category_ListTreeViewWithCountHot", p);
                return data.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
        public List<CategoryEntity> SearchCategory(string keyword)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@keyword", keyword);
                var data = unitOfWork.Procedure<CategoryEntity>("SP_Category_Search", p).ToList();
                return data;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        public bool CheckExistCate(string check)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@check", check);
                var data = unitOfWork.Procedure<CategoryEntity>("SP_Category_CheckExits", p).ToList();
                if (data.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Saves a record to the Category table.
        /// </summary>
        #endregion
    }
}
