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
    public class BackgroundContentPhotoUpload : BackgroundBasePage
    {
        public Literal ltlScript;

        private string returnUrl = string.Empty;
        private int nodeID = 0;
        private int contentID = 0;

        public string GetContentPhotoUploadMultipleUrl()
        {
            return PageUtility.ServiceSTL.AjaxUpload.GetContentPhotoUploadMultipleUrl(base.PublishmentSystemID);
        }

        public string GetContentPhotoUploadSingleUrl()
        {
            return PageUtility.ServiceSTL.AjaxUpload.GetContentPhotoUploadSingleUrl(base.PublishmentSystemID);
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID, int contentID, string returnUrl)
        {
            return PageUtils.GetCMSUrl(string.Format("background_contentPhotoUpload.aspx?publishmentSystemID={0}&nodeID={1}&contentID={2}&returnUrl={3}", publishmentSystemID, nodeID, contentID, StringUtils.ValueToUrl(returnUrl)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ReturnUrl");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.nodeID = TranslateUtils.ToInt(base.GetQueryString("nodeID"));
            this.contentID = TranslateUtils.ToInt(base.GetQueryString("ContentID"));

            if (!IsPostBack)
            {
                List<PhotoInfo> photoInfoList = new List<PhotoInfo>();
                if (this.contentID > 0)
                {
                    photoInfoList = DataProvider.PhotoDAO.GetPhotoInfoList(base.PublishmentSystemID, this.contentID);
                }

                StringBuilder scriptBuilder = new StringBuilder();

                foreach (PhotoInfo photoInfo in photoInfoList)
                {
                    scriptBuilder.AppendFormat(@"
add_form({0}, '{1}', '{2}', '{3}', '{4}', '{5}');
", photoInfo.ID, StringUtils.ToJsString(PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, photoInfo.SmallUrl)), StringUtils.ToJsString(photoInfo.SmallUrl), StringUtils.ToJsString(photoInfo.MiddleUrl), StringUtils.ToJsString(photoInfo.LargeUrl), StringUtils.ToJsString(photoInfo.Description));
                }

                this.ltlScript.Text = string.Format(@"
$(document).ready(function(){{
	{0}
}});
", scriptBuilder.ToString());
            }
        }

        public string GetPreviewImageSize()
        {
            return string.Format(@"width=""{0}"" height=""{1}""", base.PublishmentSystemInfo.Additional.PhotoSmallWidth, base.PublishmentSystemInfo.Additional.PhotoSmallHeight);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                List<int> contentIDList = new List<int>();
                if (this.contentID > 0)
                {
                    contentIDList = DataProvider.PhotoDAO.GetPhotoContentIDList(base.PublishmentSystemID, this.contentID);
                }
                int photos = TranslateUtils.ToInt(base.Request.Form["Photo_Count"]);
                if (photos > 0)
                {
                    for (int index = 1; index <= photos; index++)
                    {
                        int id = TranslateUtils.ToInt(base.Request.Form["ID_" + index]);
                        string smallUrl = base.Request.Form["SmallUrl_" + index];
                        string middleUrl = base.Request.Form["MiddleUrl_" + index];
                        string largeUrl = base.Request.Form["LargeUrl_" + index];
                        string description = base.Request.Form["Description_" + index];

                        if (!string.IsNullOrEmpty(smallUrl) && !string.IsNullOrEmpty(middleUrl) && !string.IsNullOrEmpty(largeUrl))
                        {
                            if (id > 0)
                            {
                                PhotoInfo photoInfo = DataProvider.PhotoDAO.GetPhotoInfo(id);
                                if (photoInfo != null)
                                {
                                    photoInfo.SmallUrl = smallUrl;
                                    photoInfo.MiddleUrl = middleUrl;
                                    photoInfo.LargeUrl = largeUrl;
                                    photoInfo.Description = description;

                                    DataProvider.PhotoDAO.Update(photoInfo);
                                }
                                contentIDList.Remove(id);
                            }
                            else
                            {
                                PhotoInfo photoInfo = new PhotoInfo(0, base.PublishmentSystemID, this.contentID, smallUrl, middleUrl, largeUrl, 0, description);

                                DataProvider.PhotoDAO.Insert(photoInfo);
                            }
                        }
                    }
                }

                if (contentIDList.Count > 0)
                {
                    DataProvider.PhotoDAO.Delete(contentIDList);
                }

                string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeID);
                BaiRongDataProvider.ContentDAO.UpdatePhotos(tableName, this.contentID, photos);

                PageUtils.Redirect(this.returnUrl);
            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.returnUrl);
        }
    }
}
