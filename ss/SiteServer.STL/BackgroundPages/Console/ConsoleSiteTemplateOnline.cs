using System;
using System.Collections;
using System.IO;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

using BaiRong.Core.Net;
using BaiRong.Model;
using System.Xml;
using SiteServer.STL.ImportExport;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;

namespace SiteServer.STL.BackgroundPages
{
	public class ConsoleSiteTemplateOnline : BackgroundBasePage
	{
        protected override bool IsSinglePage
        {
            get { return true; }
        }

		public DataGrid dgContents;
        private ArrayList directoryNameLowerArrayList;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (base.GetQueryString("Delete") != null)
			{
                string siteTemplateDir = base.GetQueryString("SiteTemplateDir");
			
				try
				{
					SiteTemplateManager.Instance.DeleteSiteTemplate(siteTemplateDir);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除应用模板", string.Format("应用模板:{0}", siteTemplateDir));

					base.SuccessDeleteMessage();
				}
				catch(Exception ex)
				{
                    base.FailDeleteMessage(ex);
				}
			}

            this.directoryNameLowerArrayList = SiteTemplateManager.Instance.GetDirectoryNameLowerArrayList();
			
			if (!Page.IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "在线模板下载", AppManager.Platform.Permission.Platform_Site);

                try
                {
                    ArrayList arraylist = new ArrayList();

                    string content = WebClientUtils.GetRemoteFileSource(StringUtils.Constants.Url_Moban, ECharset.utf_8);

                    XmlDocument document = XmlUtils.GetXmlDocument(content);
                    XmlNode rootNode = XmlUtils.GetXmlNode(document, "//siteTemplates");
                    if (rootNode.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode node in rootNode.ChildNodes)
                        {
                            IEnumerator ie = node.ChildNodes.GetEnumerator();
                            string templateName = string.Empty;
                            string templateType = string.Empty;
                            string size = string.Empty;
                            string author = string.Empty;
                            string uploadDate = string.Empty;
                            string pageUrl = string.Empty;
                            string demoUrl = string.Empty;
                            string downloadUrl = string.Empty;

                            while (ie.MoveNext())
                            {
                                XmlNode childNode = (XmlNode)ie.Current;
                                string nodeName = childNode.Name;
                                if (StringUtils.EqualsIgnoreCase(nodeName, "templateName"))
                                {
                                    templateName = childNode.InnerText;
                                }
                                else if (StringUtils.EqualsIgnoreCase(nodeName, "templateType"))
                                {
                                    templateType = childNode.InnerText;
                                }
                                else if (StringUtils.EqualsIgnoreCase(nodeName, "size"))
                                {
                                    size = childNode.InnerText;
                                }
                                else if (StringUtils.EqualsIgnoreCase(nodeName, "author"))
                                {
                                    author = childNode.InnerText;
                                }
                                else if (StringUtils.EqualsIgnoreCase(nodeName, "uploadDate"))
                                {
                                    uploadDate = childNode.InnerText;
                                }
                                else if (StringUtils.EqualsIgnoreCase(nodeName, "pageUrl"))
                                {
                                    pageUrl = childNode.InnerText;
                                }
                                else if (StringUtils.EqualsIgnoreCase(nodeName, "demoUrl"))
                                {
                                    demoUrl = childNode.InnerText;
                                }
                                else if (StringUtils.EqualsIgnoreCase(nodeName, "downloadUrl"))
                                {
                                    downloadUrl = childNode.InnerText;
                                }
                            }

                            Hashtable hashtable = new Hashtable();
                            hashtable["templateName"] = templateName;
                            hashtable["templateType"] = templateType;
                            hashtable["size"] = size;
                            hashtable["author"] = author;
                            hashtable["uploadDate"] = uploadDate;
                            hashtable["pageUrl"] = pageUrl;
                            hashtable["demoUrl"] = demoUrl;
                            hashtable["downloadUrl"] = downloadUrl;
                            string directorName = PageUtils.GetFileNameFromUrl(downloadUrl);
                            directorName = directorName.Replace(".zip", string.Empty);
                            hashtable["directoryName"] = directorName;
                            arraylist.Add(hashtable);
                        }
                    }

                    this.dgContents.DataSource = arraylist;
                    this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                    this.dgContents.DataBind();
                }
                catch(Exception ex)
                {
                    base.FailMessage(ex, "在线模板获取失败：页面地址无法访问！");
                }
			}
		}

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Hashtable hashtable = (Hashtable)e.Item.DataItem;

                Literal ltlTemplateName = e.Item.FindControl("ltlTemplateName") as Literal;
                Literal ltlTemplateType = e.Item.FindControl("ltlTemplateType") as Literal;
                Literal ltlDirectoryName = e.Item.FindControl("ltlDirectoryName") as Literal; 
                Literal ltlSize = e.Item.FindControl("ltlSize") as Literal;
                Literal ltlAuthor = e.Item.FindControl("ltlAuthor") as Literal;
                Literal ltlUploadDate = e.Item.FindControl("ltlUploadDate") as Literal;
                Literal ltlPageUrl = e.Item.FindControl("ltlPageUrl") as Literal;
                Literal ltlDemoUrl = e.Item.FindControl("ltlDemoUrl") as Literal;
                Literal ltlDownloadUrl = e.Item.FindControl("ltlDownloadUrl") as Literal;

                ltlTemplateName.Text = hashtable["templateName"] as string;
                string templateType = hashtable["templateType"] as string;
                EPublishmentSystemType publishmentSystemType = EPublishmentSystemTypeUtils.GetEnumType(templateType);
                ltlTemplateType.Text = EPublishmentSystemTypeUtils.GetHtml(publishmentSystemType, false);
                string directoryName = hashtable["directoryName"] as string;
                ltlDirectoryName.Text = directoryName;
                ltlSize.Text = hashtable["size"] as string;
                ltlAuthor.Text = hashtable["author"] as string;
                ltlUploadDate.Text = hashtable["uploadDate"] as string;
                string pageUrl = hashtable["pageUrl"] as string;
                ltlPageUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">简介</a>", PageUtils.AddProtocolToUrl(pageUrl));
                string demoUrl = hashtable["demoUrl"] as string;
                ltlDemoUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">演示</a>", PageUtils.AddProtocolToUrl(demoUrl));
                if (this.directoryNameLowerArrayList.Contains(directoryName.ToLower().Trim()))
                {
                    ltlDownloadUrl.Text = "已下载";
                }
                else
                {
                    string downloadUrl = hashtable["downloadUrl"] as string;
                    downloadUrl = RuntimeUtils.EncryptStringByTranslate(downloadUrl);
                    ltlDownloadUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">下载并导入</a>", ProgressBar.GetOpenWindowStringWithSiteTemplateDownload(downloadUrl, directoryName));
                }
            }
        }
	}
}
