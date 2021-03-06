using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections;

using System.Data.OleDb;
using System.Data;
using BaiRong.Core.AuxiliaryTable;

using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class UserImport : BackgroundBasePage
	{
        public RadioButtonList ImportType;
		public HtmlInputFile myFile;
        public RadioButtonList IsOverride;
        public TextBox ImportStart;
        public TextBox ImportCount;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("导入用户", "modal_userImport.aspx", arguments, 450, 350);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            if (base.IsForbidden) return;

		}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
			if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
			{
				try
				{
                    string filePath = myFile.PostedFile.FileName;
                    if (!StringUtils.EqualsIgnoreCase(PathUtils.GetExtension(filePath), ".xls"))
                    {
                        base.FailMessage("必须上传后缀为“.xls”的Excel文件");
                        return;
                    }

                    string localFilePath = PathUtils.GetTemporaryFilesPath(PathUtils.GetFileName(filePath));

                    myFile.PostedFile.SaveAs(localFilePath);

                    this.ImportContentsByExcelFile(localFilePath, TranslateUtils.ToBool(this.IsOverride.SelectedValue), TranslateUtils.ToInt(this.ImportStart.Text), TranslateUtils.ToInt(this.ImportCount.Text));

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "导入用户");

					JsUtils.OpenWindow.CloseModalPage(Page);
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, string.Format("导入用户失败，{0}", ex.Message));
				}
			}
		}

        public ArrayList GetUserInfoArrayListByExcelFile(string filePath)
        {
            ArrayList userInfoArrayList = new ArrayList();

            ArrayList al = new ArrayList();
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0;";
            OleDbConnection oleDbConn = new OleDbConnection(strConn);
            DataTable oleDt = new DataTable();
            DataTable dtTableName = new DataTable();

            oleDbConn.Open();
            dtTableName = oleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string[] tableNames = new string[dtTableName.Rows.Count];
            for (int j = 0; j < dtTableName.Rows.Count; j++)
            {
                tableNames[j] = dtTableName.Rows[j]["TABLE_NAME"].ToString();
            }
            OleDbCommand oleDbCmd = new OleDbCommand("SELECT * FROM [" + tableNames[0] + "]", oleDbConn);
            OleDbDataAdapter oleDbAdp = new OleDbDataAdapter(oleDbCmd);
            oleDbAdp.Fill(oleDt);
            oleDbConn.Close();

            if (oleDt.Rows.Count > 0)
            {
                ArrayList attributeNames = new ArrayList();
                for (int i = 0; i < oleDt.Columns.Count; i++)
                {
                    string columnName = oleDt.Columns[i].ColumnName;
                    attributeNames.Add(columnName);
                }

                foreach (DataRow row in oleDt.Rows)
                {
                    UserInfo userInfo = new UserInfo();

                    for (int i = 0; i < oleDt.Columns.Count; i++)
                    {
                        string attributeName = attributeNames[i] as string;
                        if (!string.IsNullOrEmpty(attributeName))
                        {
                            string value = row[i].ToString();
                            userInfo.SetExtendedAttribute(attributeName, value);
                        }
                    }

                    if (!string.IsNullOrEmpty(userInfo.UserName))
                    {
                        userInfo.Password = BaiRongDataProvider.UserDAO.DecodePassword(userInfo.Password, userInfo.PasswordFormat, userInfo.PasswordSalt);
                        userInfoArrayList.Add(userInfo);
                    }
                }
            }

            return userInfoArrayList;
        }

        public void ImportContentsByExcelFile(string excelFilePath, bool isOverride, int importStart, int importCount)
        {
            ArrayList userInfoArrayList = this.GetUserInfoArrayListByExcelFile(excelFilePath);

            if (importStart > 1 || importCount > 0)
            {
                ArrayList theArrayList = new ArrayList();

                if (importStart == 0)
                {
                    importStart = 1;
                }
                if (importCount == 0)
                {
                    importCount = userInfoArrayList.Count;
                }

                int firstIndex = userInfoArrayList.Count - importStart - importCount + 1;
                if (firstIndex <= 0)
                {
                    firstIndex = 0;
                }

                int addCount = 0;
                for (int i = 0; i < userInfoArrayList.Count; i++)
                {
                    if (addCount >= importCount) break;
                    if (i >= firstIndex)
                    {
                        theArrayList.Add(userInfoArrayList[i]);
                        addCount++;
                    }
                }

                userInfoArrayList = theArrayList;
            }

            foreach (UserInfo userInfo in userInfoArrayList)
            {
                string errorMessage;
                bool isCreate = BaiRongDataProvider.UserDAO.Insert(userInfo, out errorMessage);
                if (isCreate == false && isOverride)
                {
                    if (BaiRongDataProvider.UserDAO.IsUserExists(base.PublishmentSystemInfo.GroupSN, userInfo.UserName))
                    {
                        BaiRongDataProvider.UserDAO.Update(userInfo);
                    }
                }
            }
        }
	}
}
