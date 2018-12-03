using Dapper;
using System;

namespace LIB
{
    public class IplAccount : BaseService<UserEntity, int>, IAccount
    {
        public IplAccount() { }
        public bool UpdatePassword(AccountChangePasswordModel model)
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
        public UserEntity GetProfile(int UserId)
        {
            try
            {
                return new UserEntity();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
