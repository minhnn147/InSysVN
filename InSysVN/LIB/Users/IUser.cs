using LIB.DataRequests;
using System;
using System.Collections.Generic;

namespace LIB
{
    public interface IUser : IBaseServices<UserEntity, int>
    {
        UserEntity Login(string userName);
        List<UserEntity> GetByPaging(PagingRequest pagingMessage, ref int totalRecord);
        bool UpdatePassword(UserChangePassModel model);
        bool Delete(int id,ref string message);
        /// <summary>
        /// Lấy user theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserEntity GetUserByID(int id);
        List<UserEntity> GetDataUsers(bootstrapTableParam obj, int? roleId,int RoleLevel, ref int totalRow);
        UserEntity GetUserByUserName(string UserName);
        UserEntity InsertOrUpdate(UserEntity user);
        List<UserEntity> AutoCompleteUsers(Select2Param obj, ref int total);
        bool UpdateAvatar(string base64Image, int UserId, string PathServer, string PathFile);
        string CreateCode();
    }
}
