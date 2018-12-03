using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Authorize;

namespace WebApplication.Helpers
{
    public class MenuModel
    {
        public int? Id { get; set; }
        public string Text { get; set; }
        public string URL { get; set; }
        public int? ParentId { get; set; }
        public string ClassIcon { get; set; }
        public int? IdCategory { get; set; }
        public int Category { get; set; }
        public string Shortcut { get; set; }
    }
    public class GenMenu
    {
        public static List<MenuModel> GetListMenu()
        {
            List<MenuModel> listMenu = new List<MenuModel>();
            var Account = HttpContext.Current.User as CustomPrincipal;
            listMenu.Add(new MenuModel()
            {
                Text = "DANH MỤC",
                IdCategory = 1
            });
            
            listMenu.Add(new MenuModel()
            {
                Text = "QUẢN TRỊ",
                IdCategory = 3
            });
            listMenu.Add(new MenuModel()
            {
                Text = "HỆ THỐNG",
                IdCategory = 4
            });
          
            
            
            if (AccesUser.checkAccess(new ActionModule[] { ActionModule.Category }, new ActionType[] { ActionType.View }, Account))
            {
                listMenu.Add(new MenuModel()
                {
                    Id = 3,
                    Text = "Quản lý danh mục",
                    URL = "/Category/Index",
                    ParentId = null,
                    ClassIcon = "fas fa-suitcase",
                    Category = 1
                });
            }              
            if (AccesUser.checkAccess(new ActionModule[] { ActionModule.Product }, new ActionType[] { ActionType.View }, Account))
            {
                listMenu.Add(new MenuModel()
                {
                    Id = 4,
                    Text = "Sản phẩm",
                    URL = "/Products/Index",
                    ParentId = null,
                    ClassIcon = "fas fa-th",
                    Category = 1
                });
            }
            if (AccesUser.checkAccess(new ActionModule[] { ActionModule.Product }, new ActionType[] { ActionType.View }, Account))
            {
                listMenu.Add(new MenuModel()
                {
                    Id = 4,
                    Text = "Sản phẩm",
                    URL = "/Products/Index",
                    ParentId = null,
                    ClassIcon = "fas fa-th",
                    Category = 1
                });
            }
            //if (AccesUser.checkAccess(new ActionModule[] { ActionModule.Customer }, new ActionType[] { ActionType.View }, Account))
            //{
            //    listMenu.Add(new MenuModel()
            //    {
            //        Id = 5,
            //        Text = "Quản lý khách hàng",
            //        URL = null,
            //        ParentId = null,
            //        ClassIcon = "fas fa-address-card",
            //        Category = 1
            //    });
            //    listMenu.Add(new MenuModel()
            //    {
            //        Id = 16,
            //        Text = "Danh sách khách hàng",
            //        URL = "/Customer/Index",
            //        ParentId = 5,
            //        ClassIcon = "",
            //        Category = 1
            //    });
            //    listMenu.Add(new MenuModel()
            //    {
            //        Id = 17,
            //        Text = "Thẻ thành viên",
            //        URL = "/Cardnumbers/Index",
            //        ParentId = 5,
            //        ClassIcon = "",
            //        Category = 1
            //    });
            //}


            if (AccesUser.checkAccess(new ActionModule[] { ActionModule.User }, new ActionType[] { ActionType.View }, Account))
            {
                listMenu.Add(new MenuModel()
                {
                    Id = 12,
                    Text = "Q.lý người dùng",
                    URL = "/Users",
                    ParentId = null,
                    ClassIcon = "fas fa-users-cog",
                    Category = 3
                });
            }
            if (AccesUser.checkAccess(new ActionModule[] { ActionModule.Role }, new ActionType[] { ActionType.View }, Account))
            {
                listMenu.Add(new MenuModel()
                {
                    Id = 13,
                    Text = "Phân quyền",
                    URL = "/RoleModule",
                    ParentId = null,
                    ClassIcon = "fas fa-check-square",
                    Category = 3
                });
            }
            
            return listMenu;
        }
    }
}