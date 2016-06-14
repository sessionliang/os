using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Drawing;
using SiteServer.B2C.Core;
using BaiRong.Model;
using System.Text;
using SiteServer.B2C.Model;
using System.Collections;
using SiteServer.CMS.Core;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.B2C.BackgroundPages.Modal
{
	public class GoodsPhotoSelect : BackgroundBasePage
	{
        public Repeater rptPhotos;

        public Literal ltlScript;

        private int contentID;
        private string inputClientID;
        private string photosClientID;

        public static string GetOpenWindowString(int publishmentSystemID, int contentID, string inputClientID, string photosClientID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("InputClientID", inputClientID);
            arguments.Add("PhotosClientID", photosClientID);
            return PageUtilityB2C.GetOpenWindowString("关联商品相册图片", "modal_goodsPhotoSelect.aspx", arguments, 540, 350);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.contentID = base.GetIntQueryString("ContentID");
            this.inputClientID = base.GetQueryString("InputClientID");
            this.photosClientID = base.GetQueryString("PhotosClientID");

            if (!IsPostBack)
            {
                this.rptPhotos.DataSource = DataProvider.PhotoDAO.GetPhotoInfoList(base.PublishmentSystemID, this.contentID);
                this.rptPhotos.DataBind();
            }
		}

        public string ParseIconUrl(string iconUrl)
        {
            return PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, iconUrl);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
		{
            StringBuilder scriptBuilder = new StringBuilder();

            ArrayList photoIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.Form["PhotoIDCollection"]);
            if (photoIDArrayList.Count > 0)
            {

                ArrayList urlArrayList = new ArrayList();
                foreach (int photoID in photoIDArrayList)
                {
                    string url = base.Request.Form["Url_" + photoID];
                    if (!string.IsNullOrEmpty(url))
                    {
                        urlArrayList.Add(url);
                    }
                }

                if (!string.IsNullOrEmpty(this.inputClientID))
                {
                    scriptBuilder.AppendFormat(@"
if (parent.document.getElementById('{0}'))
{{
    parent.document.getElementById('{0}').value = '{1}';
}}
", this.inputClientID, base.Request.Form["PhotoIDCollection"].Replace(",", "_"));
                }
                if (!string.IsNullOrEmpty(this.photosClientID))
                {
                    string imgHTML = string.Empty;
                    foreach (string url in urlArrayList)
                    {
                        imgHTML += string.Format("<img src='{0}' />", url);
                    }
                    if (!string.IsNullOrEmpty(imgHTML))
                    {
                        imgHTML += "<br />";
                    }
                    scriptBuilder.AppendFormat(@"
if (parent.document.getElementById('{0}'))
{{
    parent.document.getElementById('{0}').innerHTML = ""{1}"";
}}
", this.photosClientID, imgHTML);
                }
            }
            scriptBuilder.Append(JsUtils.OpenWindow.HIDE_POP_WIN);

            this.ltlScript.Text = scriptBuilder.ToString();
		}

	}
}
