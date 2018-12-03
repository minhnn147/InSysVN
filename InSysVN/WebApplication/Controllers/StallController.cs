using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Framework.EF;
using LIB.Model;
using LIB.DataRequests;
using LIB.Stall;
using Newtonsoft.Json;
using System.Web.Security;
using System.Net.Sockets;

namespace WebApplication.Controllers
{
    public class StallController : BaseController
    {
        private readonly IStall _StallServices;
        public StallController()
        {
            _StallServices = SingletonIpl.GetInstance<IplStall>();
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetOrSetStall()
        {
            try
            {
                string IpAddress = GetLocalIPAddress();
                string StallName = "";
                if (_StallServices.GetOrSetStall(IpAddress, ref StallName))
                {
                    //Khởi tạo
                    HttpCookie StallCookie = new HttpCookie("StallCookie");
                    StallCookie.Value = StallName;
                    StallCookie.Expires = DateTime.Now.AddYears(100);
                    //thêm cookie
                    Response.Cookies.Add(StallCookie);

                    return Json(new { success = true, StallName = StallName }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}