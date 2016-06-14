using System;
using System.Collections.Generic;
using System.Text;

using SiteServer.BBS.Core;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.BBS.Model;
using BaiRong.Controls;
using System.Web.UI;
using BaiRong.Model;
using System.Data;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundKeywordsFilting : BackgroundBasePage
    {
        protected SqlPager spContents;
        protected Repeater rptContents;
        protected DropDownList ddlGrade;
        protected DropDownList ddlCategory;
        protected TextBox tbkeywords;
        protected Button btnquery;
        protected Button btnAdd;//添加按钮
        protected Button btnDelAll;//删除按钮
        protected Button btnExport;//导出词库
        protected Button btnImport;//导入词库

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_keywordsFilting.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            this.spContents.ItemsPerPage = 20;
            this.spContents.ConnectionString = DataProvider.ConnectionString;

            this.spContents.SelectCommand = DataProvider.KeywordsFilterDAO.GetSelectCommend(base.PublishmentSystemID, ConvertHelper.GetInteger(Request.QueryString["grade"]), ConvertHelper.GetInteger(Request.QueryString["categoryid"]), ConvertHelper.GetString(Request.QueryString["keyword"]));
            this.spContents.SortField = "Taxis";
            this.spContents.SortMode = SortMode.ASC;

            btnDelAll.Attributes.Add("onclick", "return checkstate('myform','删除');");

            if (base.GetQueryString("Delete") != null)
            {
                int id = base.GetIntQueryString("ID");
                try
                {
                    DataProvider.KeywordsFilterDAO.Delete(base.PublishmentSystemID, id);
                    base.SuccessMessage("成功删除敏感词");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除敏感词失败，{0}", ex.Message));
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Settings, "敏感词管理", AppManager.BBS.Permission.BBS_Settings);

                spContents.DataBind();
                InitCategory();
                this.btnAdd.Attributes.Add("onclick", Modal.KeywordsFilterAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));
                this.btnImport.Attributes.Add("onclick", Modal.KeywordsFilterAddFromTxt.GetOpenWindowString(base.PublishmentSystemID, 0));
            }
        }

        #region 敏感词
        public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlNum = e.Item.FindControl("ltlNum") as Literal;
                Literal ltlID = e.Item.FindControl("ltlID") as Literal;
                Literal ltlName = e.Item.FindControl("ltlName") as Literal;
                Label lblGrade = e.Item.FindControl("lblGrade") as Label;
                Literal ltlCategory = e.Item.FindControl("ltlCategory") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                int categoryid = (int)DataBinder.Eval(e.Item.DataItem, "CategoryID");
                string name = (string)DataBinder.Eval(e.Item.DataItem, "Name");
                int grade = (int)DataBinder.Eval(e.Item.DataItem, "Grade");
                int id = (int)DataBinder.Eval(e.Item.DataItem, "ID");

                ltlID.Text = id.ToString();
                ltlNum.Text = ConvertHelper.GetString(e.Item.ItemIndex + 1);
                ltlName.Text = name;
                if (grade == 1) { lblGrade.Text = "禁用"; lblGrade.Attributes["style"] = "background-color:Red;"; }
                else if (grade == 2) { lblGrade.Text = "审核"; lblGrade.Attributes["style"] = "background-color:Yellow;"; }
                else { lblGrade.Text = "替换"; lblGrade.Attributes["style"] = "background-color:Green;"; }
                KeywordsCategoryInfo info = DataProvider.KeywordsCategoryDAO.GetKeywordsCategoryInfo(categoryid);
                ltlCategory.Text = info.CategoryName;

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.KeywordsFilterAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, id));
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}&ID={1}&Delete=True"" onclick=""javascript:return confirm('此操作将删除敏感词“{2}”，确认吗？');"">删除</a>", BackgroundKeywordsFilting.GetRedirectUrl(base.PublishmentSystemID), id, name);
            }
        }
        #endregion

        #region 敏感词分类
        public void InitCategory()
        {
            List<KeywordsCategoryInfo> list = DataProvider.KeywordsCategoryDAO.GetKeywordsCategoryList(base.PublishmentSystemID);
            if (list.Count == 0)
            {
                DataProvider.KeywordsCategoryDAO.CreateDefaultKeywordsCategory(base.PublishmentSystemID);
                list = DataProvider.KeywordsCategoryDAO.GetKeywordsCategoryList(base.PublishmentSystemID);
            }

            ddlCategory.DataSource = list;
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataValueField = "CategoryID";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("所有分类", "0"));
        }
        #endregion

        #region 搜索按钮
        protected void btnquery_Click(object sender, EventArgs e)
        {
            int grade = ConvertHelper.GetInteger(ddlGrade.SelectedValue);
            int categoryid = ConvertHelper.GetInteger(ddlCategory.SelectedValue);
            string keyword;
            if (tbkeywords.Text.Trim() == "敏感词关键字")
            {
                tbkeywords.Text = ""; keyword = "";
            }
            else
            {
                keyword = tbkeywords.Text.Trim();
            }
            string url = string.Format("{0}&grade={1}&categoryid={2}&keyword={3}", BackgroundKeywordsFilting.GetRedirectUrl(base.PublishmentSystemID), grade, categoryid, keyword);
            Response.Redirect(url);
        }
        #endregion

        #region 导出词库
        protected void btnExport_Click(object sender, EventArgs e)
        {
            StringBuilder sbContent = new StringBuilder();
            int allGrade = 0;
            IList<KeywordsFilterInfo> List = DataProvider.KeywordsFilterDAO.GetKeywordsByGrade(base.PublishmentSystemID, allGrade);
            if (List.Count>0)
            {
                foreach (KeywordsFilterInfo info in List)
                {
                    sbContent.Append(info.Name);
                    sbContent.Append("|");
                    sbContent.Append(info.Grade);
                    sbContent.Append("\r\n");
                }
                string strContent= sbContent.ToString().TrimEnd('\n').ToString();
                string filePath = PathUtils.GetTemporaryFilesPath("敏感词汇导出文件.txt");
                FileUtils.DeleteFileIfExists(filePath);
                FileUtils.WriteText(filePath, ECharset.utf_8, strContent);
                PageUtils.Download(base.Page.Response, filePath);
            }
            else
            {
                base.SuccessMessage("数据为空，没法导出");
            }
        }
        #endregion

        #region 下面的全部删除按钮
        protected void btnDelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.rptContents.Items.Count; i++)
            {
                CheckBox cbSelect = (CheckBox)this.rptContents.Items[i].FindControl("chk_ID");
                if (cbSelect.Checked)
                {
                    Literal ltlID = (Literal)this.rptContents.Items[i].FindControl("ltlID");
                    DataProvider.KeywordsFilterDAO.Delete(base.PublishmentSystemID, TranslateUtils.ToInt(ltlID.Text));
                }
            }
            base.SuccessMessage("删除成功!");
            base.AddWaitAndRedirectScript(BackgroundKeywordsFilting.GetRedirectUrl(base.PublishmentSystemID));
        }
        #endregion
    }
}
