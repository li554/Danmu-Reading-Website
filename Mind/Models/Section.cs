using System;
using Newtonsoft.Json.Linq;

namespace Mind.Models
{
    public class Section
    {
        private readonly Database _database = new Database();
        public JArray GetSections(int bId)
        {
            _database.Open();
            try
            {
                var sections = new JArray();
                var sql = $"select * from book_schema.t_section where b_id = {bId}";
                var sectionReader = _database.Fetch(sql);
                while (sectionReader.Read())
                {
                    var section = new JObject
                    {
                        {"id", (int) sectionReader["id"]},
                        {"title", (string) sectionReader["s_title"]},
                        {"content", (string) sectionReader["s_content"]}
                    };
                    sections.Add(section);
                }
                sectionReader.Close();
                _database.Close();
                return sections;
            }
            catch (Exception e)
            {
                _database.Close();
                return null;
            }
        }

        public string GetSection(int bId,int sId)
        {
            _database.Open();
            try
            {
                //检查是否在书架中，如果在，那么更新书的章节记录
                //不在则无需操作
                var sql = "";
                sql = $"select * from book_schema.t_shelf where b_id={bId}";
                var book = _database.Fetch(sql);
                if (book.Read())
                {
                    book.Close();
                    sql = $"update book_schema.t_shelf set t_shelf.s_id={sId} where t_shelf.b_id={bId}";
                    _database.Update(sql);
                    _database.Close();
                    _database.Open();
                }else{
                    book.Close();
                }
                sql = sId==0 ? $"select top 1 * from book_schema.t_section where t_section.b_id = {bId} order by id" : $"select * from book_schema.t_section where t_section.id={sId}";
                var section = _database.Fetch(sql);
                if (!section.Read()) return "";
                var jObject = new JObject
                {
                    {"id", (int) section["id"]},
                    {"title", (string) section["s_title"]},
                    {"content", (string) section["s_content"]}
                };
                if (section["s_content"].Equals(""))
                {
                    var scrapy = new Scrapy();
                    var content = scrapy.Scrape((string) section["s_url"]);
                    if (content != "")
                    {
                        jObject.Remove("content");
                        jObject.Add("content",content);
                        sql = $"update book_schema.t_section set s_content = '{content}' where id={section["id"]}";
                        _database.Close();
                        _database.Update(sql);
                    }
                }
                section.Close();
                _database.Close();
                Console.WriteLine(jObject);
                return jObject.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
               _database.Close();
               return "";
            }
        }

        public int AddSections(int bid,string filepath)
        {
            try
            {
                _database.Open();
                
                //打开文件
                
                
                
                
                
                _database.Close();
            }
            catch (Exception e)
            {
                _database.Close();
            }
            return 0;
        }
    }
}