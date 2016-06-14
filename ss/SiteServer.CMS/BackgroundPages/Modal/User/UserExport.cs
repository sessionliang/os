using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections;
using System.Text;

using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data;
using System.Data.OleDb;

using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class UserExport : BackgroundBasePage
	{
        public PlaceHolder phExport;
        public RadioButtonList CheckedState;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("导出用户", "modal_userExport.aspx", arguments, 380, 250);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                this.phExport.Visible = true;
                ETriStateUtils.AddListItems(this.CheckedState, "全部", "审核通过", "未审核");
                ControlUtils.SelectListItems(this.CheckedState, ETriStateUtils.GetValue(ETriState.All));
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            this.phExport.Visible = false;

            string docFileName = "users.xls";
            string filePath = PathUtils.GetTemporaryFilesPath(docFileName);
            UserExport.CreateExcelFileForUsers(base.PublishmentSystemInfo.GroupSN, filePath, ETriStateUtils.GetEnumType(this.CheckedState.SelectedValue));

            HyperLink link = new HyperLink();
            link.NavigateUrl = PageUtils.GetTemporaryFilesUrl(docFileName);
            link.Text = "下载";
            string successMessage = "成功导出文件！&nbsp;&nbsp;" + ControlUtils.GetControlRenderHtml(link);
            base.SuccessMessage(successMessage);
		}

        public static void CreateExcelFileForUsers(string groupSN, string filePath, ETriState checkedState)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
            OLEDBConnStr += " Extended Properties=Excel 8.0;";
            StringBuilder createBuilder = new StringBuilder();

            createBuilder.Append("CREATE TABLE Users ( ");

            createBuilder.AppendFormat(" [{0}] NTEXT, ", UserAttribute.UserName);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", UserAttribute.Password);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", UserAttribute.PasswordFormat);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", UserAttribute.PasswordSalt);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", UserAttribute.GroupID);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", UserAttribute.Credits);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", UserAttribute.CreateDate);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", UserAttribute.CreateIPAddress);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", UserAttribute.LastActivityDate);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", UserAttribute.IsChecked);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", UserAttribute.IsLockedOut);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", UserAttribute.IsTemporary);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", UserAttribute.DisplayName);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", UserAttribute.Email);
            createBuilder.AppendFormat(" [{0}] NTEXT, ", UserAttribute.Mobile);

            ArrayList tableStyleInfoArrayList = TableStyleManager.GetUserTableStyleInfoArrayList(groupSN);

            foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
            {
                createBuilder.AppendFormat(" [{0}] NTEXT, ", tableStyleInfo.AttributeName);
            }

            createBuilder.Length = createBuilder.Length - 2;
            createBuilder.Append(" )");

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.Append("INSERT INTO Users (");

            preInsertBuilder.AppendFormat("[{0}], ", UserAttribute.UserName);
            preInsertBuilder.AppendFormat("[{0}], ", UserAttribute.Password);
            preInsertBuilder.AppendFormat("[{0}], ", UserAttribute.PasswordFormat);
            preInsertBuilder.AppendFormat("[{0}], ", UserAttribute.PasswordSalt);
            preInsertBuilder.AppendFormat("[{0}], ", UserAttribute.GroupID);
            preInsertBuilder.AppendFormat("[{0}], ", UserAttribute.Credits);
            preInsertBuilder.AppendFormat("[{0}], ", UserAttribute.CreateDate);
            preInsertBuilder.AppendFormat("[{0}], ", UserAttribute.CreateIPAddress);
            preInsertBuilder.AppendFormat("[{0}], ", UserAttribute.LastActivityDate);
            preInsertBuilder.AppendFormat("[{0}], ", UserAttribute.IsChecked);
            preInsertBuilder.AppendFormat("[{0}], ", UserAttribute.IsLockedOut);
            preInsertBuilder.AppendFormat("[{0}], ", UserAttribute.IsTemporary);
            preInsertBuilder.AppendFormat("[{0}], ", UserAttribute.DisplayName);
            preInsertBuilder.AppendFormat("[{0}], ", UserAttribute.Email);
            preInsertBuilder.AppendFormat("[{0}], ", UserAttribute.Mobile);

            foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
            {
                preInsertBuilder.AppendFormat("[{0}], ", tableStyleInfo.AttributeName);
            }

            preInsertBuilder.Length = preInsertBuilder.Length - 2;
            preInsertBuilder.Append(" ) VALUES (");

            ArrayList userInfoArrayList = BaiRongDataProvider.UserDAO.GetUserInfoArrayList(checkedState);

            ArrayList insertSqlArrayList = new ArrayList();
            foreach (UserInfo userInfo in userInfoArrayList)
            {
                StringBuilder insertBuilder = new StringBuilder();
                insertBuilder.Append(preInsertBuilder.ToString());

                insertBuilder.AppendFormat("'{0}', ", SqlUtils.ToSqlString(StringUtils.StripTags(userInfo.UserName)));
                insertBuilder.AppendFormat("'{0}', ", userInfo.Password);
                insertBuilder.AppendFormat("'{0}', ", userInfo.PasswordFormat);
                insertBuilder.AppendFormat("'{0}', ", userInfo.PasswordSalt);
                insertBuilder.AppendFormat("'{0}', ", userInfo.GroupID);
                insertBuilder.AppendFormat("'{0}', ", userInfo.Credits);
                insertBuilder.AppendFormat("'{0}', ", userInfo.CreateDate);
                insertBuilder.AppendFormat("'{0}', ", userInfo.CreateIPAddress);
                insertBuilder.AppendFormat("'{0}', ", userInfo.LastActivityDate);
                insertBuilder.AppendFormat("'{0}', ", userInfo.IsChecked);
                insertBuilder.AppendFormat("'{0}', ", userInfo.IsLockedOut);
                insertBuilder.AppendFormat("'{0}', ", userInfo.IsTemporary);
                insertBuilder.AppendFormat("'{0}', ", SqlUtils.ToSqlString(StringUtils.StripTags(userInfo.DisplayName)));
                insertBuilder.AppendFormat("'{0}', ", SqlUtils.ToSqlString(StringUtils.StripTags(userInfo.Email)));
                insertBuilder.AppendFormat("'{0}', ", SqlUtils.ToSqlString(StringUtils.StripTags(userInfo.Mobile)));

                foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
                {
                    string value = userInfo.GetExtendedAttribute(tableStyleInfo.AttributeName);
                    insertBuilder.AppendFormat("'{0}', ", SqlUtils.ToSqlString(StringUtils.StripTags(value)));
                }

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
