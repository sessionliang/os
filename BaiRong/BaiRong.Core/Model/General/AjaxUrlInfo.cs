using BaiRong.Core;
using System;

namespace BaiRong.Model
{
    public class AjaxUrlInfo
    {
        private string sn;
        private string ajaxUrl;
        private string parameters;

        private int publishmentSystemID;
        private int contentID;
        private int nodeID;
        private int templateID;
        private int createTaskID;

        private string actionType;

        public const string ACTION_QUEUE_CREATE = "QueueCreate";
        public const string ACTION_BACKGROUND_CREATE = "BackgroundCreate";

        public AjaxUrlInfo()
        {
            this.sn = string.Empty;
            this.ajaxUrl = string.Empty;
            this.parameters = string.Empty;
            this.publishmentSystemID = 0;
            this.contentID = 0;
            this.nodeID = 0;
            this.templateID = 0;
            this.createTaskID = 0;
            this.actionType = string.Empty;
        }

        public AjaxUrlInfo(string sn, string ajaxUrl, string parameters)
        {
            this.sn = sn;
            this.ajaxUrl = ajaxUrl;
            this.parameters = parameters;

            this.publishmentSystemID = 0;
            this.contentID = 0;
            this.nodeID = 0;
            this.templateID = 0;
            this.createTaskID = 0;
            this.actionType = string.Empty;
        }

        /// <summary>
        /// 排队生成
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="ajaxUrl"></param>
        /// <param name="parameters"></param>
        /// <param name="publishmentSystemID"></param>
        /// <param name="contentID"></param>
        /// <param name="nodeID"></param>
        /// <param name="templateID"></param>
        /// <param name="createTaskID"></param>
        public AjaxUrlInfo(string sn, string ajaxUrl, string parameters, int publishmentSystemID, int contentID, int nodeID, int templateID, int createTaskID)
        {
            this.sn = sn;
            this.ajaxUrl = ajaxUrl;
            this.parameters = parameters;
            this.publishmentSystemID = publishmentSystemID;
            this.contentID = contentID;
            this.nodeID = nodeID;
            this.templateID = templateID;
            this.createTaskID = createTaskID;
            this.actionType = ACTION_QUEUE_CREATE;
        }

        /// <summary>
        /// 后台自动生成
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="ajaxUrl"></param>
        /// <param name="parameters"></param>
        /// <param name="publishmentSystemID"></param>
        /// <param name="contentID"></param>
        /// <param name="nodeID"></param>
        /// <param name="templateID"></param>
        public AjaxUrlInfo(string sn, string ajaxUrl, string parameters, int publishmentSystemID, int contentID, int nodeID, int templateID)
        {
            this.sn = sn;
            this.ajaxUrl = ajaxUrl;
            this.parameters = parameters;
            this.publishmentSystemID = publishmentSystemID;
            this.contentID = contentID;
            this.nodeID = nodeID;
            this.templateID = templateID;
            this.createTaskID = 0;
            this.actionType = ACTION_BACKGROUND_CREATE;
        }

        public string SN
        {
            get { return sn; }
            set { sn = value; }
        }

        public string AjaxUrl
        {
            get { return ajaxUrl; }
            set { ajaxUrl = value; }
        }

        public string Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }
        public int ContentID
        {
            get { return contentID; }
            set { contentID = value; }
        }
        public int NodeID
        {
            get { return nodeID; }
            set { nodeID = value; }
        }
        public int TemplateID
        {
            get { return templateID; }
            set { templateID = value; }
        }
        public int CreateTaskID
        {
            get { return createTaskID; }
            set { createTaskID = value; }
        }

        /// <summary>
        /// 动作类型
        /// </summary>
        public string ActionType
        {
            get { return actionType; }
            set { actionType = value; }
        }
    }
}
