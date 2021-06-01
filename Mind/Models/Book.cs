using System;
using System.Collections;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace Mind.Models
{
    public class Book
    {
        private readonly Database _database = new Database();
        public string GetSortedBooks()
        {
            _database.Open();
            try
            {
                //获取书籍，并按照类型分组(哲学，自然科学，党建，教育，社科，影视文学，外文，玄幻奇幻，都市青春）
                var sql = "select distinct b_type from book_schema.t_book";
                var sqlData = _database.Fetch(sql);
                var data = new JArray();
                var typeList = new ArrayList();
                while (sqlData.Read())
                {
                    typeList.Add(sqlData[0]);
                }
                sqlData.Close();
                foreach (string type in typeList)
                {
                    sql = "select top 12 * from book_schema.t_book where b_type='" + type + "'";
                    sqlData = _database.Fetch(sql);
                    var item = new JObject {{"type", type}};
                    var itemArray = new JArray();
                    while (sqlData.Read())
                    {
                        var obj = new JObject();
                        obj.Add("id",(int) sqlData["id"]);
                        obj.Add("url",(string) sqlData["b_cover"]);
                        obj.Add("type",(string) sqlData["b_type"]);
                        obj.Add("author",(string) sqlData["b_author"]);
                        obj.Add("name",(string) sqlData["b_name"]);
                        obj.Add("intro",(string) sqlData["b_intro"]);
                        itemArray.Add(obj);
                    }
                    item.Add("values",itemArray);
                    data.Add(item);
                    sqlData.Close();
                }
                _database.Close();
                return data.ToString();
            }
            catch (Exception e)
            {
                _database.Close();
                return "";
            }
        }

        public string GetBook(int bid)
        {
            _database.Open();
            try
            {
                var sql = $"select * from book_schema.t_book where id = {bid}";
                var book = _database.Fetch(sql);
                var data = new JObject();
                if (book.Read())
                {
                    data.Add("id",(int) book["id"]);
                    data.Add("url",(string) book["b_cover"]);
                    data.Add("type",(string) book["b_type"]);
                    data.Add("author",(string) book["b_author"]);
                    data.Add("name",(string) book["b_name"]);
                    data.Add("intro",(string) book["b_intro"]);
                    book.Close();
                    var sections = new Section().GetSections(bid);
                    data.Add("sections",sections);
                    //判断书是否在书架中
                    sql = $"select * from book_schema.t_shelf where b_id={bid}";
                    var bookInShelf = _database.Fetch(sql);
                    data.Add("section",
                        !bookInShelf.Read() ? (sections.Count>0?sections[0]:null) : JObject.Parse(new Section().GetSection(bid, (int) bookInShelf["s_id"])));
                }
                _database.Close();
                return data.ToString();
            }
            catch (Exception e)
            {
                _database.Close();
                return "";
            }
        }

        public string GetSearchBooks(string query,int limit)
        {
            //query可能是作者，书名
            try
            {
                _database.Open();
                var sql = $"select top {limit} * from book_schema.t_book where b_name like '%{query}%' union " +
                      $"select top {limit} * from book_schema.t_book where b_author like '%{query}%'";
                    var book = _database.Fetch(sql);
                var books = new JArray();
                while (book.Read())
                {
                    var data = new JObject();
                    data.Add("id",(int) book["id"]);
                    data.Add("url",(string) book["b_cover"]);
                    data.Add("type",(string) book["b_type"]);
                    data.Add("author",(string) book["b_author"]);
                    data.Add("name",(string) book["b_name"]);
                    data.Add("intro",(string) book["b_intro"]);
                    data.Add("value",(string) book["b_name"]);
                    books.Add(data);
                }
                _database.Close();
                return books.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _database.Close();
                return "[]";
            }
        }

        public int AddBook(string name, string author, string intro,string type,string cover)
        {
            try
            {
                _database.Open();
                var sql =
                    $"insert into book_schema.t_book(b_name, b_author, b_cover, b_type, b_intro) values ('{name}','{author}','{cover}','{type}','{intro}')";
                var code = _database.Update(sql);
                sql = "select top 1 * from book_schema.t_book order by id desc";
                var data = _database.Fetch(sql);
                code = data.Read()?(int) data["id"]:code;
                data.Close();
                _database.Close();
                return code;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _database.Close();
                return -1;
            }
        }
    }
}