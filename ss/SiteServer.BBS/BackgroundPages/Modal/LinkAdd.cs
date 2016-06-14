using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Collections.Specialized;
using SiteServer.BBS.Core;
using BaiRong.Core;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class LinkAdd : BackgroundBasePage
    {
        protected TextBox txtLinkName;
        protected TextBox txtLinkUrl;
        protected TextBox txtIconUrl;

        private int id;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加友情链接", PageUtils.GetBBSUrl("modal_linkAdd.aspx"), arguments, 400, 300);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int id)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ID", id.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("编辑友情链接", PageUtils.GetBBSUrl("modal_linkAdd.aspx"), arguments, 400, 300);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.GetQueryString("ID")))
            {
                this.id = base.GetIntQueryString("ID");
            }

            if (!IsPostBack)
            {
                if (this.id > 0)
                {
                    LinkInfo linkInfo = DataProvider.LinkDAO.GetLinksInfo(this.id);
                    this.txtLinkName.Text = linkInfo.LinkName;
                    this.txtLinkUrl.Text = linkInfo.LinkUrl;
                    this.txtIconUrl.Text = linkInfo.IconUrl;
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            if (!string.IsNullOrEmpty(base.GetQueryString("ID")))
            {
                try
                {
                    LinkInfo linkInfo = DataProvider.LinkDAO.GetLinksInfo(base.GetIntQueryString("ID"));
                    linkInfo.LinkName = this.txtLinkName.Text;
                    linkInfo.LinkUrl = this.txtLinkUrl.Text;
                    linkInfo.IconUrl = this.txtIconUrl.Text;

                    DataProvider.LinkDAO.Update(linkInfo);
                    isChanged = true;
                }
                catch
                {
                    isChanged = false;
                    base.FailMessage("编辑友情链接出错！");
                }
            }
            else
            {
                try
                {
                    LinkInfo linkInfo = new LinkInfo(0, base.PublishmentSystemID, this.txtLinkName.Text, this.txtLinkUrl.Text, this.txtIconUrl.Text, 0);

                    DataProvider.LinkDAO.Insert(linkInfo);
                    isChanged = true;
                }
                catch
                {
                    isChanged = false;
                    base.FailMessage("添加友情链接出错！");
                }
            }
            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundLink.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
    }
}
