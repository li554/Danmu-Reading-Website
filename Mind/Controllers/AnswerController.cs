using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace Mind.Controllers
{
    public class AnswerController : Controller
    {
        // GET
        public ActionResult ImageProcess()
        {
            // var url = "https://ftp.bmp.ovh/imgs/2021/05/26403d3942f44468.jpg";
            var url = Request["url"];
            var uri = new Uri(url);
            var img = GetImageFromNet(uri, (request) => { request.Timeout = 60000;},
                (response) => Image.FromStream(response.GetResponseStream()));
            return Content("data:image/jpg;base64,"+ToBase64(new Bitmap(img)));
        }

        private static string ToBase64(Bitmap bmp)
        {
            try
            {
                var ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                var arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                var str64 = Convert.ToBase64String(arr);
                return str64;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private static Image GetImageFromNet(Uri url, Action<WebRequest> requestAction = null, Func<WebResponse, Image> responseFunc = null)
        {
            Image img;
            try
            {
                var request = WebRequest.Create(url);
                requestAction?.Invoke(request);
                using (var response = request.GetResponse())
                {
                    img = responseFunc != null ? responseFunc(response) : Image.FromStream(response.GetResponseStream());
                }
            }
            catch
            {
                img = null;
            }
            return img;
        }  
    }
}