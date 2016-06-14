using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;

using SiteServer.BBS.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundCreateFile : BackgroundBasePage
	{
        public ListBox FileCollectionToCreate;
        public Button CreateFileButton;

        private void AddFileNameArrayList(ArrayList fileNameArrayList, string directoryPath, string directoryName)
        {
            if (StringUtils.EndsWithIgnoreCase(directoryName, "template")) return;
            string[] filePaths = DirectoryUtils.GetFilePaths(directoryPath);
            if (filePaths != null && filePaths.Length > 0)
            {
                foreach (string filePath in filePaths)
                {
                    string fileName = PathUtils.GetFileName(filePath).ToLower();
                    if (fileName.EndsWith("template.xml") || fileName == "default.aspx")
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(directoryName))
                    {
                        fileNameArrayList.Add(fileName);
                    }
                    else
                    {
                        fileNameArrayList.Add(directoryName + "/" + fileName);
                    }
                }
            }
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Create, "生成文件页", AppManager.BBS.Permission.BBS_Create);

                ArrayList fileNameArrayList = new ArrayList();
                string directoryPath = PathUtilityBBS.GetTemplateDirectoryPath(base.PublishmentSystemID);

                this.AddFileNameArrayList(fileNameArrayList, directoryPath, string.Empty);
                string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(directoryPath);
                if (directoryPaths != null && directoryPaths.Length > 0)
                {
                    foreach (string dirPath in directoryPaths)
                    {
                        this.AddFileNameArrayList(fileNameArrayList, dirPath, PathUtils.GetDirectoryName(dirPath));
                    }
                }

                if (fileNameArrayList.Count == 0)
				{
					this.CreateFileButton.Enabled = false;
				}
				else
				{
                    foreach (string fileName in fileNameArrayList)
					{
                        ListItem listitem = new ListItem(fileName, fileName);
						FileCollectionToCreate.Items.Add(listitem);
					}
				}
			}
		}

        public void CreateFileButton_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				ArrayList fileNameArrayList = new ArrayList();
				foreach (ListItem item in FileCollectionToCreate.Items)
				{
					if (item.Selected)
					{
                        fileNameArrayList.Add(item.Value);
					}
				}

                if (fileNameArrayList.Count == 0)
                {
                    base.FailMessage("请选择需要生成的文件页！");
                    return;
                }

                string userKeyPrefix = StringUtils.GUID();

                DbCacheManager.Insert(userKeyPrefix + "FileNameCollection", TranslateUtils.ObjectCollectionToString(fileNameArrayList));
                PageUtils.Redirect(BackgroundProgressBar.GetCreateFilesUrl(base.PublishmentSystemID, userKeyPrefix));
			}
		}
	}
}
