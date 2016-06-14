using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CRM.Core;
using SiteServer.CRM.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages.Modal
{
	public class DocumentAdd : BackgroundBasePage
	{
        public HtmlInputFile myFile;
        public DropDownList ddlVersion;
        public TextBox tbDescription;

        private int contractID;
        private int typeID;

        public static string GetShowPopWinStringToAddByCategory(int typeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("TypeID", typeID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加文档", "modal_documentAdd.aspx", arguments, 450, 300);
        }

        public static string GetShowPopWinStringToAddByContract(int contractID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ContractID", contractID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加文档", "modal_documentAdd.aspx", arguments, 450, 300);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.typeID = TranslateUtils.ToInt(base.Request.QueryString["TypeID"]);
            this.contractID = TranslateUtils.ToInt(base.Request.QueryString["ContractID"]);

			if (!IsPostBack)
			{
                for (int i = 1; i <= 10; i++)
                {
                    ListItem listItem = new ListItem("V" + i, "V" + i);
                    this.ddlVersion.Items.Add(listItem);
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
            {
                string fileName = myFile.PostedFile.FileName;
                try
                {
                    string filePath = string.Empty;
                    if (this.typeID > 0)
                    {
                        filePath = PathUtils.Combine(DocumentManager.GetDirectoryPathByCategory(this.typeID), fileName);
                    }
                    else if (this.contractID > 0)
                    {
                        filePath = PathUtils.Combine(DocumentManager.GetDirectoryPathByContract(this.contractID), fileName);
                    }
                    if (FileUtils.IsFileExists(filePath))
                    {
                        base.FailMessage("文件上传失败，已存在同名文件！");
                        return;
                    }
                    else
                    {
                        DirectoryUtils.CreateDirectoryIfNotExists(filePath);
                        myFile.PostedFile.SaveAs(filePath);

                        EDocumentType documentType = EDocumentType.Category;
                        if (this.contractID > 0)
                        {
                            documentType = EDocumentType.Contract;
                        }
                        DocumentInfo documentInfo = new DocumentInfo(0, documentType, this.contractID, this.typeID, fileName, this.ddlVersion.SelectedValue, this.tbDescription.Text, AdminManager.Current.UserName, DateTime.Now);
                        DataProvider.DocumentDAO.Insert(documentInfo);

                        isChanged = true;
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "文件上传失败！");
                    return;
                }
            }

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, string.Format("background_document.aspx?ContractID={0}&TypeID={1}", this.contractID, this.typeID));
			}
		}
	}
}
