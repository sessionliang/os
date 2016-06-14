using System;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundConfigurationCrossSiteTrans : BackgroundBasePage
	{
        public Repeater rptContents;
        public RadioButtonList IsCrossSiteTransChecked;

        private int currentNodeID;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, "跨站转发设置", AppManager.CMS.Permission.WebSite.Configration);

                JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", ChannelLoading.GetScript(base.PublishmentSystemInfo, ELoadingType.ConfigurationCrossSiteTrans, null));

                if (!string.IsNullOrEmpty(base.GetQueryString("CurrentNodeID")))
                {
                    this.currentNodeID = TranslateUtils.ToInt(base.GetQueryString("CurrentNodeID"));
                    string onLoadScript = ChannelLoading.GetScriptOnLoad(base.PublishmentSystemID, this.currentNodeID);
                    if (!string.IsNullOrEmpty(onLoadScript))
                    {
                        JsManager.RegisterClientScriptBlock(Page, "NodeTreeScriptOnLoad", onLoadScript);
                    }
                }

                BindGrid();

                EBooleanUtils.AddListItems(this.IsCrossSiteTransChecked, "无需审核", "需要审核");
                ControlUtils.SelectListItems(this.IsCrossSiteTransChecked, base.PublishmentSystemInfo.Additional.IsCrossSiteTransChecked.ToString());
			}
		}

        public void BindGrid()
        {
            try
            {
                this.rptContents.DataSource = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(base.PublishmentSystemID, 0);
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int nodeID = (int)e.Item.DataItem;
            bool enabled = (base.IsOwningNodeID(nodeID)) ? true : false;
            if (!enabled)
            {
                if (!base.IsHasChildOwningNodeID(nodeID)) e.Item.Visible = false;
            }
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;
            ltlHtml.Text = ChannelLoading.GetChannelRowHtml(base.PublishmentSystemInfo, nodeInfo, enabled, ELoadingType.ConfigurationCrossSiteTrans, null);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.IsCrossSiteTransChecked = TranslateUtils.ToBool(this.IsCrossSiteTransChecked.SelectedValue);
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改默认跨站转发设置");

                    base.SuccessMessage("默认跨站转发设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "默认跨站转发设置修改失败！");
				}
			}
		}
	}
}
