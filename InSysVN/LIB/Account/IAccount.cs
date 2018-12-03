using LIB.DataRequests;
using System;
using System.Collections.Generic;

namespace LIB
{
    public interface IAccount : IBaseServices<UserEntity, int>
    {
        bool UpdatePassword(AccountChangePasswordModel model);
        UserEntity GetProfile(int UserId);
    }
}
