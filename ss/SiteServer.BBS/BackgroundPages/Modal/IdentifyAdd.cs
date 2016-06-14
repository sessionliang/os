using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Web.UI.WebControls;
using SiteServer.BBS.Provider.SqlServer;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class IdentifyAdd : BackgroundBasePage
    {
        protected TextBox tbTitle;
        protected TextBox tbStampUrl;
        protected TextBox tbIconUrl;

        private int id;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加主题鉴定", PageUtils.GetBBSUrl("modal_identifyAdd.aspx"), arguments, 400, 300);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int id)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ID", id.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("编辑主题鉴定", PageUtils.GetBBSUrl("modal_identifyAdd.aspx"), arguments, 400, 300);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.id = base.GetIntQueryString("ID");

            if (!IsPostBack)
            {
                if (this.id > 0)
                {
                    IdentifyInfo identifyInfo = DataProvider.IdentifyDAO.GetIdentifyInfo(this.id);
                    this.tbTitle.Text = identifyInfo.Title;
                    this.tbStampUrl.Text = identifyInfo.StampUrl;
                    this.tbIconUrl.Text = identifyInfo.IconUrl;
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            if (this.id > 0)
            {
                try
                {
                    IdentifyInfo identifyInfo = DataProvider.IdentifyDAO.GetIdentifyInfo(this.id);
                    identifyInfo.Title = this.tbTitle.Text.Trim();
                    identifyInfo.IconUrl = this.tbIconUrl.Text;
                    identifyInfo.StampUrl = this.tbStampUrl.Text;

                    DataProvider.IdentifyDAO.Update(base.PublishmentSystemID, identifyInfo);
                    isChanged = true;
                }
                catch
                {
                    isChanged = false;
                    base.FailMessage("编辑主题鉴定出错！");
                }
            }
            else
            {
                try
                {
                    IdentifyInfo identifyInfo = new IdentifyInfo(0, base.PublishmentSystemID, this.tbTitle.Text.Trim(), this.tbIconUrl.Text, this.tbStampUrl.Text, 0);

                    DataProvider.IdentifyDAO.Insert(base.PublishmentSystemID, identifyInfo);
                    isChanged = true;
                }
                catch(Exception ex)
                {
                    isChanged = false;
                    base.FailMessage(ex, "添加主题鉴定出错！");
                }
            }
            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundIdentify.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
    }
}
