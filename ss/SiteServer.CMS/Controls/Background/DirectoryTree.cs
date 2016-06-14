using System;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Core.IO.FileManagement;
using BaiRong.Model;
using BaiRong.Controls;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Collections;

using SiteServer.CMS.Core.Security;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CMS.Controls
{
	public class DirectoryTree : Control
	{
        private int publishmentSystemID = 0;
        private PublishmentSystemInfo publishmentSystemInfo;
		string rightPageURL = string.Empty;

        protected override void Render(HtmlTextWriter writer)
        {
            this.rightPageURL = base.Page.Request.QueryString["RightPageURL"];
            this.publishmentSystemID = TranslateUtils.ToInt(base.Page.Request.QueryString["PublishmentSystemID"]);

            StringBuilder builder = new StringBuilder();

            string scripts = NavigationTreeItem.GetNodeTreeScript();
            builder.Append(scripts);           

            if (ProductPermissionsManager.Current.PublishmentSystemIDList.Contains(publishmentSystemID))
            {
                this.publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                if (this.publishmentSystemInfo != null)
                {
                    try
                    {
                        string path = PathUtility.MapPath(this.publishmentSystemInfo, string.Empty);
                        if (DirectoryUtils.IsDirectoryExists(path))
                        {
                            FileSystemInfoExtendCollection fsies = FileManager.GetFileSystemInfoExtendCollection(path, false);
                            string directoryName = PathUtils.GetDirectoryName(path);
                            int parentsCount = 0;
                            builder.Append(this.GetItemHtml(true, string.Empty, directoryName, parentsCount, true));

                            if (fsies.Folders.Count > 0)
                            {
                                parentsCount++;
                                foreach (FileSystemInfoExtend folder in fsies.Folders)
                                {
                                    if (!StringUtils.EqualsIgnoreCase(folder.Name, "api"))
                                    {
                                        builder.Append(this.GetItemHtml(true, folder.Name, folder.Name, parentsCount, false));
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        PageUtils.RedirectToErrorPage(ex.Message);
                    }

                    writer.Write(builder);
                }
            }
		}

        private string GetItemHtml(bool isDisplay, string relatedPath, string text, int parentsCount, bool hasChildren)
		{
            string linkUrl = BackgroundFileManagement.GetRedirectUrl(this.publishmentSystemID, relatedPath);
			string target = "content";
            bool selected = string.IsNullOrEmpty(relatedPath);
			
			ENodeType nodeType = (selected) ? ENodeType.BackgroundPublishNode : ENodeType.BackgroundNormalNode;
            DirectoryTreeItem directoryItem = DirectoryTreeItem.CreateDirectoryTreeItem(isDisplay, selected, parentsCount, hasChildren, text, linkUrl, string.Empty, target, true, true);
            return directoryItem.GetTrHtml();
		}
	}
}