using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.BBS.Model;
using System.Web.UI;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundKeywordsCategory : BackgroundBasePage
    {
        protected Repeater rptContents;
        protected Button btnAddCategory;//添加分类

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_keywordsCategory.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                int id = base.GetIntQueryString("CategoryID");
                try
                {
                    DataProvider.KeywordsCategoryDAO.Delete(id);
                    DataProvider.KeywordsFilterDAO.DelByCategoryID(base.PublishmentSystemID, id);
                    base.SuccessMessage("成功删除分类信息");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除分类信息失败，{0}", ex.Message));
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Settings, "敏感词分类", AppManager.BBS.Permission.BBS_Settings);

                List<KeywordsCategoryInfo> list = DataProvider.KeywordsCategoryDAO.GetKeywordsCategoryList(base.PublishmentSystemID);
                if (list.Count == 0)
                {
                    DataProvider.KeywordsCategoryDAO.CreateDefaultKeywordsCategory(base.PublishmentSystemID);
                    list = DataProvider.KeywordsCategoryDAO.GetKeywordsCategoryList(base.PublishmentSystemID);
                }
                rptContents.DataSource = list;
                rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                rptContents.DataBind();

                this.btnAddCategory.Attributes.Add("onclick", Modal.KeywordsCategoryAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));
            }
        }

        public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlNum = e.Item.FindControl("ltlNum") as Literal;
                Literal ltlName = e.Item.FindControl("ltlName") as Literal;
                Literal ltlCount = e.Item.FindControl("ltlCount") as Literal;
                Literal ltlisOpen = e.Item.FindControl("ltlisOpen") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                KeywordsCategoryInfo categoryInfo = (KeywordsCategoryInfo)e.Item.DataItem;

                ltlNum.Text = ConvertHelper.GetString(e.Item.ItemIndex + 1);
                ltlName.Text = categoryInfo.CategoryName;
                if (categoryInfo.IsOpen) { ltlisOpen.Text = "开启"; }
                else { ltlisOpen.Text = "停用"; }
                ltlCount.Text = ConvertHelper.GetString(DataProvider.KeywordsCategoryDAO.KeyWordsFiltersCount(categoryInfo.CategoryID));

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.KeywordsCategoryAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, categoryInfo.CategoryID));
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}&categoryID={1}&Delete=True""
                  onclick=""javascript:return confirm('此操作将删除分类“{2}”，确认吗？');"">删除</a>", BackgroundKeywordsCategory.GetRedirectUrl(base.PublishmentSystemID), categoryInfo.CategoryID, categoryInfo.CategoryName);
            }
        }
    }
}
