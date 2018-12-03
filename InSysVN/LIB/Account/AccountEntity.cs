using System;
//using Framework.Extensions;
using Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Framework.Data.Attributes;
using System.ComponentModel;

namespace LIB
{
    public partial class AccountEntity : BaseEntity<int>
    {
        
    }
    public class AccountChangePasswordModel
    {
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Mật khẩu hiện tại là bắt buộc.")]
        [Display(Name = "Mật khẩu hiện tại")]
        public string PasswordCurrent { get; set; }
        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
        [Display(Name = "Mật khẩu mới")]
        public string PasswordNew { get; set; }
        [Required(ErrorMessage = "Nhập lại mật khẩu là bắt buộc.")]
        [Display(Name = "Nhập lại mật khẩu")]
        public string PasswordReNew { get; set; }
    }
}
