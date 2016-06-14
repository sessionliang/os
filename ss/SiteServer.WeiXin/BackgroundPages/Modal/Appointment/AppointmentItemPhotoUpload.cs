using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;

using System.Text;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;
using System.Collections.Generic;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class AppointmentItemPhotoUpload : BackgroundBasePage
	{
        public Literal ltlScript;

        private string imageUrlCollection;
        private string largeImageUrlCollection;
        private ArrayList imageUrlArrayList=new ArrayList ();
        private ArrayList largeImageUrlArrayList=new ArrayList ();

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, string imageUrlCollection, string largeImageUrlCollection)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("imageUrlCollection", imageUrlCollection.ToString());
            arguments.Add("largeImageUrlCollection", largeImageUrlCollection.ToString());
            return PageUtilityWX.GetOpenWindowString("ÉÏ´«ÕÕÆ¬", "modal_appointmentItemPhotoUpload.aspx", arguments);
        }
        public string GetContentPhotoUploadMultipleUrl()
        {
            return BackgroundAjaxUpload.GetContentPhotoUploadMultipleUrl(base.PublishmentSystemID);
        }

        public string GetContentPhotoUploadSingleUrl()
        {
            return BackgroundAjaxUpload.GetContentPhotoUploadSingleUrl(base.PublishmentSystemID);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.imageUrlCollection = base.GetQueryString("imageUrlCollection");
            this.largeImageUrlCollection = base.GetQueryString("largeImageUrlCollection");

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.imageUrlCollection))
                { 
                    this.imageUrlArrayList = TranslateUtils.StringCollectionToArrayList(imageUrlCollection);
                    this.largeImageUrlArrayList = TranslateUtils.StringCollectionToArrayList(largeImageUrlCollection);
                }
                
                int index = -1;
                StringBuilder scriptBuilder = new StringBuilder();
                 
                foreach (string imageUrl in imageUrlArrayList)
                {
                    index++;
                    scriptBuilder.AppendFormat(@"
add_form({0}, '{1}', '{2}', '{3}');
", index + 1, StringUtils.ToJsString(PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, imageUrl)), StringUtils.ToJsString(imageUrl), StringUtils.ToJsString(largeImageUrlArrayList[index].ToString()));
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
            return string.Format(@"width=""{0}""", 200);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                int photo_Count = TranslateUtils.ToInt(base.Request.Form["Photo_Count"]);
                if (photo_Count > 0)
                {
                    for (int index = 1; index <= photo_Count; index++)
                    {
                        int id = TranslateUtils.ToInt(base.Request.Form["ID_" + index]);
                        string smallUrl = base.Request.Form["SmallUrl_" + index];
                        string largeUrl = base.Request.Form["LargeUrl_" + index];

                        if (!string.IsNullOrEmpty(smallUrl) && !string.IsNullOrEmpty(largeUrl))
                        {
                            this.imageUrlArrayList.Add(smallUrl);
                            this.largeImageUrlArrayList.Add(largeUrl);
                        }
                    }
                }

                if (this.imageUrlArrayList != null && this.imageUrlArrayList.Count > 0)
                {
                    this.imageUrlCollection = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(this.imageUrlArrayList);
                    this.largeImageUrlCollection = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(this.largeImageUrlArrayList);
                }

                string scripts = string.Format("window.parent.addImage('{0}', '{1}');", this.imageUrlCollection, this.largeImageUrlCollection);
                JsUtils.OpenWindow.CloseModalPageWithoutRefresh(Page, scripts);
            }
        }
    }
}
