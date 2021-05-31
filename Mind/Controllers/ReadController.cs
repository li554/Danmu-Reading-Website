using System;
using System.Web.Mvc;
using Mind.Models;
using Newtonsoft.Json.Linq;

namespace Mind.Controllers
{
    public class ReadController : Controller
    {
        public ActionResult GetSection()
        {
            var bid = int.Parse(Request["b_id"]);
            var sid = 0;
            if (Request["s_id"]!=null)
                sid = int.Parse(Request["s_id"]);
            var model = new Section();
            return Content(model.GetSection(bid,sid));
        }

        public ActionResult GetDanmu()
        {
            var sid = int.Parse(Request["s_id"]);
            var model = new Danmu();
            return Content(model.GetDanmu(sid));
        }
        
        public bool VerifyLogin(String email)
        {
            return Session[email] != null;
        }

        public ActionResult AddDanmu()
        {
            var str = Request["data"];
            var dan = JObject.Parse(str);
            Console.Write(dan);
            if (VerifyLogin((String) dan["email"]))
            {
                var model = new Danmu();
                model.AddDanmu((int) dan["id"], (string)dan["email"], (string) dan["selected_text"],
                    (string) dan["content"]);
                var obj = new JObject {{"code", 1}, {"msg", "添加成功"}};
                return Content(obj.ToString());
            }
            else
            {
                var obj = new JObject {{"code", 0}, {"msg", "未登录，无法发送弹幕"}};
                return Content(obj.ToString());
            }
        }

        public ActionResult GetComment()
        {
            var bid = int.Parse(Request["bid"]);
            var model = new Comment();
            return Content(model.GetComment(bid));
        }

        public ActionResult AddComment()
        {
            var str = Request["data"];
            var comment = JObject.Parse(str);
            var bid = (int) comment["bid"];
            var email = (string) comment["email"];
            if (VerifyLogin(email))
            {
                var content = (string) comment["content"];
                var model = new Comment();
                var code = model.AddComment(bid,email,content);
                var obj = new JObject {{"code", code}, {"msg", code==-1?"添加失败":"添加成功"}};
                return Content(obj.ToString());
            }
            else
            {
                var obj = new JObject {{"code", 0}, {"msg", "未登录，无法发送弹幕"}};
                return Content(obj.ToString());
            }
        }
    }
}