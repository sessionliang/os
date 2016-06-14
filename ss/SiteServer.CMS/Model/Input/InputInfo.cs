using System;
using System.Xml.Serialization;
using BaiRong.Model;

namespace SiteServer.CMS.Model
{
	public class InputInfo
	{
        private int inputID;
        private string inputName;
		private int publishmentSystemID;
        private DateTime addDate;
        private bool isChecked;
        private bool isReply;
        private int taxis;
        private bool isTemplate;
        private string styleTemplate;
        private string scriptTemplate;
        private string contentTemplate;
        private string settingsXML;
        private int classifyID;//by 20151026 sofuny

        public InputInfo()
		{
            this.inputID = 0;
            this.inputName = string.Empty;
			this.publishmentSystemID = 0;
            this.addDate = DateTime.Now;
            this.isChecked = true;
            this.isReply = false;
            this.taxis = 0;
            this.isTemplate = false;
            this.styleTemplate = string.Empty;
            this.scriptTemplate = string.Empty;
            this.contentTemplate = string.Empty;
            this.settingsXML = string.Empty;
            this.classifyID = 0;
		}

        public InputInfo(int inputID, string inputName, int publishmentSystemID, DateTime addDate, bool isChecked, bool isReply, int taxis, bool isTemplate, string styleTemplate, string scriptTemplate, string contentTemplate, string settingsXML,int classifyID) 
		{
            this.inputID = inputID;
            this.inputName = inputName;
			this.publishmentSystemID = publishmentSystemID;
            this.addDate = addDate;
            this.isChecked = isChecked;
            this.isReply = isReply;
            this.taxis = taxis;
            this.isTemplate = isTemplate;
            this.styleTemplate = styleTemplate;
            this.scriptTemplate = scriptTemplate;
            this.contentTemplate = contentTemplate;
            this.settingsXML = settingsXML;
            this.classifyID = classifyID;
        }

        public int InputID
        {
            get { return inputID; }
            set { inputID = value; }
        }

        public string InputName
		{
            get { return inputName; }
            set { inputName = value; }
		}

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        public bool IsReply
        {
            get { return isReply; }
            set { isReply = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }

        public bool IsTemplate
        {
            get { return isTemplate; }
            set { isTemplate = value; }
        }

        public string StyleTemplate
        {
            get { return styleTemplate; }
            set { styleTemplate = value; }
        }

        public string ScriptTemplate
        {
            get { return scriptTemplate; }
            set { scriptTemplate = value; }
        }

        public string ContentTemplate
        {
            get { return contentTemplate; }
            set { contentTemplate = value; }
        }

        public string SettingsXML
        {
            get { return settingsXML; }
            set { settingsXML = value; }
        }

        public int ClassifyID
        {
            get { return classifyID; }
            set { classifyID = value; }
        }

        InputInfoExtend additional;
        public InputInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new InputInfoExtend(this.settingsXML);
                }
                return this.additional;
            }
        }
	}
}
