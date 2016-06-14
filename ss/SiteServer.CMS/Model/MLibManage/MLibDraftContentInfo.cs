using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.Model
{
	public class MLibDraftContentInfo : ExtendedAttributes
    {
        public const string TableName = "bairong_MLibDraftContent";
		public MLibDraftContentInfo()
		{
			this.ID = 0;
			this.NodeID = 0;
			this.PublishmentSystemID = 0;
			this.AddUserName = string.Empty;
			this.LastEditUserName = string.Empty;
			this.LastEditDate = DateTime.Now;
            this.Taxis = 0;
			this.ContentGroupNameCollection = string.Empty;
            this.Tags = string.Empty;
            this.SourceID = 0;
            this.ReferenceID = 0;
			this.IsChecked = false;
            this.CheckedLevel = 0;
            this.Comments = 0;
            this.Photos = 0;
            this.Hits = 0;
            this.HitsByDay = 0;
            this.HitsByWeek = 0;
            this.HitsByMonth = 0;
            this.LastHitsDate = DateTime.Now;

            this.Title = string.Empty;
            this.IsTop = false;
            this.AddDate = DateTime.Now;
            this.CheckTaskDate = DateUtils.SqlMinValue;
            this.UnCheckTaskDate = DateUtils.SqlMinValue;

            this.MemberName = string.Empty; //会员账号 by 20160121 sofuny 新的投稿管理功能添加  
		}

        public MLibDraftContentInfo(object dataItem)
            : base(dataItem)
		{
		}

		public int ID
		{
            get { return base.GetInt(ContentAttribute.ID, 0); }
            set { base.SetExtendedAttribute(ContentAttribute.ID, value.ToString()); }
		}

        public int NodeID
        {
            get { return base.GetInt(ContentAttribute.NodeID, 0); }
            set { base.SetExtendedAttribute(ContentAttribute.NodeID, value.ToString()); }
        }

        public int PublishmentSystemID
		{
            get { return base.GetInt(ContentAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(ContentAttribute.PublishmentSystemID, value.ToString()); }
		}

        public string AddUserName
		{
            get { return this.GetExtendedAttribute(ContentAttribute.AddUserName); }
            set { base.SetExtendedAttribute(ContentAttribute.AddUserName, value); }
		}

        public string LastEditUserName
		{
            get { return this.GetExtendedAttribute(ContentAttribute.LastEditUserName); }
            set { base.SetExtendedAttribute(ContentAttribute.LastEditUserName, value); }
		}

        public DateTime LastEditDate
		{
            get { return base.GetDateTime(ContentAttribute.LastEditDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ContentAttribute.LastEditDate, value.ToString()); }
		}

        public int Taxis
        {
            get { return base.GetInt(ContentAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(ContentAttribute.Taxis, value.ToString()); }
        }

        public string ContentGroupNameCollection
		{
            get { return this.GetExtendedAttribute(ContentAttribute.ContentGroupNameCollection); }
            set { base.SetExtendedAttribute(ContentAttribute.ContentGroupNameCollection, value); }
		}

        public string Tags
        {
            get { return this.GetExtendedAttribute(ContentAttribute.Tags); }
            set { base.SetExtendedAttribute(ContentAttribute.Tags, value); }
        }

        public int SourceID
        {
            get { return base.GetInt(ContentAttribute.SourceID, 0); }
            set { base.SetExtendedAttribute(ContentAttribute.SourceID, value.ToString()); }
        }

        public int ReferenceID
        {
            get { return base.GetInt(ContentAttribute.ReferenceID, 0); }
            set { base.SetExtendedAttribute(ContentAttribute.ReferenceID, value.ToString()); }
        }

        public bool IsChecked
		{
            get { return base.GetBool(ContentAttribute.IsChecked, false); }
            set { base.SetExtendedAttribute(ContentAttribute.IsChecked, value.ToString()); }
		}

        public int CheckedLevel
		{
            get { return base.GetInt(ContentAttribute.CheckedLevel, 0); }
            set { base.SetExtendedAttribute(ContentAttribute.CheckedLevel, value.ToString()); }
		}

        public int Comments
        {
            get { return base.GetInt(ContentAttribute.Comments, 0); }
            set { base.SetExtendedAttribute(ContentAttribute.Comments, value.ToString()); }
        }

        public int Photos
        {
            get { return base.GetInt(ContentAttribute.Photos, 0); }
            set { base.SetExtendedAttribute(ContentAttribute.Photos, value.ToString()); }
        }

        public int Teleplays
        {
            get { return base.GetInt(ContentAttribute.Teleplays, 0); }
            set { base.SetExtendedAttribute(ContentAttribute.Teleplays, value.ToString()); }
        }

        public int Hits
        {
            get { return base.GetInt(ContentAttribute.Hits, 0); }
            set { base.SetExtendedAttribute(ContentAttribute.Hits, value.ToString()); }
        }

        public int HitsByDay
        {
            get { return base.GetInt(ContentAttribute.HitsByDay, 0); }
            set { base.SetExtendedAttribute(ContentAttribute.HitsByDay, value.ToString()); }
        }

        public int HitsByWeek
        {
            get { return base.GetInt(ContentAttribute.HitsByWeek, 0); }
            set { base.SetExtendedAttribute(ContentAttribute.HitsByWeek, value.ToString()); }
        }

        public int HitsByMonth
        {
            get { return base.GetInt(ContentAttribute.HitsByMonth, 0); }
            set { base.SetExtendedAttribute(ContentAttribute.HitsByMonth, value.ToString()); }
        }

        public DateTime LastHitsDate
        {
            get { return base.GetDateTime(ContentAttribute.LastHitsDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ContentAttribute.LastHitsDate, value.ToString()); }
        }

       

        public string Title
		{
            get { return this.GetExtendedAttribute(ContentAttribute.Title); }
            set { base.SetExtendedAttribute(ContentAttribute.Title, value); }
		}

      
        public bool IsTop
        {
            get { return base.GetBool(ContentAttribute.IsTop, false); }
            set { base.SetExtendedAttribute(ContentAttribute.IsTop, value.ToString()); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(ContentAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ContentAttribute.AddDate, value.ToString()); }
        }

        public DateTime CheckTaskDate
        {
            get { return base.GetDateTime(ContentAttribute.CheckTaskDate, DateUtils.SqlMinValue); }
            set { base.SetExtendedAttribute(ContentAttribute.CheckTaskDate, value.ToString()); }
        }

        public DateTime UnCheckTaskDate
        {
            get { return base.GetDateTime(ContentAttribute.UnCheckTaskDate, DateUtils.SqlMinValue); }
            set { base.SetExtendedAttribute(ContentAttribute.UnCheckTaskDate, value.ToString()); }
        }
         
        public string MemberName
        {
            get { return this.GetExtendedAttribute(ContentAttribute.MemberName); }
            set { base.SetExtendedAttribute(ContentAttribute.MemberName, value); }
        }

        
        
        public override void SetExtendedAttribute(string name, string value)
        {
            base.SetExtendedAttribute(name, value);
        }

        public override string GetExtendedAttribute(string name)
        {
            return base.GetExtendedAttribute(name);
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return ContentAttribute.AllAttributes;
        }
	}
}
