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

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class KeywordAddNews : BackgroundBasePage
	{
        public TextBox tbKeywords;
        public DropDownList ddlMatchType;
        public CheckBox cbIsEnabled;
        public PlaceHolder phSelect;
        public CheckBox cbIsSelect;

        private int keywordID;
        private bool isSingle;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, bool isSingle)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("isSingle", isSingle.ToString());
            return PageUtilityWX.GetOpenWindowString("���ͼ�Ļظ��ؼ���", "modal_keywordAddNews.aspx", arguments);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int keywordID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("keywordID", keywordID.ToString());
            return PageUtilityWX.GetOpenWindowString("�༭ͼ�Ļظ��ؼ���", "modal_keywordAddNews.aspx", arguments);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.keywordID = TranslateUtils.ToInt(base.GetQueryString("keywordID"));
            this.isSingle = TranslateUtils.ToBool(base.GetQueryString("isSingle"));

			if (!IsPostBack)
			{
                EMatchTypeUtils.AddListItems(this.ddlMatchType);
                this.cbIsEnabled.Checked = true;

                if (this.keywordID > 0)
                {
                    KeywordInfo keywordInfo = DataProviderWX.KeywordDAO.GetKeywordInfo(this.keywordID);

                    this.tbKeywords.Text = keywordInfo.Keywords;
                    ControlUtils.SelectListItems(this.ddlMatchType, EMatchTypeUtils.GetValue(keywordInfo.MatchType));
                    this.cbIsEnabled.Checked = !keywordInfo.IsDisabled;
                }
                else
                {
                    this.phSelect.Visible = true;
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;
            int keywordIDNew = 0;

            try
            {
                if (this.keywordID == 0)
                {
                    string conflictKeywords = string.Empty;
                    if (KeywordManager.IsKeywordInsertConflict(base.PublishmentSystemID, this.tbKeywords.Text, out conflictKeywords))
                    {
                        base.FailMessage(string.Format("�����ؼ��ʡ�{0}���Ѵ��ڣ������������ؼ���", conflictKeywords));
                    }
                    else
                    {
                        KeywordInfo keywordInfo = new KeywordInfo();

                        keywordInfo.KeywordID = 0;
                        keywordInfo.PublishmentSystemID = base.PublishmentSystemID;
                        keywordInfo.Keywords = this.tbKeywords.Text;
                        keywordInfo.IsDisabled = !this.cbIsEnabled.Checked;
                        keywordInfo.KeywordType = EKeywordType.News;
                        keywordInfo.MatchType = EMatchTypeUtils.GetEnumType(this.ddlMatchType.SelectedValue);
                        keywordInfo.Reply = string.Empty;
                        keywordInfo.AddDate = DateTime.Now;
                        keywordInfo.Taxis = 0;

                        keywordIDNew = DataProviderWX.KeywordDAO.Insert(keywordInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "���ͼ�Ļظ��ؼ���", string.Format("�ؼ���:{0}", this.tbKeywords.Text));

                        isChanged = true;
                    }
                }
                else
                {
                    string conflictKeywords = string.Empty;
                    if (KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, this.keywordID, this.tbKeywords.Text, out conflictKeywords))
                    {
                        base.FailMessage(string.Format("�����ؼ��ʡ�{0}���Ѵ��ڣ������������ؼ���", conflictKeywords));
                    }
                    else
                    {
                        KeywordInfo keywordInfo = DataProviderWX.KeywordDAO.GetKeywordInfo(this.keywordID);
                        keywordInfo.Keywords = this.tbKeywords.Text;
                        keywordInfo.IsDisabled = !this.cbIsEnabled.Checked;
                        keywordInfo.MatchType = EMatchTypeUtils.GetEnumType(this.ddlMatchType.SelectedValue);

                        DataProviderWX.KeywordDAO.Update(keywordInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "�༭ͼ�Ļظ��ؼ���", string.Format("�ؼ���:{0}", this.tbKeywords.Text));

                        isChanged = true;
                    }
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "ʧ�ܣ�" + ex.Message);
            }

            if (isChanged)
            {
                if (this.keywordID == 0)
                {
                    if (this.cbIsSelect.Checked)
                    {
                        PageUtils.Redirect(Modal.ContentSelect.GetRedirectUrlByKeywordAddList(base.PublishmentSystemID, !this.isSingle, keywordIDNew));
                    }
                    else
                    {
                        JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundKeywordNewsAdd.GetRedirectUrl(base.PublishmentSystemID, keywordIDNew, 0, this.isSingle));
                    }
                }
                else
                {
                    JsUtils.OpenWindow.CloseModalPage(Page);
                }
            }
		}
	}
}
