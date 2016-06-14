using System;

namespace SiteServer.CMS.Model
{
    public class TemplateLogInfo
    {
        private int id;
        private int templateID;
        private int publishmentSystemID;
        private DateTime addDate;
        private string addUserName;
        private int contentLength;
        private string templateContent;

        public TemplateLogInfo()
        {
            this.id = 0;
            this.templateID = 0;
            this.publishmentSystemID = 0;
            this.addDate = DateTime.Now;
            this.addUserName = string.Empty;
            this.contentLength = 0;
            this.templateContent = string.Empty;
        }

        public TemplateLogInfo(int id, int templateID, int publishmentSystemID, DateTime addDate, string addUserName, int contentLength, string templateContent)
        {
            this.id = id;
            this.templateID = templateID;
            this.publishmentSystemID = publishmentSystemID;
            this.addDate = addDate;
            this.addUserName = addUserName;
            this.contentLength = contentLength;
            this.templateContent = templateContent;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int TemplateID
        {
            get { return templateID; }
            set { templateID = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        public string AddUserName
        {
            get { return addUserName; }
            set { addUserName = value; }
        }

        public int ContentLength
        {
            get { return contentLength; }
            set { contentLength = value; }
        }

        public string TemplateContent
        {
            get { return templateContent; }
            set { templateContent = value; }
        }
    }
}
