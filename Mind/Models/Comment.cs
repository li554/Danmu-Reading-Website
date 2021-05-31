using System;
using Newtonsoft.Json.Linq;

namespace Mind.Models
{
    public class Comment
    {
        private readonly Database _database = new Database();
        public string GetComment(int bid)
        {
            _database.Open();
            try
            {
                var sql = $"select t_user.name as name,t_user.avatar as avatar,t_comment.* from book_schema.t_comment,book_schema.t_user where t_user.email=t_comment.u_email and b_id={bid} order by t_comment.id desc";
                var comments = _database.Fetch(sql);
                var data = new JArray();
                while (comments.Read())
                {
                    var obj = new JObject
                    {
                        {"user_avatar", (string) comments["avatar"]},
                        {"user_name", (string) comments["name"]},
                        {"book_id", (int) comments["b_id"]},
                        {"id", (int) comments["id"]},
                        {"content", (string) comments["c_content"]}
                    };
                    data.Add(obj);
                }
                comments.Close();
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

        public int AddComment(int bid, string email, string content)
        {
            _database.Open();
            try
            {
                var sql =
                    $"insert into book_schema.t_comment(b_id, u_email, c_content) values ({bid},'{email}','{content}')";
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