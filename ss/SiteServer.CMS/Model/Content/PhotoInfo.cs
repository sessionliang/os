using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public class PhotoInfo : ExtendedAttributes
    {
        public const string TableName = "siteserver_Photo";

        public PhotoInfo(object dataItem)
            : base(dataItem)
        {
        }

        public PhotoInfo()
        {
            this.ID = 0;
            this.PublishmentSystemID = 0;
            this.ContentID = 0;
            this.SmallUrl = string.Empty;
            this.MiddleUrl = string.Empty;
            this.LargeUrl = string.Empty;
            this.Taxis = 0;
            this.Description = string.Empty;
        }

        public PhotoInfo(int id, int publishmentSystemID, int contentID, string smallUrl, string middleUrl, string largeUrl, int taxis, string description)
        {
            this.ID = id;
            this.PublishmentSystemID = publishmentSystemID;
            this.ContentID = contentID;
            this.SmallUrl = smallUrl;
            this.MiddleUrl = middleUrl;
            this.LargeUrl = largeUrl;
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

        public string SmallUrl
        {
            get { return base.GetExtendedAttribute("SmallUrl"); }
            set { base.SetExtendedAttribute("SmallUrl", value); }
        }

        public string MiddleUrl
        {
            get { return base.GetExtendedAttribute("MiddleUrl"); }
            set { base.SetExtendedAttribute("MiddleUrl", value); }
        }

        public string LargeUrl
        {
            get { return base.GetExtendedAttribute("LargeUrl"); }
            set { base.SetExtendedAttribute("LargeUrl", value); }
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
