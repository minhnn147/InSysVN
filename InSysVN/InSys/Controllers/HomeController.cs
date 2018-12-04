using Framework.EF;
using LIB;
using LIB.ContactUs;
using System.Web.Mvc;

namespace InSys.Controllers
{
    public class HomeController : Controller
    {
        private ICategory _cateRepo;
        private IContactUs _contact;
        public HomeController()
        {
            _cateRepo = SingletonIpl.GetInstance<IplCategory>();
            _contact = SingletonIpl.GetInstance<iplContactUs>();
        }
        public ActionResult Index()
        {
            ViewBag.Category = _cateRepo.GetAllData();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public JsonResult ContactUs(string fullname, string email, string phone, string content)
        {
            ContactUsEntity entity = new ContactUsEntity
            {
                FullName = fullname,
                Email = email,
                Phone = phone,
                Content = content
            };
            long res = _contact.InsertContact(entity);
            if (res > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}