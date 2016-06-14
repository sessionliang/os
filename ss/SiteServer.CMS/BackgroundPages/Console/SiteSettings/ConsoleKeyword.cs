using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using SiteServer.CMS.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;

namespace SiteServer.CMS.BackgroundPages
{
    public class ConsoleKeyword : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAdd;
        public Button btnImport;

        private int itemID;

        protected override bool IsSaasForbidden { get { return true; } }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null && base.GetQueryString("KeywordID") != null)
            {
                int keywordID = TranslateUtils.ToInt(base.GetQueryString("KeywordID"));
                try
                {
                    DataProvider.KeywordDAO.Delete(keywordID);
                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

            this.itemID = TranslateUtils.ToInt(base.GetQueryString("itemID"));

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.KeywordDAO.GetSelectCommand(this.itemID);
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.SortMode = SortMode.DESC; //排序

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_SiteSettings, "敏感词管理", AppManager.Platform.Permission.Platform_SiteSettings);

                #region 默认创建一个敏感词分类
                DataProvider.KeywordClassifyDAO.SetDefaultKeywordClassifyInfo(base.PublishmentSystemID);
                #endregion

                this.spContents.DataBind();
                this.btnAdd.Attributes.Add("onclick", Modal.KeywordAdd.GetOpenWindowStringToAdd(this.itemID));
                this.btnImport.Attributes.Add("onclick", Modal.KeywordImport.GetOpenWindowString(this.itemID));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int keywordID = TranslateUtils.EvalInt(e.Item.DataItem, "KeywordID");
                string keyword = TranslateUtils.EvalString(e.Item.DataItem, "Keyword");
                string alternative = TranslateUtils.EvalString(e.Item.DataItem, "Alternative");
                EKeywordGrade grade = EKeywordGradeUtils.GetEnumType(TranslateUtils.EvalString(e.Item.DataItem, "Grade"));

                Literal ltlKeyword = e.Item.FindControl("ltlKeyword") as Literal;
                Literal ltlAlternative = e.Item.FindControl("ltlAlternative") as Literal;
                Literal ltlGrade = e.Item.FindControl("ltlGrade") as Literal;
                Literal ltlEdit = e.Item.FindControl("ltlEdit") as Literal;
                Literal ltlDelete = e.Item.FindControl("ltlDelete") as Literal;

                ltlKeyword.Text = keyword;
                ltlAlternative.Text = alternative;
                ltlGrade.Text = EKeywordGradeUtils.GetText(grade);
                ltlEdit.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.KeywordAdd.GetOpenWindowStringToEdit(keywordID, this.itemID));

                string urlDelete = PageUtils.GetCMSUrl(string.Format("console_keyword.aspx?Delete=True&KeywordID={0}", keywordID));
                ltlDelete.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除关键字“{1}”确认吗？')"";>删除</a>", urlDelete, keyword);
            }
        }

        #region 导出词库
        protected void ExportWord_Click(object sender, EventArgs e)
        {
            StringBuilder sbContent = new StringBuilder();
            List<KeywordInfo> list = DataProvider.KeywordDAO.GetKeywordInfoList(this.itemID);
            if (list.Count > 0)
            {
                foreach (KeywordInfo keywordInfo in list)
                {
                    sbContent.Append(keywordInfo.Keyword);
                    if (!string.IsNullOrEmpty(keywordInfo.Alternative))
                    {
                        sbContent.Append("|");
                        sbContent.Append(keywordInfo.Alternative);
                    }
                    sbContent.Append(",");
                }
                if (sbContent.Length > 0) sbContent.Length -= 1;

                string filePath = PathUtils.GetTemporaryFilesPath("敏感词.txt");
                FileUtils.DeleteFileIfExists(filePath);
                FileUtils.WriteText(filePath, ECharset.utf_8, sbContent.ToString());
                PageUtils.Download(base.Page.Response, filePath);
            }
        }

        #endregion
    }
}
