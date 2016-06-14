using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;
using System;

namespace SiteServer.CMS.Model
{
    public class TaskCheckInfo : ExtendedAttributes
    {
        public TaskCheckInfo(string serviceParameters)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(serviceParameters);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public int NodeID
        {
            get { return this.GetInt("NodeID", 0); }
            set { base.SetExtendedAttribute("NodeID", value.ToString()); }
        }

        public int ContentID
        {
            get { return this.GetInt("ContentID", 0); }
            set { base.SetExtendedAttribute("ContentID", value.ToString()); }
        }

        public int CheckedLevel
        {
            get { return this.GetInt("CheckedLevel", 0); }
            set { base.SetExtendedAttribute("CheckedLevel", value.ToString()); }
        }

        public string CheckType
        {
            get { return this.GetExtendedAttribute("CheckType"); }
            set { base.SetExtendedAttribute("CheckType", value); }
        }

        public int TranslateNodeID
        {
            get { return this.GetInt("TranslateNodeID", 0); }
            set { base.SetExtendedAttribute("TranslateNodeID", value.ToString()); }
        }

        public string CheckReasons
        {
            get { return this.GetExtendedAttribute("CheckReasons"); }
            set { base.SetExtendedAttribute("CheckReasons", value); }
        }

        public string UserName
        {
            get { return this.GetExtendedAttribute("UserName"); }
            set { base.SetExtendedAttribute("UserName", value); }
        }

        public string AfterCheckType
        {
            get { return this.GetExtendedAttribute("AfterCheckType"); }
            set { base.SetExtendedAttribute("AfterCheckType", value); }
        }

        public string PublishServiceParameters
        {
            get { return this.GetExtendedAttribute("PublishServiceParameters"); }
            set { base.SetExtendedAttribute("PublishServiceParameters", value); }
        }

        public bool IsAll
        {
            get { return base.GetBool("IsCreateAll", false); }
            set { base.SetExtendedAttribute("IsCreateAll", value.ToString()); }
        }

        public string ChannelIDCollection
        {
            get { return this.GetExtendedAttribute("ChannelIDCollection"); }
            set { base.SetExtendedAttribute("ChannelIDCollection", value); }
        }

    }
}
