using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Web.UI;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundIdentify : BackgroundBasePage
    {
        public DataGrid dgContents;
        public Button AddButton;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_identify.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                int id = base.GetIntQueryString("ID");
                try
                {
                    DataProvider.IdentifyDAO.Delete(base.PublishmentSystemID, id);
                    base.SuccessMessage("成功删除鉴定");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除鉴定失败，{0}", ex.Message));
                }
            }

            if (base.GetQueryString("ID") != null && (base.GetQueryString("Up") != null || base.GetQueryString("Down") != null))
            {
                int id = base.GetIntQueryString("ID");
                bool isDown = (base.GetQueryString("Down") != null) ? true : false;
                if (isDown)
                {
                    DataProvider.IdentifyDAO.UpdateTaxisToDown(base.PublishmentSystemID, id);
                }
                else
                {
                    DataProvider.IdentifyDAO.UpdateTaxisToUp(base.PublishmentSystemID, id);
                }
                PageUtils.Redirect(BackgroundIdentify.GetRedirectUrl(base.PublishmentSystemID));
                return;
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Template, "主题鉴定", AppManager.BBS.Permission.BBS_Template);

                List<IdentifyInfo> list = DataProvider.IdentifyDAO.GetIdentifyList(base.PublishmentSystemID);
                if (list.Count == 0)
                {
                    DataProvider.IdentifyDAO.CreateDefaultIdentify(base.PublishmentSystemID);
                    list = DataProvider.IdentifyDAO.GetIdentifyList(base.PublishmentSystemID);
                }
                this.dgContents.DataSource = list;
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                this.AddButton.Attributes.Add("onclick", Modal.IdentifyAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));
            }
        }

        public void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                IdentifyInfo identifyInfo = e.Item.DataItem as IdentifyInfo;

                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlStampUrl = e.Item.FindControl("ltlStampUrl") as Literal;
                Literal ltlIconUrl = e.Item.FindControl("ltlIconUrl") as Literal;
                HyperLink hlUpLinkButton = e.Item.FindControl("hlUpLinkButton") as HyperLink;
                HyperLink hlDownLinkButton = e.Item.FindControl("hlDownLinkButton") as HyperLink;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlTitle.Text = identifyInfo.Title;
                ltlStampUrl.Text = string.Format(@"<img src=""{0}"" />", PageUtilityBBS.GetBBSUrl(base.PublishmentSystemID, identifyInfo.StampUrl));
                if (!string.IsNullOrEmpty(identifyInfo.IconUrl))
                {
                    ltlIconUrl.Text = string.Format(@"<img src=""{0}"" />", PageUtilityBBS.GetBBSUrl(base.PublishmentSystemID, identifyInfo.IconUrl));
                }

                string backgroundUrl = BackgroundIdentify.GetRedirectUrl(base.PublishmentSystemID);

                hlUpLinkButton.NavigateUrl = string.Format("{0}&ID={1}&Up=True", backgroundUrl, identifyInfo.ID);
                hlDownLinkButton.NavigateUrl = string.Format("{0}&ID={1}&Down=True", backgroundUrl, identifyInfo.ID);

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.IdentifyAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, identifyInfo.ID));
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}&ID={1}&Delete=True"" onClick=""javascript:return confirm('此操作将删除鉴定“{2}”，确认吗？');"">删除</a>", backgroundUrl, identifyInfo.ID, identifyInfo.Title);
            }
        }
    }
}
