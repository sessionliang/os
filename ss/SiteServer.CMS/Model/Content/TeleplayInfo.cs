using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public class TeleplayInfo : ExtendedAttributes
    {
        public const string TableName = "siteserver_Teleplay";

        public TeleplayInfo(object dataItem)
            : base(dataItem)
        {
        }

        public TeleplayInfo()
        {
            this.ID = 0;
            this.PublishmentSystemID = 0;
            this.ContentID = 0;
            this.Title = "";
            this.StillUrl = string.Empty;
            this.Taxis = 0;
            this.Description = string.Empty;
        }

        public TeleplayInfo(int id, int publishmentSystemID, int contentID, string stillUrl, int taxis, string description, string title)
        {
            this.ID = id;
            this.PublishmentSystemID = publishmentSystemID;
            this.ContentID = contentID;
            this.Title = title;
            this.StillUrl = stillUrl;
            this.Taxis = taxis;
            this.Description = description;
        }

        public int ID
        {
            get { return base.GetInt("ID", 0); }
            set { base.SetExtendedAttribute("ID", value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt("PublishmentSystemID", 0); }
            set { base.SetExtendedAttribute("PublishmentSystemID", value.ToString()); }
        }

        public int ContentID
        {
            get { return base.GetInt("ContentID", 0); }
            set { base.SetExtendedAttribute("ContentID", value.ToString()); }
        }

        public string Title
        {
            get { return base.GetExtendedAttribute("Title"); }
            set { base.SetExtendedAttribute("Title", value); }
        }

        public string StillUrl
        {
            get { return base.GetExtendedAttribute("StillUrl"); }
            set { base.SetExtendedAttribute("StillUrl", value); }
        }

        public int Taxis
        {
            get { return base.GetInt("Taxis", 0); }
            set { base.SetExtendedAttribute("Taxis", value.ToString()); }
        }

        public string Description
        {
            get { return base.GetExtendedAttribute("Description"); }
            set { base.SetExtendedAttribute("Description", value); }
        }

    }
}
