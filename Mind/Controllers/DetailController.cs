using System.Web.Mvc;
using Mind.Models;

namespace Mind.Controllers
{
    public class DetailController: Controller
    {
        public ActionResult GetBook()
        {
            var bid = int.Parse(Request["b_id"]);
            var model = new Book();
            return Content(model.GetBook(bid));
        }
    }
}