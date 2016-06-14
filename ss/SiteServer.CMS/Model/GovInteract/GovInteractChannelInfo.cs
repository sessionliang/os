using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	public class GovInteractChannelInfo
	{
		private int nodeID;
        private int publishmentSystemID;
        private int applyStyleID;
        private int queryStyleID;
        private string departmentIDCollection;
		private string summary;

		public GovInteractChannelInfo()
		{
            this.nodeID = 0;
            this.publishmentSystemID = 0;
            this.applyStyleID = 0;
            this.queryStyleID = 0;
            this.departmentIDCollection = string.Empty;
			this.summary = string.Empty;
		}

        public GovInteractChannelInfo(int nodeID, int publishmentSystemID, int applyStyleID, int queryStyleID, string departmentIDCollection, string summary) 
		{
            this.nodeID = nodeID;
            this.publishmentSystemID = publishmentSystemID;
            this.applyStyleID = applyStyleID;
            this.queryStyleID = queryStyleID;
            this.departmentIDCollection = departmentIDCollection;
            this.summary = summary;
		}

        public int NodeID
		{
            get { return nodeID; }
            set { nodeID = value; }
		}

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public int ApplyStyleID
        {
            get { return applyStyleID; }
            set { applyStyleID = value; }
        }

        public int QueryStyleID
        {
            get { return queryStyleID; }
            set { queryStyleID = value; }
        }

        public string DepartmentIDCollection
		{
            get { return departmentIDCollection; }
            set { departmentIDCollection = value; }
		}

        public string Summary
		{
            get { return summary; }
            set { summary = value; }
		}
	}
}
