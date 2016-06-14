using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Collections;
using System.Text;

using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data;
using System.Data.OleDb;

using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class SearchwordExport : BackgroundBasePage
    {
        public PlaceHolder phExport;
        //public RadioButtonList CheckedState;

        public TextBox tbSearchResultCount;
        public TextBox tbSearchCount;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("导出搜索关键词", "modal_searchwordExport.aspx", arguments, 380, 250);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                this.phExport.Visible = true;
                //ETriStateUtils.AddListItems(this.CheckedState, "全部", "审核通过", "未审核");
                //ControlUtils.SelectListItems(this.CheckedState, ETriStateUtils.GetValue(ETriState.All));
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            this.phExport.Visible = false;

            string docFileName = "searchwords.xls";
            string filePath = PathUtils.GetTemporaryFilesPath(docFileName);
            SearchwordExport.CreateExcelFileForSearchwords(base.PublishmentSystemID, filePath, TranslateUtils.ToInt(this.tbSearchResultCount.Text), TranslateUtils.ToInt(this.tbSearchCount.Text));

            HyperLink link = new HyperLink();
            link.NavigateUrl = PageUtils.GetTemporaryFilesUrl(docFileName);
            link.Text = "下载";
            string successMessage = "成功导出文件！&nbsp;&nbsp;" + ControlUtils.GetControlRenderHtml(link);
            base.SuccessMessage(successMessage);
        }

        public static void CreateExcelFileForSearchwords(int publishmentSystemID, string filePath, int searchResultCount, int searchCount)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
            OLEDBConnStr += " Extended Properties=Excel 8.0;";
            StringBuilder createBuilder = new StringBuilder();

            createBuilder.Append("CREATE TABLE Searchwords ( ");

            createBuilder.AppendFormat(" [{0}] NTEXT, ", SearchwordAttribute.Searchword);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", SearchwordAttribute.SearchResultCount);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", SearchwordAttribute.SearchCount);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", SearchwordAttribute.AddDate);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", SearchwordAttribute.IsEnabled);


            createBuilder.Length = createBuilder.Length - 2;
            createBuilder.Append(" )");

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.Append("INSERT INTO Searchwords (");

            preInsertBuilder.AppendFormat("[{0}], ", SearchwordAttribute.Searchword);
            preInsertBuilder.AppendFormat("[{0}], ", SearchwordAttribute.SearchResultCount);
            preInsertBuilder.AppendFormat("[{0}], ", SearchwordAttribute.SearchCount);
            preInsertBuilder.AppendFormat("[{0}], ", SearchwordAttribute.AddDate);
            preInsertBuilder.AppendFormat("[{0}], ", SearchwordAttribute.IsEnabled);

            preInsertBuilder.Length = preInsertBuilder.Length - 2;
            preInsertBuilder.Append(" ) VALUES (");

            //if (searchwordInfo.SearchResultCount < TranslateUtils.ToInt(this.tbSearchResultCount.Text))
            //    continue;
            //if (searchwordInfo.SearchCount < TranslateUtils.ToInt(this.tbSearchCount.Text))
            //    continue;
            ArrayList searchwordInfoArrayList = DataProvider.SearchwordDAO.GetSearchwordInfoArrayList(publishmentSystemID, string.Format(" SearchResultCount >= {0} AND SearchCount >= {1} ", searchResultCount, searchCount));

            ArrayList insertSqlArrayList = new ArrayList();
            foreach (SearchwordInfo searchwordInfo in searchwordInfoArrayList)
            {
                StringBuilder insertBuilder = new StringBuilder();
                insertBuilder.Append(preInsertBuilder.ToString());

                insertBuilder.AppendFormat("'{0}', ", SqlUtils.ToSqlString(StringUtils.StripTags(searchwordInfo.Searchword)));
                insertBuilder.AppendFormat("'{0}', ", searchwordInfo.SearchResultCount);
                insertBuilder.AppendFormat("'{0}', ", searchwordInfo.SearchCount);
                insertBuilder.AppendFormat("'{0}', ", searchwordInfo.AddDate);
                insertBuilder.AppendFormat("'{0}', ", searchwordInfo.IsEnabled);

                insertBuilder.Length = insertBuilder.Length - 2;
                insertBuilder.Append(") ");

                insertSqlArrayList.Add(insertBuilder.ToString());
            }

            OleDbConnection oConn = new OleDbConnection();

            oConn.ConnectionString = OLEDBConnStr;
            OleDbCommand oCreateComm = new OleDbCommand();
            oCreateComm.Connection = oConn;
            oCreateComm.CommandText = createBuilder.ToString();

            oConn.Open();
            oCreateComm.ExecuteNonQuery();
            foreach (string insertSql in insertSqlArrayList)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql;
                oInsertComm.ExecuteNonQuery();
            }
            oConn.Close();
        }
    }
}
