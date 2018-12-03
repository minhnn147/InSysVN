using System;
//using Framework.Extensions;
using Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Framework.Data.Attributes;
using System.ComponentModel;

namespace LIB
{
    [Table("Users")]
    public partial class UserEntity : BaseEntity<int>
    {
        #region Properties
        [Key]
        public int? Id { get; set; }
        [DisplayName("Mã người dùng")]
        public string UserCode { get; set; }
        [DisplayName("Họ và tên")]
        public string FullName { get; set; }
        [DisplayName("Tên tài khoản")]
        public string UserName { get; set; }
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }
        [DisplayName("Giới tính")]
        public byte Gender { get; set; }
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }
        [DisplayName("Số điện thoại")]
        public string Phone { get; set; }
        [DisplayName("Ngày sinh")]
        public DateTime? Birthday { get; set; }
        public bool Status { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        public int SettingId { get; set; }
        public string CompanyId { get; set; }
        [DisplayName("Ảnh đại diện")]
        public string AvatarImg { get; set; }
        public string ResetPassCode { get; set; }
        [DisplayName("Quyền người dùng")]
        public int RoleId { get; set; }
        public int RoleLevel { get; set; }
        public string RoleDisplayName { get; set; }
        #endregion
    }
    public class UserResetPassCodeModel
    {
        [Required(ErrorMessage = "Tên tài khoản là bắt buộc")]
        [Display(Name = "Tên tài khoản")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Địa chỉ email là bắt buộc")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class UserChangePassModel
    {
        public int UserId { get; set; }

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
