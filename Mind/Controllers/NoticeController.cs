using System.Web.Mvc;
using Mind.DAL;
using Mind.Models;
using Newtonsoft.Json.Linq;

namespace Mind.Controllers
{
    public class NoticeController : Controller
    {
        private readonly NoticeService _service = new NoticeService();
        public ActionResult GetNotices()
        {
            var filter = Request["filter"];
            var notices =  _service.GetNotices(filter);
            var array = JArray.FromObject(notices);
            return Content(array.ToString());
        }

        public ActionResult AddNotice()
        {
            //获取当前系统时间
            var time = System.DateTime.Now;
            var email = Request["email"];
            var content = Request["content"];
            const int left = 0;
            var title = Request["title"];
            var notice = new Notice(-1, email, left, content, time,title);
            var code = _service.AddNotice(notice);
            var obj = new JObject {{"code", code}, {"msg", code == 1 ? "成功" : "失败"}};
            return Content(obj.ToString());
        }
    }
}