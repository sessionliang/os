using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core;
using BaiRong.Core.Cryptography;


namespace SiteServer.BBS.Pages
{
    public class RegisterSucess : BasePage
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            string checkCode = PageUtils.FilterSqlAndXss(base.Request.QueryString["checkCode"]);
            string userName = PageUtils.FilterSqlAndXss(base.Request.QueryString["userName"]);
            if (!string.IsNullOrEmpty(checkCode) && !string.IsNullOrEmpty(userName))
            {
                if (EncryptUtils.Md5(userName) == checkCode)
                {
                    int userID = BaiRongDataProvider.UserDAO.GetUserID(base.PublishmentSystemInfo.GroupSN, userName);
                    BaiRongDataProvider.UserDAO.Check(userID);
                    return;
                }
            }
        }
    }
}
