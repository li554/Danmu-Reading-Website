using System;
using System.Collections.Generic;
using Mind.Models;

namespace Mind.DAL
{
    public class NoticeService
    {
        private readonly Database _database = new Database();

        public List<Notice> GetNotices(int pageIndex, int pageSize, string filter, out int total)
        {
            var notices = new List<Notice>();
            total = 0;
            try
            {
                _database.Open();
                filter = filter ?? "";
                var sql = $"select * into temptable from book_schema.t_notice " +
                          $"where n_title like '%{filter}%' or n_content like '%{filter}%' or u_email like '%{filter}%' order by id desc;" +
                          $"select top ({pageSize}) * from temptable where id not in(select top (({pageIndex}-1)*{pageSize}) id from temptable order by id) order by id;";
                var noticeReader = _database.Fetch(sql);
                while (noticeReader.Read())
                {
                    var id = (int) noticeReader["id"];
                    var uEmail = (string) noticeReader["u_email"];
                    var nLeft = (int) noticeReader["n_left"];
                    var nContent = (string) noticeReader["n_content"];
                    var nTime = (DateTime) noticeReader["n_time"];
                    var nTitle = (string) noticeReader["n_title"];
                    var notice = new Notice(id, uEmail, nLeft, nContent, nTime, nTitle);
                    notices.Add(notice);
                }
                noticeReader.Close();
                sql = $"select count(*) as total from temptable";
                var totalReader = _database.Fetch(sql);
                total = totalReader.Read() ? (int) totalReader["total"] : 0;
                totalReader.Close();
                sql = $"drop table temptable";
                _database.Update(sql);
                _database.Close();
                return notices;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _database.Close();
                return notices;
            }
        }

        public int AddNotice(Notice notice)
        {
            try
            {
                _database.Open();
                var sql =
                    $"insert into book_schema.t_notice(u_email, n_left, n_content, n_time, n_title) values ('{notice.UEmail}'" +
                    $",{notice.NLeft},'{notice.NContent}','{notice.NTime}','{notice.NTitle}')";
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