using LIB.DataRequests;
using System;
using System.Collections.Generic;

namespace LIB.Stall
{
    public interface IStall : IBaseServices<StallEntity, int>
    {
        bool GetOrSetStall(string IpAddress, ref string StallName);
    }
}
