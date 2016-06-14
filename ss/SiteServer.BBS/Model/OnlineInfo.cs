using System;
using System.Text;

namespace SiteServer.BBS.Model
{
    public sealed class OnlineInfo
    {
        private int id;
        private int publishmentSystemID;
        private string userName;   //登陆用户名
        private string sessionID;
        private DateTime activeTime;  //最新活动时间
        private string ipAddress;   //登陆IP
        private bool isHide;

        //public OnlineInfo()
        //{
        //}

        public OnlineInfo(int id, int publishmentSystemID, string userName, string sessionID, DateTime activeTime, string ipAddress, bool isHide)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.userName = userName;
            this.sessionID = sessionID;
            this.activeTime = activeTime;
            this.ipAddress = ipAddress;
            this.isHide = isHide;
        }

        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }

        public string SessionID
        {
            get
            {
                return sessionID;
            }
            set
            {
                sessionID = value;
            }
        }

        public DateTime ActiveTime
        {
            get
            {
                return activeTime;
            }
            set
            {
                activeTime = value;
            }
        }

        public string IPAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                ipAddress = value;
            }
        }

        public bool IsHide
        {
            get
            {
                return isHide;
            }
            set
            {
                isHide = value;
            }
        }
    }

}