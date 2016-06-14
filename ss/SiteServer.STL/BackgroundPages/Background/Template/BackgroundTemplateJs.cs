using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundTemplateJs : BackgroundBasePage
	{
		public DataGrid dgContents;

        public string PublishmentSystemUrl
        {
            get
            {
                return base.PublishmentSystemInfo.PublishmentSystemUrl;
            }
        }

        private string directoryPath;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.directoryPath = PathUtility.MapPath(base.PublishmentSystemInfo, "@/script");

			if (!base.IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Template, "脚本文件管理", AppManager.CMS.Permission.WebSite.Template);

				if (base.GetQueryString("Delete") != null)
				{
                    string fileName = base.GetQueryString("FileName");

					try
					{
                        FileUtils.DeleteFileIfExists(PathUtils.Combine(this.directoryPath, fileName));
                        StringUtility.AddLog(base.PublishmentSystemID, "删除脚本文件", string.Format("脚本文件:{0}", fileName));
						base.SuccessDeleteMessage();
					}
					catch(Exception ex)
					{
                        base.FailDeleteMessage(ex);
					}
				}
			}
			BindGrid();
		}

		public void BindGrid()
		{
			try
			{
				DirectoryUtils.CreateDirectoryIfNotExists(this.directoryPath);
                string[] fileNames = DirectoryUtils.GetFileNames(this.directoryPath);
                ArrayList fileNameArrayList = new ArrayList();
                foreach (string fileName in fileNames)
                {
                    if (EFileSystemTypeUtils.IsTextEditable(EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(fileName))))
                    {
                        if (!fileName.Contains("_parsed"))
                        {
                            fileNameArrayList.Add(fileName);
                        }
                    }
                }
                this.dgContents.DataSource = fileNameArrayList;
                this.dgContents.DataBind();
			}
			catch(Exception ex)
			{
                PageUtils.RedirectToErrorPage(ex.Message);
			}
		}

        public string GetCharset(string fileName)
        {
            ECharset charset = FileUtils.GetFileCharset(PathUtils.Combine(this.directoryPath, fileName));
            return ECharsetUtils.GetText(charset);
        }

	}
}
