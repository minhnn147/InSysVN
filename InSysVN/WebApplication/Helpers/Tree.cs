using LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Helpers
{
    public partial class Tree
    {
        public static List<CategoryEntity> getChildList(List<CategoryEntity> data, int? IdParent)
        {
            List<CategoryEntity> con = new List<CategoryEntity>();
            //foreach (var itemParent in data)
            //{
            //    if (itemParent.ParentId == IdParent)
            //    {
            //        con.Add(itemParent);
            //        data = data.Where(m => m.Id != itemParent.Id).ToList();
            //        List<CategoryEntity> dataChild = getChildList(data, itemParent.Id.Value);
            //        foreach (var itemChild in dataChild)
            //        {
            //            itemChild.Name = "--- " + itemChild.Name;
            //            con.Add(itemChild);
            //        }
            //    }
            //}
            return con;
        }
    }
}