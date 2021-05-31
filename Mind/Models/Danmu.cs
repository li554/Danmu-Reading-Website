using System;
using Newtonsoft.Json.Linq;

namespace Mind.Models
{
    public class Danmu
    {
        private Database _database = new Database();
        
        public string GetDanmu(int sid)
        {
            //从数据库查询弹幕数据，并返回
            _database.Open();
            var sql =
                $"select t_danmu.*,t_user.avatar u_avatar,t_user.name u_name from book_schema.t_danmu,book_schema.t_user where t_danmu.u_email=t_user.email and t_danmu.s_id = {sid}";
            Console.WriteLine(sql);
            try
            {
                var danList = new JArray();
                var dan = _database.Fetch(sql);
                while (dan.Read())
                {
                    var danItem = new JObject
                    {
                        {"id", (int) dan["id"]},
                        {"selected_text", (string) dan["d_selected"]},
                        {"content", (string) dan["d_content"]},
                        {"avatar", (string) dan["u_avatar"]},
                        {"name", (string) dan["u_name"]}
                    };
                    danList.Add(danItem);
                }
                dan.Close();
                return danList.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _database.Close();
                return "";
            }
        }

        public void AddDanmu(int sid,string u_email,string d_selected,string d_content)
        {
            //添加弹幕到数据库
            _database.Open();
            var sql = $"insert into book_schema.t_danmu(s_id, u_email, d_selected, d_content) " +
                      $"values ({sid},'{u_email}','{d_selected}','{d_content}')";
            try
            {
                _database.Update(sql);
                _database.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _database.Close();
            }
        }
    }
}