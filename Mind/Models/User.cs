﻿using System;
using Newtonsoft.Json.Linq;

namespace Mind.Models
{
    public class User
    {
        private readonly Database _database = new Database();
        public JObject Login(string email)
        {
            //根据email获取token信息
            //打开数据库：
            _database.Open();
            try
            {
                var sql = $"select * from book_schema.t_user where email='{email}'";
                var user = _database.Fetch(sql);
                var data = new JObject();
                //判断是否查询到有数据
                if (user.Read())
                {
                    data = new JObject
                    {
                        {"email", (string) user["email"]},
                        {"avatar", (string) user["avatar"]},
                        {"name", (string) user["name"]},
                        {"pass", (string) user["pass"]},
                        {"isManager",false}
                    };
                    user.Close();
                }
                else
                {
                    user.Close();
                    sql = $"select * from book_schema.t_manager where id='{email}'";
                    var manager = _database.Fetch(sql);
                    if (manager.Read())
                    {
                        data.Add("isManager",true);
                        data.Add("pass",(string) manager["pass"]);
                        data.Add("email",email);
                        data.Add("name","管理员");
                    }
                    manager.Close();
                }
                _database.Close();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _database.Close();
                return null;
            }
        }

        public string Signup(string email,string name,string pass)
        {
            _database.Open();
            try
            {
                var sql = $"insert into book_schema.t_user(email,name,pass) values('{email}','{name}','{pass}')";
                var code = _database.Update(sql);
                var data = new JObject {{"code", code}, {"msg", "注册完成"}};
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
    }
}