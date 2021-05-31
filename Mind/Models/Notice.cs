using System;

namespace Mind.Models
{
    public class Notice
    {
        private int id;
        private string uEmail;
        private int nLeft;
        private string nContent;
        private DateTime nTime;
        private string nTitle;
        public Notice(int id,string uEmail, int nLeft, string nContent, DateTime nTime,string nTitle)
        {
            this.id = id;
            this.uEmail = uEmail;
            this.nLeft = nLeft;
            this.nContent = nContent;
            this.nTime = nTime;
            this.nTitle = nTitle;
        }

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string UEmail
        {
            get => uEmail;
            set => uEmail = value;
        }

        public int NLeft
        {
            get => nLeft;
            set => nLeft = value;
        }

        public string NContent
        {
            get => nContent;
            set => nContent = value;
        }

        public DateTime NTime
        {
            get => nTime;
            set => nTime = value;
        }

        public string NTitle
        {
            get => nTitle;
            set => nTitle = value;
        }
    }
}