using System;
using BaiRong.Model;

namespace BaiRong.Model
{
    public class LogInfo
    {
        public const string ADMIN_LOGIN = "后台管理员登录";


        private int id;
        private string userName;
        private string ipAddress;
        private DateTime addDate;
        private string action;
        private string summary;

        public LogInfo()
        {
            this.id = 0;
            this.userName = string.Empty;
            this.ipAddress = string.Empty;
            this.addDate = DateTime.Now;
            this.action = string.Empty;
            this.summary = string.Empty;
        }

        public LogInfo(int id, string userName, string ipAddress, DateTime addDate, string action, string summary)
        {
            this.id = id;
            this.userName = userName;
            this.ipAddress = ipAddress;
            this.addDate = addDate;
            this.action = action;
            this.summary = summary;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        public string Action
        {
            get { return action; }
            set { action = value; }
        }

        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }
    }
}
