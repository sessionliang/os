using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public class ViewsStatisticsAttribute
    {
        protected ViewsStatisticsAttribute()
        {
        }

        public const string ID = "ID";
        public const string UserID = "UserID";
        public const string NodeID = "NodeID";
        public const string StasCount = "StasCount";
        public const string StasYear = "StasYear";
        public const string StasMonth = "StasMonth";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string AddDate = "AddDate";

        private static ArrayList allAttributes;
        public static ArrayList AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new ArrayList();
                    allAttributes.Add(ID.ToLower());
                    allAttributes.Add(UserID.ToLower());
                    allAttributes.Add(NodeID.ToLower());
                    allAttributes.Add(StasCount.ToLower());
                    allAttributes.Add(StasYear.ToLower());
                    allAttributes.Add(StasMonth.ToLower());
                    allAttributes.Add(PublishmentSystemID.ToLower());
                }
                return allAttributes;
            }
        }

    }

    public class ViewsStatisticsInfo : ExtendedAttributes
    {
        public const string TableName = "siteserver_ViewsStatistics";

        public ViewsStatisticsInfo()
        {
            this.ID = 0;
            this.UserID = 0;
            this.NodeID = 0;
            this.StasCount = 0;
            this.StasYear = string.Empty;
            this.StasMonth = string.Empty;
            this.PublishmentSystemID = 0;
            this.AddDate = DateTime.Now;
        }

        public ViewsStatisticsInfo(object dataItem)
            : base(dataItem)
        {
        }

        public ViewsStatisticsInfo(int id, int userID, int nodeID, int stasCount, string stasYear, string stasMonth, int publishmentSystemID, DateTime addDate)
        {
            this.ID = id;
            this.UserID = userID;
            this.NodeID = nodeID;
            this.StasCount = stasCount;
            this.StasYear = stasYear;
            this.StasMonth = stasMonth;
            this.PublishmentSystemID = publishmentSystemID;
            this.AddDate = addDate;
        }

        public int ID
        {
            get { return base.GetInt(ViewsStatisticsAttribute.ID, 0); }
            set { base.SetExtendedAttribute(ViewsStatisticsAttribute.ID, value.ToString()); }
        }
         
        public int UserID
        {
            get { return base.GetInt(ViewsStatisticsAttribute.UserID, 0); }
            set { base.SetExtendedAttribute(ViewsStatisticsAttribute.UserID, value.ToString()); }
        }

        public int NodeID
        {
            get { return base.GetInt(ViewsStatisticsAttribute.NodeID, 0); }
            set { base.SetExtendedAttribute(ViewsStatisticsAttribute.NodeID, value.ToString()); }
        }

        public int StasCount
        {
            get { return base.GetInt(ViewsStatisticsAttribute.StasCount, 0); }
            set { base.SetExtendedAttribute(ViewsStatisticsAttribute.StasCount, value.ToString()); }
        }
        public string StasYear
        {
            get { return base.GetExtendedAttribute(ViewsStatisticsAttribute.StasYear); }
            set { base.SetExtendedAttribute(ViewsStatisticsAttribute.StasYear, value); }
        }
        public string StasMonth
        {
            get { return base.GetExtendedAttribute(ViewsStatisticsAttribute.StasMonth); }
            set { base.SetExtendedAttribute(ViewsStatisticsAttribute.StasMonth, value); }
        } 
        public int PublishmentSystemID
        {
            get { return base.GetInt(ViewsStatisticsAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(ViewsStatisticsAttribute.PublishmentSystemID, value.ToString()); }
        }
        public DateTime AddDate
        {
            get { return base.GetDateTime(ViewsStatisticsAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ViewsStatisticsAttribute.AddDate, value.ToString()); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return ViewsStatisticsAttribute.AllAttributes;
        }
    }
}
