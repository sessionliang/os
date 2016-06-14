using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using System.Collections.Generic;
using BaiRong.Model;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class KeywordAdd : BackgroundBasePage
    {
        protected TextBox tbKeyword;
        protected TextBox tbAlternative;
        protected DropDownList ddlGrade;

        private int itemID;
        private int keywordID;

        public static string GetOpenWindowStringToAdd(int itemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("itemID", itemID.ToString());
            return PageUtility.GetOpenWindowString("添加敏感词", "modal_keywordAdd.aspx", arguments, 380, 300);
        }

        public static string GetOpenWindowStringToEdit(int keywordID, int itemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("itemID", itemID.ToString());
            arguments.Add("KeywordID", keywordID.ToString());
            return PageUtility.GetOpenWindowString("修改敏感词", "modal_keywordAdd.aspx", arguments, 380, 300);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.itemID = TranslateUtils.ToInt(base.GetQueryString("itemID"));
            this.keywordID = TranslateUtils.ToInt(base.GetQueryString("KeywordID"));

            if (!IsPostBack)
            {
                EKeywordGradeUtils.AddListItems(this.ddlGrade);
                if (this.keywordID > 0)
                {
                    KeywordInfo keywordInfo = DataProvider.KeywordDAO.GetKeywordInfo(keywordID);
                    this.tbKeyword.Text = keywordInfo.Keyword;
                    this.tbAlternative.Text = keywordInfo.Alternative;
                    ControlUtils.SelectListItems(this.ddlGrade, EKeywordGradeUtils.GetValue(keywordInfo.Grade));
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            if (this.keywordID > 0)
            {
                try
                {
                    KeywordInfo keywordInfo = DataProvider.KeywordDAO.GetKeywordInfo(this.keywordID);
                    keywordInfo.Keyword = PageUtils.FilterXSS(this.tbKeyword.Text.Trim());
                    keywordInfo.Alternative = PageUtils.FilterXSS(this.tbAlternative.Text.Trim());
                    keywordInfo.Grade = EKeywordGradeUtils.GetEnumType(ddlGrade.SelectedValue);
                    keywordInfo.ClassifyID = this.itemID;
                    DataProvider.KeywordDAO.Update(keywordInfo);

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "修改敏感词失败！");
                }
            }
            else
            {
                if (DataProvider.KeywordDAO.IsExists(this.tbKeyword.Text))
                {
                    base.FailMessage("敏感词添加失败，敏感词名称已存在！");
                }
                else
                {
                    try
                    {
                        KeywordInfo keywordInfo = new KeywordInfo();
                        keywordInfo.Keyword = PageUtils.FilterXSS(this.tbKeyword.Text.Trim());
                        keywordInfo.Alternative = PageUtils.FilterXSS(this.tbAlternative.Text.Trim());
                        keywordInfo.Grade = EKeywordGradeUtils.GetEnumType(ddlGrade.SelectedValue);
                        keywordInfo.ClassifyID = this.itemID;
                        DataProvider.KeywordDAO.Insert(keywordInfo);
                        isChanged = true;
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "添加敏感词失败！");
                    }
                }
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
    }
}
