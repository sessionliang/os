using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

using System.Text;
using System.Collections.Generic;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundContentTeleplayUpload : BackgroundBasePage
    {
        public Literal ltlScript;

        private string returnUrl = string.Empty;
        private int nodeID = 0;
        private int contentID = 0;

        public string GetContentTeleplayUploadMultipleUrl()
        {
            return PageUtility.ServiceSTL.AjaxUpload.GetContentTeleplayUploadMultipleUrl(base.PublishmentSystemID);
        }

        public string GetContentTeleplayUploadSingleUrl()
        {
            return PageUtility.ServiceSTL.AjaxUpload.GetContentTeleplayUploadSingleUrl(base.PublishmentSystemID);
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID, int contentID, string returnUrl)
        {
            return PageUtils.GetCMSUrl(string.Format("background_contentTeleplayUpload.aspx?publishmentSystemID={0}&nodeID={1}&contentID={2}&returnUrl={3}", publishmentSystemID, nodeID, contentID, StringUtils.ValueToUrl(returnUrl)));
        }

        public static string GetSetTaxisUrl(string type, int taxisID)
        {
            return PageUtils.GetCMSUrl(string.Format(PageUrl + "&type={0}&taxisID={1}", type, taxisID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ReturnUrl");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.nodeID = TranslateUtils.ToInt(base.GetQueryString("nodeID"));
            this.contentID = TranslateUtils.ToInt(base.GetQueryString("ContentID"));

            string type = base.GetQueryStringNoSqlAndXss("type");
            int taxisID = TranslateUtils.ToInt(base.GetQueryStringNoSqlAndXss("taxisID"));
            if (!string.IsNullOrEmpty(type))
            {
                if (type == "up")
                {
                    DataProvider.TeleplayDAO.UpdateTaxisToUp(base.PublishmentSystemID, this.contentID, taxisID);
                }
                else if (type == "down")
                {
                    DataProvider.TeleplayDAO.UpdateTaxisToDown(base.PublishmentSystemID, this.contentID, taxisID);
                }
            }
            if (!IsPostBack)
            {
                List<TeleplayInfo> TeleplayInfoList = new List<TeleplayInfo>();
                if (this.contentID > 0)
                {
                    TeleplayInfoList = DataProvider.TeleplayDAO.GetTeleplayInfoList(base.PublishmentSystemID, this.contentID);
                }

                StringBuilder scriptBuilder = new StringBuilder();

                
                foreach (TeleplayInfo TeleplayInfo in TeleplayInfoList)
                {
                    scriptBuilder.AppendFormat(@"
add_form({0}, '{1}', '{2}', '{3}', '{4}', '{5}', '<a class=""btn"" href=""javascript:;"" id=""preview_0"" onclick=""openWindow(\'预览视频\',\'{6}\',500,500,\'true\');return false;"" title=""预览""><i class=""icon-eye-open""></i></a>');
", TeleplayInfo.ID, StringUtils.ToJsString(PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, TeleplayInfo.StillUrl)), StringUtils.ToJsString(TeleplayInfo.Description), TeleplayInfo.Title, GetSetTaxisUrl("up", TeleplayInfo.ID), GetSetTaxisUrl("down", TeleplayInfo.ID), SiteServer.CMS.BackgroundPages.Modal.Message.GetRedirectStringToPreviewVideoByUrl(base.PublishmentSystemID, TeleplayInfo.StillUrl));
                }

                this.ltlScript.Text = string.Format(@"
$(document).ready(function(){{
	{0}
}});
", scriptBuilder.ToString());
            }
        }

        public string GetPreviewVideoSize()
        {
            return string.Format(@"width=""{0}"" height=""{1}""", 50, 50);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                List<int> contentIDList = new List<int>();
                if (this.contentID > 0)
                {
                    contentIDList = DataProvider.TeleplayDAO.GetTeleplayContentIDList(base.PublishmentSystemID, this.contentID);
                }
                int Teleplays = TranslateUtils.ToInt(base.Request.Form["Teleplay_Count"]);
                if (Teleplays > 0)
                {
                    for (int index = 1; index <= Teleplays; index++)
                    {
                        int id = TranslateUtils.ToInt(base.Request.Form["ID_" + index]);
                        string stillUrl = base.Request.Form["StillUrl_" + index];
                        string description = base.Request.Form["Description_" + index];
                        string title = base.Request.Form["Title_" + index];
                        if (!string.IsNullOrEmpty(stillUrl))
                        {
                            if (id > 0)
                            {
                                TeleplayInfo TeleplayInfo = DataProvider.TeleplayDAO.GetTeleplayInfo(id);
                                if (TeleplayInfo != null)
                                {
                                    TeleplayInfo.StillUrl = stillUrl;
                                    TeleplayInfo.Description = description;
                                    TeleplayInfo.Title = title;
                                    DataProvider.TeleplayDAO.Update(TeleplayInfo);
                                }
                                contentIDList.Remove(id);
                            }
                            else
                            {
                                TeleplayInfo TeleplayInfo = new TeleplayInfo(0, base.PublishmentSystemID, this.contentID, stillUrl, 0, description, title);

                                DataProvider.TeleplayDAO.Insert(TeleplayInfo);
                            }
                        }
                    }
                }

                if (contentIDList.Count > 0)
                {
                    DataProvider.TeleplayDAO.Delete(contentIDList);
                }

                string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeID);
                BaiRongDataProvider.ContentDAO.UpdateTeleplays(tableName, this.contentID, Teleplays);

                PageUtils.Redirect(this.returnUrl);
            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.returnUrl);
        }

        public static string PageUrl
        {
            get
            {
                return string.Format("background_contentTeleplayUpload.aspx?publishmentSystemID={0}&nodeID={1}&ContentID={2}&ReturnUrl={3}", RequestUtils.GetQueryString("publishmentSystemID"), RequestUtils.GetQueryString("nodeID"), RequestUtils.GetQueryString("ContentID"), RequestUtils.GetQueryString("ReturnUrl"));
            }
        }
    }
}
