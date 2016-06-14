using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class RestrictionOptions : BackgroundBasePage
	{
        protected RadioButtonList RestrictionTypeOfChannel;
        protected RadioButtonList RestrictionTypeOfContent;

        private int nodeID;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            return PageUtility.GetOpenWindowString("访问限制选项", "modal_restrictionOptions.aspx", arguments, 500, 450);
        }
		
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.nodeID = base.GetIntQueryString("NodeID");

			if (!IsPostBack)
			{
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);

                ERestrictionType typeOfChannel = ERestrictionType.NoRestriction;
                ERestrictionType typeOfContnet = ERestrictionType.NoRestriction;
                if (nodeInfo.Additional.Attributes.Count > 0)
                {
                    typeOfChannel = nodeInfo.Additional.RestrictionTypeOfChannel;
                    typeOfContnet = nodeInfo.Additional.RestrictionTypeOfContent;
                }

                ERestrictionTypeUtils.AddListItems(this.RestrictionTypeOfChannel);
                ControlUtils.SelectListItemsIgnoreCase(this.RestrictionTypeOfChannel, ERestrictionTypeUtils.GetValue(typeOfChannel));
                ERestrictionTypeUtils.AddListItems(this.RestrictionTypeOfContent);
                ControlUtils.SelectListItemsIgnoreCase(this.RestrictionTypeOfContent, ERestrictionTypeUtils.GetValue(typeOfContnet));

                
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isSuccess = false;

            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
                nodeInfo.Additional.RestrictionTypeOfChannel = ERestrictionTypeUtils.GetEnumType(this.RestrictionTypeOfChannel.SelectedValue);
                nodeInfo.Additional.RestrictionTypeOfContent = ERestrictionTypeUtils.GetEnumType(this.RestrictionTypeOfContent.SelectedValue);

                try
                {
                    DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "设置栏目页面访问限制选项", string.Format("栏目:{0}", nodeInfo.NodeName));

                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, ex.Message);
                }
            }

            if (isSuccess)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetCMSUrl(string.Format("background_restrictionOptions.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
            }
        }
	}
}
