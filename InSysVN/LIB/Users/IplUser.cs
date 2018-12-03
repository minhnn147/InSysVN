using System.Collections.Generic;
using System.Data;
using System.Linq;
using LIB.DataRequests;
using Dapper;
using System;

namespace LIB
{
    public class IplUser : BaseService<UserEntity, int>, IUser
    {
        public IplUser() { }
        public UserEntity Login(string userName)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@UserName", userName);
                return unitOfWork.Procedure<UserEntity>("sp_Users_Login", p).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

        }
        public List<UserEntity> GetByPaging(PagingRequest pagingMessage, ref int totalRecord)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@pageIndex", pagingMessage.PageIndex);
                p.Add("@pageSize", pagingMessage.PageSize);
                p.Add("@keyword", pagingMessage.Keyword);
                p.Add("@filter", pagingMessage.Filter);
                p.Add("@sort", pagingMessage.Sort);
                p.Add("@totalRecord", DbType.Int32, direction: ParameterDirection.Output);
                var entity = unitOfWork.Procedure<UserEntity>("spUsers_GetByPaging", p).ToList();
                totalRecord = p.Get<int>("@totalRecord");
                return entity;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

        }
        public bool Delete(int id, ref string message)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", id);
                return unitOfWork.ProcedureExecute("sp_Users_Delete", param);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }
        public bool UpdatePassword(UserChangePassModel model)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Password", model.PasswordNew);
                param.Add("@UserId", model.UserId);
                return unitOfWork.ProcedureExecute("sp_Users_UpdatePassword", param);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }

        }
        public List<UserEntity> GetDataUsers(bootstrapTableParam obj, int? RoleId, int RoleLevel, ref int totalRow)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@txtSearch", obj.search);
                param.Add("@pageNumber", obj.pageNumber());
                param.Add("@pageSize", obj.pageSize());
                param.Add("@order", obj.order);
                param.Add("@sort", obj.sort);
                param.Add("@RoleId", RoleId);
                param.Add("@RoleLevel", RoleLevel);
                param.Add("@totalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
                List<UserEntity> lst = unitOfWork.Procedure<UserEntity>("sp_Users_GetData", param).ToList();
                totalRow = param.Get<int>("@totalRecord");
                return lst;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

        }
        public List<UserEntity> AutoCompleteUsers(Select2Param obj, ref int total)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@search", obj.term);
                param.Add("@page", obj.page);
                param.Add("@total", DbType.Int32, direction: ParameterDirection.Output);
                List<UserEntity> lstUsers = unitOfWork.Procedure<UserEntity>("sp_Users_GetDataAutoComplete", param).ToList();
                total = param.Get<int>("total");
                return lstUsers;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

        }
        public UserEntity GetUserByUserName(string UserName)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserName", UserName);
                return unitOfWork.Procedure<UserEntity>("sp_Users_GetByUserName", param).SingleOrDefault();
            }
            catch (Exception ex)
            {

                Log.Error(ex);
                return null;
            }

        }
        public string CreateCode()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Prefix", Enum.PrefixCode.User);
                return unitOfWork.Procedure<string>("sp_Users_CreateCode", param).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

        }
        public UserEntity GetUserByID(int id)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", id);
                return unitOfWork.Procedure<UserEntity>("sp_Users_GetById", param).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

        }
        public UserEntity InsertOrUpdate(UserEntity user)
        {
            try
            {
                string xml = XMLHelper.SerializeXML<UserEntity>(user).Replace("xsi:nil=\"true\"", "");
                DynamicParameters param = new DynamicParameters();
                param.Add("@strxml", xml);
                param.Add("@UserId", user.Id);
                param.Add("@Prefix", Enum.PrefixCode.User);
                return unitOfWork.Procedure<UserEntity>("sp_Users_InsertOrUpdate", param).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

        }
        public bool UpdateAvatar(string base64Image, int UserId, string PathServer, string PathFile)
        {
            try
            {
                UploadFile.UploadImageWithBase64(base64Image, PathServer, PathFile);
                var param = new DynamicParameters();
                param.Add("@Id", UserId);
                param.Add("@srcAvatar", PathFile);
                return unitOfWork.ProcedureExecute("sp_Users_UpdateAvatar", param);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }

        }
    }
}
