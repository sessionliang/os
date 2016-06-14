using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

using BaiRong.Model;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
    public class BackgroundCreateFile : BackgroundBasePage
    {
        public ListBox FileCollectionToCreate;
        public ListBox IncludeCollectionToCreate;
        public Button CreateFileButton;
        public Button DeleteAllFileButton;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Create, "生成文件页", AppManager.CMS.Permission.WebSite.Create);

                ArrayList templateInfoArrayList = DataProvider.TemplateDAO.GetTemplateInfoArrayListOfFile(base.PublishmentSystemID);

                foreach (TemplateInfo templateInfo in templateInfoArrayList)
                {
                    ListItem listitem = new ListItem(templateInfo.CreatedFileFullName, templateInfo.TemplateID.ToString());
                    FileCollectionToCreate.Items.Add(listitem);
                }

                string directoryPath = PathUtility.MapPath(base.PublishmentSystemInfo, "@/include");
                DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
                string[] fileNames = DirectoryUtils.GetFileNames(directoryPath);
                foreach (string fileName in fileNames)
                {
                    if (EFileSystemTypeUtils.IsTextEditable(EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(fileName))))
                    {
                        if (!fileName.Contains("_parsed"))
                        {
                            ListItem listitem = new ListItem(fileName, fileName);
                            IncludeCollectionToCreate.Items.Add(listitem);
                        }
                    }
                }

                this.DeleteAllFileButton.Attributes.Add("onclick", "return confirm(\"此操作将删除所有已生成的文件页面，确定吗？\");");
            }
        }

        public void CreateFileButton_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                ArrayList templateIDArrayList = new ArrayList();
                foreach (ListItem item in FileCollectionToCreate.Items)
                {
                    if (item.Selected)
                    {
                        int templateID = int.Parse(item.Value);
                        templateIDArrayList.Add(templateID);
                    }
                }
                ArrayList includeArrayList = new ArrayList();
                foreach (ListItem item in IncludeCollectionToCreate.Items)
                {
                    if (item.Selected)
                    {
                        includeArrayList.Add(item.Value);
                    }
                }
                ProcessCreateFile(templateIDArrayList, includeArrayList);
            }
        }

        public void DeleteAllFileButton_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                string url = PageUtils.GetSTLUrl(string.Format("background_progressBar.aspx?PublishmentSystemID={0}&DeleteAllPage=True&TemplateType={1}", base.PublishmentSystemID, ETemplateTypeUtils.GetValue(ETemplateType.FileTemplate)));
                PageUtils.RedirectToLoadingPage(url);
            }
        }

        private void ProcessCreateFile(ICollection templateIDArrayList, ICollection includeArrayList)
        {
            if (templateIDArrayList.Count == 0 && includeArrayList.Count == 0)
            {
                base.FailMessage("请选择需要生成的文件页！");
                return;
            }

            //if (!ConfigManager.Instance.Additional.IsSiteServerServiceCreate)
            //{
                string userKeyPrefix = StringUtils.GUID();

                DbCacheManager.Insert(userKeyPrefix + Constants.CACHE_CREATE_FILES_TEMPLATE_ID_ARRAYLIST, TranslateUtils.ObjectCollectionToString(templateIDArrayList));
                DbCacheManager.Insert(userKeyPrefix + Constants.CACHE_CREATE_FILES_INCLUDE_FILE_ARRAYLIST, TranslateUtils.ObjectCollectionToString(includeArrayList));
                string url = PageUtils.GetSTLUrl(string.Format("background_progressBar.aspx?PublishmentSystemID={0}&CreateFiles=True&UserKeyPrefix={1}", base.PublishmentSystemID, userKeyPrefix));
                PageUtils.Redirect(url);
            //}
            //else
            //{
            //    foreach (string templateID in templateIDArrayList)
            //    {
            //        AjaxUrlManager.AddCreateUrl(base.PublishmentSystemID, 0, 0, TranslateUtils.ToInt(templateID));
            //    }
            //    base.SuccessMessage("正在进行排队生成，您现在可以离开进行其他操作了！");
            //}
        }

    }
}
