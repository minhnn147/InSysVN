using LIB.Model;
using System;
using System.Collections.Generic;

namespace LIB
{
    public interface ITemplate : IBaseServices<TemplateEntity, int>
    {
        List<TemplateEntity> GetData();
    }
}
