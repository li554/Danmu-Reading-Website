using System;
using System.Web.Mvc;
using Mind.Models;
using Newtonsoft.Json.Linq;

namespace Mind.Controllers
{
    public class ShelfController : Controller
    {
        public ActionResult GetShelfBooks()
        {
            var email = Request["email"];
            var model = new Shelf();
            return Content(model.GetShelfBooks(email));
        }
        
        public bool VerifyLogin(String email)
        {
            return Session[email] != null;
        }

        public ActionResult AddToShelf()
        {
            var bid = int.Parse(Request["bid"]);
            var email = Request["email"];
            if (VerifyLogin(email))
            {
                var sid = int.Parse(Request["sid"]);
                var model = new Shelf();
                var code = model.AddBook(bid,email,sid);
                var data = new JObject {{"code", code}, {"msg", code == -1 ? "添加失败" : "添加成功"}};
                return Content(data.ToString());
            }
            else
            {
                var data = new JObject {{"code", -2}, {"msg", "未登录，不能访问书架"}};
                return Content(data.ToString());
            }
        }
    }
}