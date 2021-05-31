using System.Net;
using System.Text;
using HtmlAgilityPack;
using System.IO;

namespace Mind.Models
{
    public class Scrapy
    {
        public string Scrape(string url)
        {
            //发起请求
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //获得响应
            var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //将响应用流保存，#httpWebResponse只能返回流
            var stream = httpWebResponse.GetResponseStream();
            //将流文件进行编码
            var streamReader = new StreamReader(stream, Encoding.UTF8);
            return httpWebResponse.StatusCode==HttpStatusCode.OK ? SectionProcess(streamReader.ReadToEnd()) : "";
        }

        private static string SectionProcess(string reader)
        {
            //用HAP解析html
            var document = new HtmlDocument();
            document.LoadHtml(reader);
            var node = document.DocumentNode.SelectSingleNode("/html/body/div[@class='content']");
            var content = node.InnerHtml;
            return content;
        }
    }
}