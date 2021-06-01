using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using Mind.Models;
using Newtonsoft.Json.Linq;

namespace Mind.Controllers
{
    public class LoginController : Controller
    {
        private const string conf = "Data Source = .; Initial Catalog = book_db; Integrated Security = SSPI";
        private readonly SqlConnection sqlConnection = new SqlConnection(conf);
        public ActionResult PLogin()
        {
            var email = Request["email"];
            var pass = Request["pass"];
            //判断是否是管理员
            var data = new JObject();
            //判断session是否已经登录
            if (Session[email]!=null)
            {
                //已经登录
                data.Add("code",1);
                data.Add("msg","已经登录，无需重新登录");
            }
            else
            {
                var model = new User();
                var obj = model.Login(email);
                if (obj == null)
                {
                    data.Add("code",-1);
                    data.Add("msg","未找到用户或密码错误");
                }
                else
                {
                    var checkPass = obj["pass"].ToString();
                    if (pass == checkPass)
                    {
                        data.Add("code",((bool) obj["isManager"])?2:0);
                        data.Add("msg","登录成功");
                        data.Add("user",obj);
                        Session["email"] = obj["email"];
                        Session["name"] = obj["name"];
                        Session[email] = GetRandomString(9,true,true,false,false,"");
                    }
                    else
                    {
                        data.Add("code",-1);
                        data.Add("msg","未找到用户或密码错误");
                    }
                }
                return Content(data.ToString());
            }
            return Content(data.ToString());
        }

        private static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            var b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            var r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum) { str += "0123456789"; }
            if (useLow) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe) { str += "!'#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (var i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
        
        public ActionResult PSignup()
        {
            var email = Request["email"];
            var name = Request["user"];
            var pass = Request["pass"];
            var model = new User();
            return Content(model.Signup(email,name,pass));
        }
    }
}