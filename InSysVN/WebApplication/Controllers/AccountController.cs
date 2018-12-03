using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Framework.EF;
using Microsoft.Owin.Security;
using WebApplication.Models;
using LIB;
using WebApplication.Code;
using Newtonsoft.Json;
using System.Web.Security;
using WebApplication.Authorize;

namespace WebApplication.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IUser _userService;
        private readonly IAccount _accountService;
        public AccountController()
        {
            _userService = SingletonIpl.GetInstance<IplUser>();
            _accountService = SingletonIpl.GetInstance<IplAccount>();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User != null)
            {
                return Redirect("/Home");
            }
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var encryptPassword = Utilities.EncodePassword(model.Password, AppSettings.PasswordHash);
            UserEntity loginResponse = _userService.Login(model.UserName);
            if (loginResponse != null && loginResponse.Id > 0)
            {
                if (loginResponse.Password.Equals(encryptPassword))
                {
                    var serializeModel = new CustomPrincipalSerializeModel
                    {
                        UserId = loginResponse.Id.Value,
                        FullName = loginResponse.FullName,
                        UserName = loginResponse.UserName,
                        Email = loginResponse.Email,
                        AvatarImg = loginResponse.AvatarImg,
                        RoleId = loginResponse.RoleId,
                        RoleLevel = loginResponse.RoleLevel
                    };
                    string userData = JsonConvert.SerializeObject(serializeModel);
                    var authTicket = new FormsAuthenticationTicket(
                    1,
                    loginResponse.UserName,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(15),
                    model.RememberMe,
                    userData);
                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);
                    if (serializeModel.RoleLevel == (int)enumRoleLevel.Staff) return RedirectToAction("Index", "Home");
                    else return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng. Vui lòng kiểm tra lại!");
                    ModelState.Remove("Password");
                }
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập không đúng. vui lòng kiểm tra lại!");
                ModelState.Remove("Password");
            }
            model.Password = null;
            return View(model);
        }
        [HttpPost]
        public ActionResult ResetPassword(string Account, string Email)
        {
            //get account by email
            //check....
            return View("Login");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }
        public ActionResult ChangePassword()
        {
            return View("ChangePassword");
        }
        public ActionResult YourProfile()
        {
            UserEntity user = _userService.GetUserByID(User.Id);
            return View("Profile", user);
        }
        [HttpPost]
        public JsonResult UpdatePassword(AccountChangePasswordModel model)
        {
            if (!model.PasswordNew.Equals(model.PasswordReNew))
            {
                return Json(new { success = false, mess = "Xác nhận mật khẩu không khớp." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                model.PasswordCurrent = Utilities.EncodePassword(model.PasswordCurrent, AppSettings.PasswordHash);
                UserEntity acc = _userService.GetUserByID(User.Id);
                if (!acc.Password.Equals(model.PasswordCurrent))
                {
                    return Json(new { success = false, mess = "Mật khẩu hiện tại không đúng." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    model.PasswordNew = Utilities.EncodePassword(model.PasswordNew, AppSettings.PasswordHash);
                    if (model.PasswordNew == model.PasswordCurrent)
                    {
                        return Json(new { success = false, mess = "Mật khẩu mới phải khác mật khẩu hiện tại." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        model.UserId = acc.Id.Value;
                        return Json(new { success = _accountService.UpdatePassword(model) }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }
    }
}