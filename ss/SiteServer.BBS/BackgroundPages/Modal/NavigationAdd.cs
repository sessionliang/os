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
    public class NavigationAdd : BackgroundBasePage
    {
        public TextBox tbTitle;
        public CheckBox cbIsB;
        public CheckBox cbIsI;
        public CheckBox cbIsU;
        public TextBox tbColor;
        public TextBox tbLinkUrl;
        public RadioButtonList rblIsBlank;

        private ENavType navType;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, ENavType navType)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("navType", ENavTypeUtils.GetValue(navType));
            return JsUtils.OpenWindow.GetOpenWindowString("ÃÌº”¡¥Ω”", PageUtils.GetBBSUrl("modal_navigationAdd.aspx"), arguments, 400, 360);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int id, ENavType navType)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("id", id.ToString());
            arguments.Add("navType", ENavTypeUtils.GetValue(navType));
            return JsUtils.OpenWindow.GetOpenWindowString("±‡º≠¡¥Ω”", PageUtils.GetBBSUrl("modal_navigationAdd.aspx"), arguments, 400, 360);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            this.navType = ENavTypeUtils.GetEnumType(base.GetQueryString("navType"));

            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int id = int.Parse(Request.QueryString["id"]);
                    NavigationInfo navInfo = DataProvider.NavigationDAO.GetNavigationInfo(id);
                    bool isStrong = false;
                    bool isEM = false;
                    bool isU = false;
                    string color = string.Empty;
                    StringUtilityBBS.GetHighlight(navInfo.FormatString, out isStrong, out isEM, out isU, out color);
                    this.cbIsB.Checked = isStrong;
                    this.cbIsI.Checked = isEM;
                    this.cbIsU.Checked = isU;
                    this.tbTitle.Text = navInfo.Title;
                    this.tbColor.Text = color;
                    this.tbLinkUrl.Text = navInfo.LinkUrl;
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsBlank, navInfo.IsBlank.ToString());
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

                    NavigationInfo navInfo = DataProvider.NavigationDAO.GetNavigationInfo(id);
                    navInfo.Title = this.tbTitle.Text;
                    navInfo.FormatString = StringUtilityBBS.GetHighlightFormatString(this.cbIsB.Checked, this.cbIsI.Checked, this.cbIsU.Checked, this.tbColor.Text);
                    navInfo.LinkUrl = this.tbLinkUrl.Text;
                    navInfo.IsBlank = TranslateUtils.ToBool(this.rblIsBlank.SelectedValue);
                    
                    try
                    {
                        DataProvider.NavigationDAO.Update(navInfo);

                        isChanged = true;
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "µº∫Ω¡¥Ω”–ﬁ∏ƒ ß∞‹£°");
                    }
                }
                else
                {
                    NavigationInfo navInfo = new NavigationInfo(0, base.PublishmentSystemID, 0, this.navType, this.tbTitle.Text, StringUtilityBBS.GetHighlightFormatString(this.cbIsB.Checked, this.cbIsI.Checked, this.cbIsU.Checked, this.tbColor.Text), this.tbLinkUrl.Text, TranslateUtils.ToBool(this.rblIsBlank.SelectedValue));

                    try
                    {
                        DataProvider.NavigationDAO.Insert(navInfo);

                        isChanged = true;
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "µº∫Ω¡¥Ω”ÃÌº” ß∞‹£°");
                    }
                }
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundNavigation.GetRedirectUrl(base.PublishmentSystemID, this.navType));
            }
        }
    }
}

