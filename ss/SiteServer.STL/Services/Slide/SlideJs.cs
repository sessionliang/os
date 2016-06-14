using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using SiteServer.CMS.Core.Security;

using SiteServer.CMS.Core;
using System.Collections.Generic;

namespace SiteServer.CMS.Services
{
    public class SlideJs : BasePage
    {
        public Literal ltlScript;

        public void Page_Load(object sender, System.EventArgs e)
        {
            int contentID = TranslateUtils.ToInt(base.Request.QueryString["contentID"]);

            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(ETableStyle.BackgroundContent, base.PublishmentSystemInfo.AuxiliaryTableForContent, contentID);

            List<PhotoInfo> photoInfoList = DataProvider.PhotoDAO.GetPhotoInfoList(base.PublishmentSystemID, contentID);

            StringBuilder builder = new StringBuilder();

            builder.Append(@"
var slide_data = {
");

            builder.AppendFormat(@"
    ""slide"":{{""title"":""{0}""}},
    ""images"":[
", StringUtils.ToJsString(contentInfo.Title));


            foreach (PhotoInfo photoInfo in photoInfoList)
            {
                builder.AppendFormat(@"
            {{""title"":""{0}"",""intro"":""{1}"",""thumb_50"":""{2}"",""thumb_160"":""{2}"",""image_url"":""{3}"",""id"":""{4}""}},", StringUtils.ToJsString(contentInfo.Title), StringUtils.ToJsString(photoInfo.Description), StringUtils.ToJsString(PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, photoInfo.SmallUrl)), StringUtils.ToJsString(PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, photoInfo.LargeUrl)), photoInfo.ID);
            }

            if (photoInfoList.Count > 0)
            {
                builder.Length -= 1;
            }
            
            builder.Append(@"
    ],
");

            builder.Append(@"
""next_album"":{""interface"":""http:\/\/slide.news.sina.com.cn\/interface\/slide_interface.php?ch=1&sid=17490&id=14751&range=\/d\/8"",""title"":""\u5e7f\u897f\u8d44\u6e90\u53bf\u8def\u9762\u7ed3\u51b0\u4ea4\u901a\u4e2d\u65ad\u6210\u5b64\u57ce"",""url"":""http:\/\/slide.news.sina.com.cn\/c\/slide_1_17490_14751.html\/d\/8"",""thumb_50"":""http:\/\/www.sinaimg.cn\/dy\/slidenews\/1_t50\/2011_01\/17490_68235_448162.jpg""},
	
	""prev_album"":{""interface"":""http:\/\/slide.news.sina.com.cn\/interface\/slide_interface.php?ch=1&sid=17490&id=14762&range=\/d\/8"",""title"":""\u65b0\u7586\u963f\u52d2\u6cf0\u6253\u901a\u88ab\u96ea\u5c01\u583540\u4f59\u5929\u7267\u9053"",""url"":""http:\/\/slide.news.sina.com.cn\/c\/slide_1_17490_14762.html\/d\/8"",""thumb_50"":""http:\/\/www.sinaimg.cn\/dy\/slidenews\/1_t50\/2011_01\/17490_68372_482939.jpg""}
");

            builder.Append(@"
}");

            this.ltlScript.Text = builder.ToString();
        }
    }
}
