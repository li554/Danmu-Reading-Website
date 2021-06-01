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

        public ActionResult DeleteShelfBooks()
        {
            var email = Request["email"];
            var bids = Request["bids"];
            var model = new Shelf();
            var code = model.DeleteBooks(email, bids);
            var obj = new JObject();
            obj.Add("code",code);
            obj.Add("msg",code==-1?"删除失败":$"删除了{code}条数据");
            return Content(obj.ToString());
        }
        
        public bool VerifyLogin(string email)
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