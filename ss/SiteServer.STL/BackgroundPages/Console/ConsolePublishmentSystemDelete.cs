using System;
using System.Collections;
using System.Drawing;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core.Security;

namespace SiteServer.STL.BackgroundPages
{
	public class ConsolePublishmentSystemDelete : BackgroundBasePage
	{
        public PlaceHolder phIsRetainFiles;
		public RadioButtonList RetainFiles;
        public PlaceHolder phReturn;

		private int nodeID = 0;
        private bool isBackgroundDelete = false;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			this.nodeID = base.GetIntQueryString("nodeID");
            this.isBackgroundDelete = base.GetBoolQueryString("isBackgroundDelete");

			if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "删除应用", AppManager.Platform.Permission.Platform_Site);

                this.phIsRetainFiles.Visible = this.phReturn.Visible = !this.isBackgroundDelete;

                PublishmentSystemInfo psInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.nodeID);
                base.InfoMessage(string.Format("此操作将会删除应用“{0}({1})”，确认吗？", psInfo.PublishmentSystemName, psInfo.PublishmentSystemDir));
			}
		}

        public string GetReturnUrl()
        {
            return PageUtils.GetSTLUrl("console_publishmentSystem.aspx");
        }

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				try
				{
                    bool isRetainFiles = false;
                    if (!this.isBackgroundDelete)
                    {
                        isRetainFiles = TranslateUtils.ToBool(this.RetainFiles.SelectedValue);
                    }

                    if (isRetainFiles == false)
                    {
                        PublishmentSystemInfo psInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.nodeID);
                        DirectoryUtility.DeletePublishmentSystemFiles(psInfo);

                        base.SuccessMessage("成功删除应用以及相关文件！");                       
                    }
                    else
                    {
                        base.SuccessMessage("成功删除应用，相关文件未被删除！");
                    }

                    if (!this.isBackgroundDelete)
                    {
                        base.AddWaitAndRedirectScript(PageUtils.GetSTLUrl("console_publishmentSystem.aspx"));
                    }
                    else
                    {
                        base.AddScript(string.Format(@"setTimeout(""window.top.location.href='{0}'"", 1500);", PageUtils.GetAdminDirectoryUrl("main.aspx")));
                    }

                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.nodeID);
                    ArrayList arraylist = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(this.nodeID);
                    BaiRongDataProvider.TableStyleDAO.Delete(arraylist, publishmentSystemInfo.AuxiliaryTableForContent);
					DataProvider.NodeDAO.Delete(this.nodeID);

                    ProductPermissionsManager.Current.ClearCache();

                    if (publishmentSystemInfo.PublishmentSystemType == BaiRong.Model.EPublishmentSystemType.MLib)
                    {
                        //清空ml_submission
                        DataProvider.MlibDAO.ClearSubmission();
                    }

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除应用", string.Format("应用:{0}", publishmentSystemInfo.PublishmentSystemName));
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "删除系统失败！");
				}
			}
		}

	}
}
