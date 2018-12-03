using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Tên Đăng Nhập")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }
        [Display(Name = "Ghi Nhớ?")]
        public bool RememberMe { get; set; }
    }
    public class CustomPrincipalSerializeModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? SettingId { get; set; }
        public int? CompanyId { get; set; }
        public string[] Roles { get; set; }
        public string AvatarImg { get; set; }
        public int? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int RoleId { get; set; }
        public int RoleLevel { get; set; }
    }
}