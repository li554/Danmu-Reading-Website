using System.Web.Mvc;
using Mind.Models;

namespace Mind.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult GetSortedBooks()
        {
            var index = new Book();
            return Content(index.GetSortedBooks());
        }

        public ActionResult GetSearchBooks()
        {
            //获取query
            var query = Request["query"];
            var limit = Request["limit"];
            var index = new Book();
            return Content(limit==null ? index.GetSearchBooks(query,10000) : index.GetSearchBooks(query,int.Parse(limit)));
        }
    }
}