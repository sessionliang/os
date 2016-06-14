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
    public class KeywordAddText : BackgroundBasePage
    {
        public TextBox tbKeywords;
        public DropDownList ddlMatchType;
        public CheckBox cbIsEnabled;
        public TextBox tbReply;

        public Button btnContentSelect;
        public Button btnChannelSelect;

        private int keywordID;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return PageUtilityWX.GetOpenWindowString("添加文本回复关键词", "modal_keywordAddText.aspx", arguments);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int keywordID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("keywordID", keywordID.ToString());
            return PageUtilityWX.GetOpenWindowString("编辑文本回复关键词", "modal_keywordAddText.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.keywordID = TranslateUtils.ToInt(base.GetQueryString("keywordID"));

            if (!IsPostBack)
            {
                EMatchTypeUtils.AddListItems(this.ddlMatchType);

                this.cbIsEnabled.Checked = true;

                this.btnContentSelect.Attributes.Add("onclick", Modal.ContentSelect.GetOpenWindowString(base.PublishmentSystemID, false, "contentSelect"));
                this.btnChannelSelect.Attributes.Add("onclick", SiteServer.CMS.BackgroundPages.Modal.ChannelSelect.GetOpenWindowString(base.PublishmentSystemID, true));

                if (this.keywordID > 0)
                {
                    KeywordInfo keywordInfo = DataProviderWX.KeywordDAO.GetKeywordInfo(this.keywordID);

                    this.tbKeywords.Text = keywordInfo.Keywords;
                    ControlUtils.SelectListItems(this.ddlMatchType, EMatchTypeUtils.GetValue(keywordInfo.MatchType));
                    this.cbIsEnabled.Checked = !keywordInfo.IsDisabled;
                    this.tbReply.Text = keywordInfo.Reply;
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            try
            {
                if (this.keywordID == 0)
                {
                    string conflictKeywords = string.Empty;
                    if (KeywordManager.IsKeywordInsertConflict(base.PublishmentSystemID, PageUtils.FilterSql(this.tbKeywords.Text), out conflictKeywords))
                    {
                        base.FailMessage(string.Format("触发关键词“{0}”已存在，请设置其他关键词", conflictKeywords));
                    }
                    else
                    {
                        KeywordInfo keywordInfo = new KeywordInfo();


                        keywordInfo.KeywordID = 0;
                        keywordInfo.PublishmentSystemID = base.PublishmentSystemID;
                        keywordInfo.Keywords = PageUtils.FilterSql(this.tbKeywords.Text);
                        keywordInfo.IsDisabled = !this.cbIsEnabled.Checked;
                        keywordInfo.KeywordType = EKeywordType.Text;
                        keywordInfo.MatchType = EMatchTypeUtils.GetEnumType(this.ddlMatchType.SelectedValue);
                        keywordInfo.Reply = this.tbReply.Text;
                        keywordInfo.AddDate = DateTime.Now;
                        keywordInfo.Taxis = 0;

                        DataProviderWX.KeywordDAO.Insert(keywordInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "添加文本回复关键词", string.Format("关键词:{0}", this.tbKeywords.Text));

                        isChanged = true;
                    }
                }
                else
                {
                    string conflictKeywords = string.Empty;
                    if (KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, this.keywordID, this.tbKeywords.Text, out conflictKeywords))
                    {
                        base.FailMessage(string.Format("触发关键词“{0}”已存在，请设置其他关键词", conflictKeywords));
                    }
                    else
                    {
                        KeywordInfo keywordInfo = DataProviderWX.KeywordDAO.GetKeywordInfo(this.keywordID);
                        keywordInfo.Keywords = this.tbKeywords.Text;
                        keywordInfo.IsDisabled = !this.cbIsEnabled.Checked;
                        keywordInfo.MatchType = EMatchTypeUtils.GetEnumType(this.ddlMatchType.SelectedValue);
                        keywordInfo.Reply = this.tbReply.Text;

                        DataProviderWX.KeywordDAO.Update(keywordInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "编辑文本回复关键词", string.Format("关键词:{0}", this.tbKeywords.Text));

                        isChanged = true;
                    }
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "失败：" + ex.Message);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
    }
}
