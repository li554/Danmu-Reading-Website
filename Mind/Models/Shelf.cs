using System;
using Newtonsoft.Json.Linq;

namespace Mind.Models
{
    public class Shelf
    {
        private readonly Database _database = new Database();
        public string GetShelfBooks(string email)
        {
            _database.Open();
            try
            {
                var sql =
                    $"select t_book.*,t_section.id sid,t_section.s_title title,t_section.s_content content,t_section.s_url s_url from book_schema.t_shelf,book_schema.t_book,book_schema.t_section where t_shelf.b_id=t_book.id and t_section.id=t_shelf.s_id and t_shelf.u_email='{email}'";
                Console.WriteLine(sql);
                var books = _database.Fetch(sql);
                var data = new JArray();
                while (books.Read())
                {
                    var section = new JObject
                    {
                        {"id", (int) books["sid"]},
                        {"title", (string) books["title"]},
                        {"content", (string) books["content"]},
                        {"url", (string) books["s_url"]}
                    };
                    var obj = new JObject
                    {
                        {"id", (int) books["id"]},
                        {"url", (string) books["b_cover"]},
                        {"type", (string) books["b_type"]},
                        {"author", (string) books["b_author"]},
                        {"name", (string) books["b_name"]},
                        {"intro", (string) books["b_intro"]},
                        {"section",section}
                    };
                    data.Add(obj);
                }
                books.Close();
                _database.Close();
                return data.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _database.Close();
                return "";
            }
        }

        public int AddBook(int bid,string email,int sid)
        {
            _database.Open();
            try
            {
                var sql = $"insert into book_schema.t_shelf(b_id, u_email, s_id) values ('{bid}','{email}','{sid}')";
                return _database.Update(sql);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _database.Close();
                return -1;
            }
        }

        public int DeleteBooks(string email,string bids)
        {
            _database.Open();
            try
            {
                var sql = $"delete from book_schema.t_shelf where u_email='{email}' and id in {bids}";
                var code = _database.Update(sql);
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