using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model.Service;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class PublishPages : BackgroundBasePage
    {
        protected RadioButtonList IsCreate;

        protected PlaceHolder IsChannelPlaceHolder;
        protected RadioButtonList IsIncludeChildren;
        protected RadioButtonList IsIncludeContents;

        private bool isChannel;
        private int nodeID;
        private string idCollection;

        public static string GetOpenWindowStringByContents(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("IsChannel", false.ToString());
            return PageUtility.GetOpenWindowStringWithCheckBoxValue("发布页面", "modal_publishPages.aspx", arguments, "ContentIDCollection", "请选择需要发布页面的内容!", 500, 400);
        }

        public static string GetOpenWindowStringByChannels(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("IsChannel", true.ToString());
            return PageUtility.GetOpenWindowStringWithCheckBoxValue("发布页面", "modal_publishPages.aspx", arguments, "ChannelIDCollection", "请选择需要发布页面的栏目!", 500, 400);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "IsChannel");
            this.isChannel = TranslateUtils.ToBool(base.GetQueryString("IsChannel"));
            if (this.isChannel)
            {
                this.idCollection = base.GetQueryString("ChannelIDCollection");
                this.IsChannelPlaceHolder.Visible = true;
            }
            else
            {
                this.nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"));
                this.idCollection = base.GetQueryString("ContentIDCollection");
                this.IsChannelPlaceHolder.Visible = false;
            }

            if (!IsPostBack)
            {
              
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (this.isChannel)
            {
                PageUtils.Redirect(Modal.ProgressBar.GetRedirectUrlStringWithPublishChannelsOneByOne(base.PublishmentSystemID, this.idCollection, TranslateUtils.ToBool(this.IsCreate.SelectedValue), TranslateUtils.ToBool(this.IsIncludeChildren.SelectedValue), TranslateUtils.ToBool(this.IsIncludeContents.SelectedValue)));
            }
            else
            {
                PageUtils.Redirect(Modal.ProgressBar.GetRedirectUrlStringWithPublishContentsOneByOne(base.PublishmentSystemID, this.nodeID, this.idCollection, TranslateUtils.ToBool(this.IsCreate.SelectedValue)));
            }
        }
    }
}
