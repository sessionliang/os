using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model.Service;
using BaiRong.Core.Service;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundPublishMain : BackgroundBasePage
    {
        public Literal ltlFrame;

        private EStorageClassify classify;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!Page.IsPostBack)
            {
                this.classify = EStorageClassifyUtils.GetEnumType(base.GetQueryString("classify"));
                bool isStorage = false;
                if (this.classify == EStorageClassify.Site)
                {
                    if (base.PublishmentSystemInfo.Additional.IsSiteStorage)
                    {
                        isStorage = true;
                    }
                }
                else if (this.classify == EStorageClassify.Image)
                {
                    if (base.PublishmentSystemInfo.Additional.IsImageStorage)
                    {
                        isStorage = true;
                    }
                }
                else if (this.classify == EStorageClassify.Video)
                {
                    if (base.PublishmentSystemInfo.Additional.IsVideoStorage)
                    {
                        isStorage = true;
                    }
                }
                else if (this.classify == EStorageClassify.File)
                {
                    if (base.PublishmentSystemInfo.Additional.IsFileStorage)
                    {
                        isStorage = true;
                    }
                }

                if (!isStorage)
                {
                    PageUtils.RedirectToErrorPage(string.Format("{0}存储发布不可用，未采用独立空间存储{0}文件。请到“设置管理”>“存储空间设置”>“{0}存储空间”中进行设置。", EStorageClassifyUtils.GetText(this.classify)));
                }
                else
                {
                    string localUrl = BackgroundPublishLocal.GetRedirectUrl(base.PublishmentSystemID, string.Empty, this.classify, string.Empty);
                    string remoteUrl = BackgroundPublishRemote.GetRedirectUrl(base.PublishmentSystemID, string.Empty, this.classify, string.Empty);
                    this.ltlFrame.Text = string.Format(@"
<frameset framespacing=""0"" border=""false"" cols=""50%,50%"" frameborder=""0"" scrolling=""yes"">
	<frame name=""local"" scrolling=""auto"" marginwidth=""0"" marginheight=""0"" src=""{0}"" >
	<frame name=""remote"" scrolling=""auto"" marginwidth=""0"" marginheight=""0"" src=""{1}"" >
</frameset>
", localUrl, remoteUrl);
                }
            }
        }
    }
}
