using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BaiRong.Controls;

using SiteServer.BBS.Model;

using BaiRong.Core;
using System.Collections.Specialized;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class AnnouncementAdd : BackgroundBasePage
    {
        public TextBox tbTitle;
        public CheckBox cbIsB;
        public CheckBox cbIsI;
        public CheckBox cbIsU;
        public TextBox tbColor;
        public TextBox tbLinkUrl;
        public RadioButtonList rblIsBlank;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加公告", PageUtils.GetBBSUrl("modal_announcementAdd.aspx"), arguments, 400, 350);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int id)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("id", id.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("编辑公告", PageUtils.GetBBSUrl("modal_announcementAdd.aspx"), arguments, 400, 350);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int id = int.Parse(Request.QueryString["id"]);
                    AnnouncementInfo announcementInfo = DataProvider.AnnouncementDAO.GetAnnouncementInfo(id);
                    bool isStrong = false;
                    bool isEM = false;
                    bool isU = false;
                    string color = string.Empty;
                    StringUtilityBBS.GetHighlight(announcementInfo.FormatString, out isStrong, out isEM, out isU, out color);
                    this.cbIsB.Checked = isStrong;
                    this.cbIsI.Checked = isEM;
                    this.cbIsU.Checked = isU;
                    this.tbTitle.Text = announcementInfo.Title;
                    this.tbColor.Text = color;
                    this.tbLinkUrl.Text = announcementInfo.LinkUrl;
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsBlank, announcementInfo.IsBlank.ToString());
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            if (IsPostBack)
            {
                if (Request.QueryString["ID"] != null)
                {
                    int id = int.Parse(Request.QueryString["ID"]);

                    AnnouncementInfo announcementInfo = DataProvider.AnnouncementDAO.GetAnnouncementInfo(id);
                    announcementInfo.Title = this.tbTitle.Text;
                    announcementInfo.FormatString = StringUtilityBBS.GetHighlightFormatString(this.cbIsB.Checked, this.cbIsI.Checked, this.cbIsU.Checked, this.tbColor.Text);
                    announcementInfo.LinkUrl = PageUtils.AddProtocolToUrl(this.tbLinkUrl.Text);
                    announcementInfo.IsBlank = TranslateUtils.ToBool(this.rblIsBlank.SelectedValue);
                    
                    try
                    {
                        DataProvider.AnnouncementDAO.Update(announcementInfo);

                        isChanged = true;
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "公告修改失败！");
                    }
                }
                else
                {
                    AnnouncementInfo announcementInfo = new AnnouncementInfo(0, base.PublishmentSystemID, 0, DateTime.Now, this.tbTitle.Text, StringUtilityBBS.GetHighlightFormatString(this.cbIsB.Checked, this.cbIsI.Checked, this.cbIsU.Checked, this.tbColor.Text), PageUtils.AddProtocolToUrl(this.tbLinkUrl.Text), TranslateUtils.ToBool(this.rblIsBlank.SelectedValue));

                    try
                    {
                        DataProvider.AnnouncementDAO.Insert(announcementInfo);

                        isChanged = true;
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "公告添加失败！");
                    }
                }
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundAnnouncement.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
    }
}

