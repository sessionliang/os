using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser.StlElement;
using BaiRong.Controls;
using SiteServer.CMS.Services;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal.StlTemplate
{
    public class StlTemplateSelect : BackgroundBasePage
	{
        public DropDownList ddlTemplateType;
        public DropDownList ddlTemplateID;
        public PlaceHolder phNodeID;
        public DropDownList ddlNodeID;
        public Literal ltlSingle;
        public Repeater rptContents;
        public SqlPager spContents;

        private ETemplateType templateType;
        private string target;
        private int nodeID;

        protected override bool IsSinglePage
        {
            get{ return true; }
        }

        public static string GetOpenLayerString(int publishmentSystemID, ETemplateType templateType, int templateID, bool isInDesignPage)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateType", ETemplateTypeUtils.GetValue(templateType));
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("isInDesignPage", isInDesignPage.ToString());
            return JsUtils.Layer.GetOpenLayerString("选择模板及页面", PageUtils.GetSTLUrl("modal_stlTemplateSelect.aspx"), arguments, 500, 0);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.templateType = ETemplateTypeUtils.GetEnumType(base.GetQueryString("templateType"));
            int templateID = TranslateUtils.ToInt(base.GetQueryString("templateID"));
            this.target = TranslateUtils.ToBool(base.GetQueryString("isInDesignPage")) ? "_top" : "_blank";
            this.nodeID = TranslateUtils.ToIntWithNagetive(base.GetQueryString("nodeID"));

            if (IsPostBack)
            {
                string pageUrl = PageUtils.GetSTLUrl(string.Format("modal_stlTemplateSelect.aspx?PublishmentSystemID={0}&templateType={1}&templateID={2}&isInDesignPage={3}&nodeID={4}", base.PublishmentSystemID, this.ddlTemplateType.SelectedValue, this.ddlTemplateID.SelectedValue, base.GetQueryString("isInDesignPage"), this.ddlNodeID.SelectedValue));
                PageUtils.Redirect(pageUrl);
                return;
            }

            ETemplateTypeUtils.AddListItems(this.ddlTemplateType);
            ControlUtils.SelectListItems(this.ddlTemplateType, ETemplateTypeUtils.GetValue(this.templateType));

            ArrayList templateInfoArrayList = DataProvider.TemplateDAO.GetTemplateInfoArrayListByType(base.PublishmentSystemID, this.templateType);
            foreach (TemplateInfo value in templateInfoArrayList)
            {
                ListItem listItem = new ListItem(value.TemplateName, value.TemplateID.ToString());
                this.ddlTemplateID.Items.Add(listItem);
            }
            if (templateID > 0)
            {
                ControlUtils.SelectListItems(this.ddlTemplateID, templateID.ToString());
            }

            TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(base.PublishmentSystemID, TranslateUtils.ToInt(this.ddlTemplateID.SelectedValue));

            if (this.templateType == ETemplateType.IndexPageTemplate)
            {
                this.LoadingByIndexPageTemplate(templateInfo);
            }
            else if (this.templateType == ETemplateType.ChannelTemplate)
            {
                this.LoadingByChannelTemplate(templateInfo);
            }
            else if (this.templateType == ETemplateType.ContentTemplate)
            {
                this.LoadingByContentTemplate(templateInfo);
            }
            else if (this.templateType == ETemplateType.FileTemplate)
            {
                this.LoadingByFileTemplate(templateInfo);
            }
		}

        private void LoadingByIndexPageTemplate(TemplateInfo templateInfo)
        {
            this.phNodeID.Visible = false;

            string pageUrl = PageUtility.DynamicPage.GetDesignUrl(PageUtility.DynamicPage.GetRedirectUrl(base.PublishmentSystemID, 0, 0, templateInfo.TemplateID, 0));
            this.ltlSingle.Text = string.Format(@"<tr><td><a href=""{0}"" target=""_blank"">{1}</a></td><td class=""center""><a href=""{2}"" target=""{3}"">可视化编辑</a></td></tr>", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, templateInfo.CreatedFileFullName), NodeManager.GetNodeName(base.PublishmentSystemID, base.PublishmentSystemID), pageUrl, this.target);
        }

        private void LoadingByChannelTemplate(TemplateInfo templateInfo)
        {
            NodeInfo nodeInfo = null;
            if (this.nodeID == -1)
            {
                base.FailMessage("此栏目未采用所选栏目模板，请重新选择");
            }
            else if (this.nodeID != 0)
            {
                nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
                if (nodeInfo.ChannelTemplateID != templateInfo.TemplateID)
                {
                    nodeInfo = null;
                }
            }

            this.phNodeID.Visible = true;
            NodeManager.AddListItems(this.ddlNodeID.Items, base.PublishmentSystemInfo, false, true);
            int firstNodeID = 0;
            foreach (ListItem listItem in this.ddlNodeID.Items)
            {
                int listNodeID = TranslateUtils.ToInt(listItem.Value);
                NodeInfo listNodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, listNodeID);
                listItem.Text += string.Format(" ({0})", TemplateManager.GetTemplateName(listNodeInfo.PublishmentSystemID, listNodeInfo.ChannelTemplateID));
                if (listNodeInfo.ChannelTemplateID != templateInfo.TemplateID)
                {
                    listItem.Attributes.Add("style", "color:gray;");
                    listItem.Value = "-1";
                }
                else if (firstNodeID == 0)
                {
                    firstNodeID = listNodeID;
                }
            }

            if (nodeInfo != null)
            {
                ControlUtils.SelectListItems(this.ddlNodeID, nodeInfo.NodeID.ToString());
            }
            else if (firstNodeID > 0)
            {
                ControlUtils.SelectListItems(this.ddlNodeID, firstNodeID.ToString());
            }

            if (nodeInfo == null)
            {
                nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, firstNodeID);
            }
            if (nodeInfo != null)
            {
                string pageUrl = PageUtility.DynamicPage.GetDesignUrl(PageUtility.DynamicPage.GetRedirectUrl(base.PublishmentSystemID, nodeInfo.NodeID, 0, 0, 0));
                this.ltlSingle.Text = string.Format(@"<tr><td><a href=""{0}"" target=""_blank"">{1}</a></td><td class=""center""><a href=""{2}"" target=""{3}"">可视化编辑</a></td></tr>", PageUtility.GetChannelUrl(base.PublishmentSystemInfo, nodeInfo, false, base.PublishmentSystemInfo.Additional.VisualType), NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, nodeInfo.NodeID), pageUrl, this.target);
            }
        }

        private void LoadingByContentTemplate(TemplateInfo templateInfo)
        {
            NodeInfo nodeInfo = null;
            if (this.nodeID == -1)
            {
                base.FailMessage("此栏目未采用所选内容模板，请重新选择");
            }
            else if (this.nodeID != 0)
            {
                nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
                if (nodeInfo.ContentTemplateID != templateInfo.TemplateID)
                {
                    nodeInfo = null;
                }
            }

            this.phNodeID.Visible = true;
            NodeManager.AddListItems(this.ddlNodeID.Items, base.PublishmentSystemInfo, false, true);
            int firstNodeID = 0;
            foreach (ListItem listItem in this.ddlNodeID.Items)
            {
                int listNodeID = TranslateUtils.ToInt(listItem.Value);
                NodeInfo listNodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, listNodeID);
                listItem.Text += string.Format(" ({0})", TemplateManager.GetTemplateName(listNodeInfo.PublishmentSystemID, listNodeInfo.ContentTemplateID));
                if (listNodeInfo.ContentTemplateID != templateInfo.TemplateID)
                {
                    listItem.Attributes.Add("style", "color:gray;");
                    listItem.Value = "-1";
                }
                else if (firstNodeID == 0)
                {
                    firstNodeID = listNodeID;
                }
            }

            if (nodeInfo != null)
            {
                ControlUtils.SelectListItems(this.ddlNodeID, nodeInfo.NodeID.ToString());
            }
            else if (firstNodeID > 0)
            {
                ControlUtils.SelectListItems(this.ddlNodeID, firstNodeID.ToString());
            }

            if (nodeInfo == null)
            {
                nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, firstNodeID);
            }
            if (nodeInfo != null)
            {
                string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
                ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);

                this.spContents.ControlToPaginate = this.rptContents;
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.spContents.ItemsPerPage = 12;
                this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
                this.spContents.SelectCommand = BaiRongDataProvider.ContentDAO.GetSelectCommend(tableName, nodeInfo.NodeID, ETriState.All);
                this.spContents.SortField = BaiRongDataProvider.ContentDAO.GetSortFieldName();
                this.spContents.SortMode = SortMode.DESC;
                this.spContents.OrderByString = ETaxisTypeUtils.GetOrderByString(tableStyle, ETaxisType.OrderByTaxisDesc);

                this.spContents.DataBind();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlDesignUrl = e.Item.FindControl("ltlDesignUrl") as Literal;

                ContentInfo contentInfo = new ContentInfo(e.Item.DataItem);
                string pageUrl = PageUtility.DynamicPage.GetDesignUrl(PageUtility.DynamicPage.GetRedirectUrl(base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, 0, 0));

                ltlTitle.Text = WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, string.Empty);
                ltlDesignUrl.Text = string.Format(@"<a href=""{0}"" target=""{1}"">可视化编辑</a>", pageUrl, this.target);
            }
        }

        private void LoadingByFileTemplate(TemplateInfo templateInfo)
        {
            this.phNodeID.Visible = false;

            string pageUrl = string.Empty;

            if (templateInfo.TemplateType == ETemplateType.IndexPageTemplate)
            {
                pageUrl = PageUtility.DynamicPage.GetDesignUrl(PageUtility.DynamicPage.GetRedirectUrl(base.PublishmentSystemID, base.PublishmentSystemID, 0, 0, 0));
            }
            else
            {
                pageUrl = PageUtility.DynamicPage.GetDesignUrl(PageUtility.DynamicPage.GetRedirectUrl(base.PublishmentSystemID, 0, 0, templateInfo.TemplateID, 0));
            }

            this.ltlSingle.Text = string.Format(@"<tr><td><a href=""{0}"" target=""_blank"">{1}</a></td><td class=""center""><a href=""{2}"" target=""{3}"">可视化编辑</a></td></tr>", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, templateInfo.CreatedFileFullName), templateInfo.TemplateName, pageUrl, this.target);
        }
	}
}
