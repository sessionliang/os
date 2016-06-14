using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Net;
using BaiRong.Controls;
using SiteServer.CMS.BackgroundPages;

using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using SiteServer.CMS.Core;
using System.Collections.Generic;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class KeywordEdit : BackgroundBasePage
	{
        public TextBox tbKeyword;
        public DropDownList ddlMatchType;
        public CheckBox cbIsEnabled;

        private int keywordID;
        private string keyword;

        public static string GetOpenWindowString(int publishmentSystemID, int keywordID, string keyword)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("keywordID", keywordID.ToString());
            arguments.Add("keyword", keyword.ToString());
            return PageUtilityWX.GetOpenWindowString("±à¼­¹Ø¼ü´Ê", "modal_keywordEdit.aspx", arguments);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.keywordID = TranslateUtils.ToInt(base.GetQueryString("keywordID"));
            this.keyword = base.GetQueryString("keyword");

			if (!IsPostBack)
			{
                EMatchTypeUtils.AddListItems(this.ddlMatchType);

                this.cbIsEnabled.Checked = true;

                if (this.keywordID > 0)
                {
                    KeywordInfo keywordInfo = DataProviderWX.KeywordDAO.GetKeywordInfo(this.keywordID);

                    this.tbKeyword.Text = this.keyword;
                    ControlUtils.SelectListItems(this.ddlMatchType, EMatchTypeUtils.GetValue(keywordInfo.MatchType));
                    this.cbIsEnabled.Checked = !keywordInfo.IsDisabled;
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            try
            {
                string conflictKeywords = string.Empty;
                if (KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, this.keywordID, this.tbKeyword.Text, out conflictKeywords))
                {
                    base.FailMessage(string.Format("¹Ø¼ü´Ê¡°{0}¡±ÒÑ´æÔÚ£¬ÇëÉèÖÃÆäËû¹Ø¼ü´Ê", conflictKeywords));
                }
                else
                {
                    KeywordInfo keywordInfo = DataProviderWX.KeywordDAO.GetKeywordInfo(this.keywordID);
                    List<string> keywordList = TranslateUtils.StringCollectionToStringList(keywordInfo.Keywords, ' ');
                    int i = keywordList.IndexOf(this.keyword);
                    if (i != -1)
                    {
                        keywordList[i] = this.tbKeyword.Text;
                    }
                    keywordInfo.Keywords = TranslateUtils.ObjectCollectionToString(keywordList, " ");
                    keywordInfo.IsDisabled = !this.cbIsEnabled.Checked;
                    keywordInfo.MatchType = EMatchTypeUtils.GetEnumType(this.ddlMatchType.SelectedValue);

                    DataProviderWX.KeywordDAO.Update(keywordInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "±à¼­¹Ø¼ü´Ê", string.Format("¹Ø¼ü´Ê:{0}", this.keyword));

                    isChanged = true;
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "Ê§°Ü£º" + ex.Message);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
		}
	}
}
