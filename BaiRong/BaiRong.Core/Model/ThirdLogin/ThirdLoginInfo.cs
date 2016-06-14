using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiRong.Model
{
    public class BaiRongThirdLoginInfo
    {
        private int id;
        private ESiteserverThirdLoginType thirdLoginType;
        private string thirdLoginName;
        private bool isEnabled;
        private int taxis;
        private string description;
        private string settingsXML;

        public BaiRongThirdLoginInfo()
        {
            this.id = 0;
            this.thirdLoginType = ESiteserverThirdLoginType.QQ;
            this.thirdLoginName = string.Empty;
            this.isEnabled = true;
            this.taxis = 0;
            this.description = string.Empty;
            this.settingsXML = string.Empty;
        }

        public BaiRongThirdLoginInfo(int id, ESiteserverThirdLoginType ESiteserverThirdLoginType, string thirdLoginName, bool isEnabled, int taxis, string description, string settingsXML)
        {
            this.id = id;
            this.thirdLoginType = ESiteserverThirdLoginType;
            this.thirdLoginName = thirdLoginName;
            this.isEnabled = isEnabled;
            this.taxis = taxis;
            this.description = description;
            this.settingsXML = settingsXML;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public ESiteserverThirdLoginType ThirdLoginType
        {
            get { return thirdLoginType; }
            set { thirdLoginType = value; }
        }

        public string ThirdLoginName
        {
            get { return thirdLoginName; }
            set { thirdLoginName = value; }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }


        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string SettingsXML
        {
            get { return settingsXML; }
            set { settingsXML = value; }
        }
    }
}
