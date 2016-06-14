using BaiRong.Core;

using SiteServer.BBS.Core;

namespace SiteServer.BBS.Pages
{
    public class LogoutPage : BasePage
	{
        protected override bool IsAccessable
        {
            get { return true; }
        }

        public string GetReturnUrl()
        {
            string returnUrl = PageUtils.GetReturnUrl(false);
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "default.aspx";
            }
            return PageUtils.ParseNavigationUrl(returnUrl);
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            int publishmentSystemID = base.GetIntQueryString("publishmentSystemID");
            DataProvider.OnlineDAO.DeleteByUserName(publishmentSystemID, BaiRongDataProvider.UserDAO.CurrentUserName);
        }
	}
}
