using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using BaiRong.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using BaiRong.Model;
using System.IO;
using System.Collections;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class KeywordImport : BackgroundBasePage
    {
        public DropDownList ddlGrade;
        public TextBox tbKeywords;

        private int itemID;

        public static string GetOpenWindowString(int itemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("itemID", itemID.ToString());
            return PageUtility.GetOpenWindowString("导入敏感词", "modal_keywordImport.aspx", arguments, 500, 500);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            this.itemID = TranslateUtils.ToInt(base.GetQueryString("itemID"));
            if (!base.IsPostBack)
            {
                EKeywordGradeUtils.AddListItems(this.ddlGrade);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                EKeywordGrade grade = EKeywordGradeUtils.GetEnumType(this.ddlGrade.SelectedValue);

                string[] keywordArray = this.tbKeywords.Text.Split(',');
                foreach (string item in keywordArray)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string value = item.Trim();
                        string keyword = string.Empty;
                        string alternative = string.Empty;

                        if (value.IndexOf('|') != -1)
                        {
                            keyword = value.Split('|')[0];
                            alternative = value.Split('|')[1];
                        }
                        else
                        {
                            keyword = value;
                        }

                        if (!string.IsNullOrEmpty(keyword) && !DataProvider.KeywordDAO.IsExists(keyword))
                        {
                            KeywordInfo keywordInfo = new KeywordInfo(0, keyword, alternative, grade, this.itemID);
                            DataProvider.KeywordDAO.Insert(keywordInfo);
                        }
                    }
                }

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "导入敏感词失败");
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
    }
}
