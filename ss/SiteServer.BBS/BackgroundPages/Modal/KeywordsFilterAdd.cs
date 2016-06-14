using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using BaiRong.Core;
using SiteServer.BBS.Model;
using System.Web.UI.WebControls;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class KeywordsFilterAdd : BackgroundBasePage
    {
        protected Literal ltlReplacement;
        protected Literal ltlisNewCategory;

        protected TextBox tbReplacement;
        protected TextBox tbName;
        protected TextBox tbCategory;

        protected DropDownList ddlCategory;
        protected DropDownList ddlGrade;
        protected Button btnNewCategory;
        private int id;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加敏感词汇", PageUtils.GetBBSUrl("modal_keywordsFilterAdd.aspx"), arguments, 400, 330);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int id)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ID", id.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("编辑敏感词汇", PageUtils.GetBBSUrl("modal_keywordsFilterAdd.aspx"), arguments, 400, 330);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.id = base.GetIntQueryString("ID");

            if (!IsPostBack)
            {
                if (this.id > 0)
                {
                    KeywordsFilterInfo Info = DataProvider.KeywordsFilterDAO.GetKeywordsFilterInfo(this.id);
                    tbName.Text = Info.Name;
                    ddlCategory.SelectedValue = ConvertHelper.GetString(Info.CategoryID);
                    ddlGrade.SelectedValue = ConvertHelper.GetString(Info.Grade);
                    tbReplacement.Text = ConvertHelper.GetString(Info.Replacement);
                }
                InitCategory();
                InitReplacement();
            }
        }

        public void InitCategory()
        {
            IList<KeywordsCategoryInfo> list = DataProvider.KeywordsCategoryDAO.GetKeywordsCategoryList(base.PublishmentSystemID);
            if (list.Count == 0)
            {
                DataProvider.KeywordsCategoryDAO.CreateDefaultKeywordsCategory(base.PublishmentSystemID);
                list = DataProvider.KeywordsCategoryDAO.GetKeywordsCategoryList(base.PublishmentSystemID);
            }
            ddlCategory.DataSource = list;
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataValueField = "CategoryID";
            ddlCategory.DataBind();
        }

        public void InitReplacement()
        {
            int grade = Convert.ToInt32(ddlGrade.SelectedValue);
            if (grade == 3)
            {
                ltlReplacement.Visible = true;
                tbReplacement.Visible = true;
            }
            else
            {
                ltlReplacement.Visible = false;
                tbReplacement.Visible = false;
            }
        }

        protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitReplacement();
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            if (this.id > 0)
            {
                try
                {
                    KeywordsFilterInfo info = DataProvider.KeywordsFilterDAO.GetKeywordsFilterInfo(this.id);
                    info.Name = tbName.Text.Trim();
                    info.Replacement = tbReplacement.Text.Trim();
                    info.CategoryID = TranslateUtils.ToInt(ddlCategory.SelectedValue);
                    info.Grade = TranslateUtils.ToInt(ddlGrade.SelectedValue);
                    DataProvider.KeywordsFilterDAO.Update(base.PublishmentSystemID, info);
                    isChanged = true;
                }
                catch(Exception ex)
                {
                    isChanged = false;
                    base.FailMessage(ex, "编辑敏感词汇出错！");
                }
            }
            else
            {
                try
                {
                    KeywordsFilterInfo info = new KeywordsFilterInfo(0, base.PublishmentSystemID, TranslateUtils.ToInt(ddlCategory.SelectedValue), TranslateUtils.ToInt(ddlGrade.SelectedValue), tbName.Text.Trim(), tbReplacement.Text.Trim(), 0);

                    DataProvider.KeywordsFilterDAO.Insert(base.PublishmentSystemID, info);
                    isChanged = true;
                }
                catch(Exception ex)
                {
                    isChanged = false;
                    base.FailMessage(ex, "添加敏感词汇出错！");
                }
            }
            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundKeywordsFilting.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
        protected void btnNewCategory_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            if (this.id > 0)
            {
                try
                {
                    KeywordsCategoryInfo keywordsCategoryInfo = new KeywordsCategoryInfo(0, base.PublishmentSystemID, tbCategory.Text.Trim(), true, 0);

                    int categoryid = DataProvider.KeywordsCategoryDAO.Add(keywordsCategoryInfo);

                    KeywordsFilterInfo info = DataProvider.KeywordsFilterDAO.GetKeywordsFilterInfo(this.id);
                    info.Name = tbName.Text.Trim();
                    info.Replacement = tbReplacement.Text.Trim();
                    info.CategoryID = categoryid;
                    info.Grade = TranslateUtils.ToInt(ddlGrade.SelectedValue);
                    DataProvider.KeywordsFilterDAO.Update(base.PublishmentSystemID, info);
                    
                    isChanged = true;
                }
                catch(Exception ex)
                {
                    isChanged = false;
                    base.FailMessage(ex, "编辑敏感词汇出错！");
                }
            }
            else
            {
                try
                {
                    KeywordsCategoryInfo keywordsCategoryInfo = new KeywordsCategoryInfo(0, base.PublishmentSystemID, tbCategory.Text.Trim(), true, 0);

                    int categoryid = DataProvider.KeywordsCategoryDAO.Add(keywordsCategoryInfo);

                    KeywordsFilterInfo info = new KeywordsFilterInfo(0, base.PublishmentSystemID, categoryid, TranslateUtils.ToInt(ddlGrade.SelectedValue), tbName.Text.Trim(), tbReplacement.Text.Trim(), 0);

                    DataProvider.KeywordsFilterDAO.Insert(base.PublishmentSystemID, info);
                    isChanged = true;
                }
                catch(Exception ex)
                {
                    isChanged = false;
                    base.FailMessage(ex, "添加敏感词汇出错！");
                }
            }
            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundKeywordsFilting.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
    }
}
