using System.Web.Mvc;
using Mind.Models;

namespace Mind.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new Book();
            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }
        
        public ActionResult Detail()
        {
            return View();
        }
        
        public ActionResult Read()
        {
            return View();
        }

        public ActionResult Shelf()
        {
            return View();
        }

        public ActionResult Answer()
        {
            return View();
        }

        public ActionResult Notice()
        {
            return View();
        }
    }
}