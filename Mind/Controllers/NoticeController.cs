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
            var pageIndex = Request["pageIndex"]!=null?int.Parse(Request["pageIndex"]):1;
            var pageSize = Request["pageSize"] != null?int.Parse(Request["pageSize"]):10;
            var filter = Request["filter"];
            var notices = _service.GetNotices(pageIndex,pageSize,filter,out var total);
            var array = JArray.FromObject(notices);
            var obj = new JObject {{"notices", array}, {"total", total}};
            return Content(obj.ToString());
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