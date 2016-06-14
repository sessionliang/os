using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System.Web.UI;
using SiteServer.CMS.Model;
using System.Collections;
using BaiRong.Model;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WCM.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
    public class BackgroundTagStyleLeft : BackgroundBasePage
    {
        public DataGrid dgContents;

        private ETableStyle tableStyle = ETableStyle.GovInteractContent;
        private string type = string.Empty;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.tableStyle = ETableStyleUtils.GetEnumType(base.GetQueryString("tableStyle"));
            this.type = base.GetQueryString("type");

            if (!IsPostBack)
            {
                string pageTitle = string.Empty;
                if (this.tableStyle == ETableStyle.GovInteractContent)
                {
                    if (StringUtils.EqualsIgnoreCase(this.type, "DepartmentSelect"))
                    {
                        pageTitle = "负责部门设置";
                    }
                    else if (StringUtils.EqualsIgnoreCase(this.type, "AdministratorSelect"))
                    {
                        pageTitle = "负责人员设置";
                    }
                    else if (StringUtils.EqualsIgnoreCase(this.type, "Attributes"))
                    {
                        pageTitle = "办件字段管理";
                    }
                    else if (StringUtils.EqualsIgnoreCase(this.type, "Apply"))
                    {
                        pageTitle = "办件提交样式";
                    }
                    else if (StringUtils.EqualsIgnoreCase(this.type, "Query"))
                    {
                        pageTitle = "办件查询样式";
                    }
                    else if (StringUtils.EqualsIgnoreCase(this.type, "MailSMS"))
                    {
                        pageTitle = " 邮件/短信发送管理";
                    }
                    else if (StringUtils.EqualsIgnoreCase(this.type, "InteractType"))
                    {
                        pageTitle = "办件类型管理";
                    }
                }

                base.InfoMessage(pageTitle);

                BindGrid();
            }
        }

        public void BindGrid()
        {
            if (this.tableStyle == ETableStyle.GovInteractContent)
            {
                ArrayList nodeInfoArrayList = GovInteractManager.GetNodeInfoArrayList(base.PublishmentSystemInfo);
                this.dgContents.DataSource = nodeInfoArrayList;
            }
            
            this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
            this.dgContents.DataBind();
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlName = e.Item.FindControl("ltlName") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                if (this.tableStyle == ETableStyle.GovInteractContent)
                {
                    NodeInfo nodeInfo = e.Item.DataItem as NodeInfo;
                    ltlName.Text = nodeInfo.NodeName;
                    if (StringUtils.EqualsIgnoreCase(this.type, "DepartmentSelect"))
                    {
                        ltlEditUrl.Text = string.Format(@"<a target='management' href=""{0}"">负责部门设置</a>", BackgroundGovInteractDepartmentSelect.GetRedirectUrl(base.PublishmentSystemID, nodeInfo.NodeID));
                    }
                    else if (StringUtils.EqualsIgnoreCase(this.type, "AdministratorSelect"))
                    {
                        ltlEditUrl.Text = string.Format(@"<a target='management' href=""{0}"">负责人员设置</a>", BackgroundGovInteractPermissions.GetRedirectUrl(base.PublishmentSystemID, nodeInfo.NodeID));
                    }
                    else if (StringUtils.EqualsIgnoreCase(this.type, "Attributes"))
                    {
                        ltlEditUrl.Text = string.Format(@"<a target='management' href=""{0}"">自定义字段</a>", BackgroundTableStyle.GetRedirectUrl(base.PublishmentSystemID, BaiRong.Model.ETableStyle.GovInteractContent, base.PublishmentSystemInfo.AuxiliaryTableForGovInteract, nodeInfo.NodeID));
                    }
                    else if (StringUtils.EqualsIgnoreCase(this.type, "Apply"))
                    {
                        int applyStyleID = DataProvider.GovInteractChannelDAO.GetApplyStyleID(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);
                        ltlEditUrl.Text = string.Format(@"<a target='management' href=""{0}"">自定义提交模板</a>&nbsp;&nbsp;&nbsp;&nbsp;<a target='management' href=""{1}"">预览 </a>", PageUtils.GetSTLUrl(string.Format("background_tagStyleTemplate.aspx?PublishmentSystemID={0}&StyleID={1}&ReturnUrl=", base.PublishmentSystemID, applyStyleID)), PageUtils.GetSTLUrl(string.Format("background_tagStylePreview.aspx?PublishmentSystemID={0}&StyleID={1}&ReturnUrl=", base.PublishmentSystemID, applyStyleID)));
                    }
                    else if (StringUtils.EqualsIgnoreCase(this.type, "Query"))
                    {
                        int queryStyleID = DataProvider.GovInteractChannelDAO.GetQueryStyleID(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);
                        ltlEditUrl.Text = string.Format(@"<a target='management' href=""{0}"">自定义查询模板</a>&nbsp;&nbsp;&nbsp;&nbsp;<a target='management' href=""{1}"">预览 </a>", PageUtils.GetSTLUrl(string.Format("background_tagStyleTemplate.aspx?PublishmentSystemID={0}&StyleID={1}&ReturnUrl=", base.PublishmentSystemID, queryStyleID)), PageUtils.GetSTLUrl(string.Format("background_tagStylePreview.aspx?PublishmentSystemID={0}&StyleID={1}&ReturnUrl=", base.PublishmentSystemID, queryStyleID)));
                    }
                    else if (StringUtils.EqualsIgnoreCase(this.type, "MailSMS"))
                    {
                        int styleID = DataProvider.GovInteractChannelDAO.GetApplyStyleID(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);
                        ltlEditUrl.Text = string.Format(@"<a target='management' href=""{0}"">邮件/短信发送</a>", BackgroundTagStyleMailSMS.GetRedirectUrl(base.PublishmentSystemID, styleID, tableStyle, nodeInfo.NodeID));
                    }
                    else if (StringUtils.EqualsIgnoreCase(this.type, "InteractType"))
                    {
                        ltlEditUrl.Text = string.Format(@"<a target='management' href=""{0}"">办件类型管理</a>", PageUtils.GetWCMUrl(string.Format("background_govInteractType.aspx?PublishmentSystemID={0}&NodeID={1}", base.PublishmentSystemID, nodeInfo.NodeID)));
                    }
                }
            }
        }
    }
}
