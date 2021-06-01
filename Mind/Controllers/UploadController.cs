using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Mind.Models;
using Newtonsoft.Json.Linq;

namespace Mind.Controllers
{
    public class UploadController : Controller
    {
        // GET
        public ActionResult UploadBook()
        {
            var name = Request["book"];
            var author = Request["author"];
            var intro = Request["intro"];
            var type = Request["type"];
            var obj = new JObject {{"code", -1}, {"msg", "失败"}};

            if (name != "")
            {
                var book = new Book();
                var bid = book.AddBook(name, author, intro, type, (string) Session["cover"]);
                if (bid != 0)
                {
                    // var section = new Section();
                    // var code = section.AddSections(bid,(string) Session["txt"]);
                    var request = (HttpWebRequest) WebRequest.Create($"http://localhost:8081/convert?novel_path={(string)Session["txt"]}&bid={bid}");
                    request.Method = "GET";
                    request.ContentType = "text/html;charset=UTF-8";
                    request.UserAgent = null;
                    request.Timeout = 20000;
                    var response = (HttpWebResponse) request.GetResponse();
                    var myResponseStream = response.GetResponseStream();
                    var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                    var retString = myStreamReader.ReadToEnd();
                    //读取完成 关闭数据流
                    myStreamReader.Close();
                    myResponseStream.Close();
                    obj.Remove("code");
                    obj.Remove("msg");
                    obj.Add("code",1);
                    obj.Add("msg",retString);
                }
            }
            return Content(obj.ToString());
        }

        public ActionResult UploadTxt()
        {
            var files = Request.Files;
            var filePath = Server.MapPath("~/App_Data/UploadFiles/");
            if (files.Count > 0)
            {
                var fileName = files[0].FileName;
                var arr = fileName.Split('.');
                var extend = arr[arr.Length - 1];
                if (extend=="jpg"||extend=="jpeg"||extend=="png")
                {
                    Session["cover"] = "/App_Data/UploadFiles/" + fileName;
                    files[0].SaveAs(Path.Combine(filePath, fileName));
                    return Content("上传成功");
                }
                else if (extend=="txt")
                {
                    Session["txt"] = Path.Combine(filePath, fileName);
                    files[0].SaveAs(Path.Combine(filePath, fileName));
                    return Content("上传成功");
                }
                else
                {
                    return Content("上传失败");
                }
            }
            else
            {
                return Content("<p>未获取到Files:" + files.Count.ToString() + "</p>");
            }
        }
    }
}