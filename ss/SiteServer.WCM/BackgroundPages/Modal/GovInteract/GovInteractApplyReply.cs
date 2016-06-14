using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using SiteServer.CMS.Model;
using System.Text;
using System.Web;
using BaiRong.Model;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WCM.Core;

namespace SiteServer.WCM.BackgroundPages.Modal
{
	public class GovInteractApplyReply : BackgroundBasePage
	{
        protected TextBox tbReply;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;
        public HtmlInputFile htmlFileUrl;

        private GovInteractContentInfo contentInfo;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, int contentID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            return PageUtilityWCM.GetOpenWindowString("回复办件", "modal_govInteractApplyReply.aspx", arguments, 600, 450);
        }
        
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ContentID");

            this.contentInfo = DataProvider.GovInteractContentDAO.GetContentInfo(base.PublishmentSystemInfo, TranslateUtils.ToInt(base.Request.QueryString["ContentID"]));

			if (!IsPostBack)
			{
                this.ltlDepartmentName.Text = AdminManager.CurrrentDepartmentName;
                this.ltlUserName.Text = AdminManager.DisplayName;
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
            try
            {
                DataProvider.GovInteractReplyDAO.DeleteByContentID(base.PublishmentSystemID, this.contentInfo.ID);
                string fileUrl = this.UploadFile(this.htmlFileUrl.PostedFile);
                GovInteractReplyInfo replyInfo = new GovInteractReplyInfo(0, base.PublishmentSystemID, this.contentInfo.NodeID, this.contentInfo.ID, this.tbReply.Text, fileUrl, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.GovInteractReplyDAO.Insert(replyInfo);

                GovInteractApplyManager.Log(base.PublishmentSystemID, this.contentInfo.NodeID, this.contentInfo.ID, EGovInteractLogType.Reply);
                if (AdminManager.Current.DepartmentID > 0)
                {
                    DataProvider.GovInteractContentDAO.UpdateDepartmentID(base.PublishmentSystemInfo, this.contentInfo.ID, AdminManager.Current.DepartmentID);
                }
                DataProvider.GovInteractContentDAO.UpdateState(base.PublishmentSystemInfo, this.contentInfo.ID, EGovInteractState.Replied);

                isChanged = true;
            }
			catch(Exception ex)
			{
                base.FailMessage(ex, ex.Message);
			    isChanged = false;
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPage(Page, string.Format("alert('办件回复成功!');"));
			}
		}

        private string UploadFile(HttpPostedFile myFile)
        {
            string fileUrl = string.Empty;

            if (myFile != null && !string.IsNullOrEmpty(myFile.FileName))
            {
                string filePath = myFile.FileName;
                try
                {
                    string fileExtName = PathUtils.GetExtension(filePath);
                    string localDirectoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);
                    string localFileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, filePath);

                    string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                    if (!PathUtility.IsFileExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                    {
                        return string.Empty;
                    }
                    if (!PathUtility.IsFileSizeAllowed(base.PublishmentSystemInfo, myFile.ContentLength))
                    {
                        return string.Empty;
                    }

                    myFile.SaveAs(localFilePath);
                    FileUtility.AddWaterMark(base.PublishmentSystemInfo, localFilePath);

                    fileUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                    fileUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, fileUrl);
                }
                catch { }
            }

            return fileUrl;
        }
	}
}
