using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using BaiRong.Core;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class KeywordsCategoryAdd : BackgroundBasePage
    {
        protected TextBox tbName;
        protected RadioButtonList rblOpen;
        private int id;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加分类信息", PageUtils.GetBBSUrl("modal_keywordsCategoryAdd.aspx"), arguments, 400, 300);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int id)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ID", id.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("编辑分类信息", PageUtils.GetBBSUrl("modal_keywordsCategoryAdd.aspx"), arguments, 400, 300);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.id = base.GetIntQueryString("ID");
            if (!IsPostBack)
            {
                if (this.id > 0)
                {
                    KeywordsCategoryInfo info = DataProvider.KeywordsCategoryDAO.GetKeywordsCategoryInfo(this.id);
                    tbName.Text = info.CategoryName;
                    rblOpen.SelectedValue = info.IsOpen.ToString();
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            if (this.id>0)
            {
                try
                {
                    KeywordsCategoryInfo info = DataProvider.KeywordsCategoryDAO.GetKeywordsCategoryInfo(base.GetIntQueryString("ID"));
                    info.CategoryName = tbName.Text.Trim();
                    info.IsOpen = TranslateUtils.ToBool(rblOpen.SelectedValue);

                    DataProvider.KeywordsCategoryDAO.Update(info);
                    isChanged = true;
                }
                catch
                {
                    isChanged = false;
                    base.FailMessage("编辑分类信息出错！");
                }
            }
            else
            {
                try
                {
                    KeywordsCategoryInfo info = new KeywordsCategoryInfo(0, base.PublishmentSystemID, tbName.Text.Trim(), TranslateUtils.ToBool(rblOpen.SelectedValue), 0);

                    DataProvider.KeywordsCategoryDAO.Insert(info);
                    isChanged = true;
                }
                catch
                {
                    isChanged = false;
                    base.FailMessage("添加分类信息出错！");
                }
            }
            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundKeywordsCategory.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
    }
}
