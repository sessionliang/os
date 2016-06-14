using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal
{
    public class ConfigurationCreateInclude : BackgroundBasePage
	{
        protected ListBox IncludeFileCollection;

		private int nodeID;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID)
        {
            return ChannelLoading.GetConfigurationCreateIncludeOpenWindowString(publishmentSystemID, nodeID);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID");
            this.nodeID = base.GetIntQueryString("NodeID");

			if (!IsPostBack)
			{
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);

                string directoryPath = PathUtility.MapPath(base.PublishmentSystemInfo, "@/include");
                DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
                string[] fileNames = DirectoryUtils.GetFileNames(directoryPath);
                foreach (string fileName in fileNames)
                {
                    if (EFileSystemTypeUtils.IsTextEditable(EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(fileName))))
                    {
                        ListItem listitem = new ListItem(fileName, fileName);
                        this.IncludeFileCollection.Items.Add(listitem);
                    }
                }

                ControlUtils.SelectListItems(this.IncludeFileCollection, TranslateUtils.StringCollectionToArrayList(nodeInfo.Additional.CreateIncludeFilesIfContentChanged));

				
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isSuccess = false;

            try
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);

                nodeInfo.Additional.CreateIncludeFilesIfContentChanged = ControlUtils.GetSelectedListControlValueCollection(this.IncludeFileCollection);

                DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);

                StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "设置栏目变动生成页面", string.Format("栏目:{0}", nodeInfo.NodeName));
                isSuccess = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }

            if (isSuccess)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetCMSUrl(string.Format("background_configurationCreateTrigger.aspx?PublishmentSystemID={0}&CurrentNodeID={1}", base.PublishmentSystemID, this.nodeID)));
            }
        }
	}
}
