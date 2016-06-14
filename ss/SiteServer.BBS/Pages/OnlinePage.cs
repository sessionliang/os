using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;


using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using System.Collections.Specialized;
using BaiRong.Model;
using System.Data;
using System.Collections;

namespace SiteServer.BBS.Pages
{
    public class OnlinePage : BasePage
    {
        public Repeater rptUsers;

        public void Page_Load(object sender, EventArgs e)
        {
            int onlineCount = OnlineManager.GetOnlineCount(base.PublishmentSystemID);
            ArrayList userNameArrayList = OnlineManager.GetUserNameArrayList(base.PublishmentSystemID);
            int onlineAnonymousCount = onlineCount - userNameArrayList.Count;

            rptUsers.DataSource = userNameArrayList;
            rptUsers.ItemDataBound += new RepeaterItemEventHandler(rptUsers_ItemDataBound);
            rptUsers.DataBind();
        }

        void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlAvator = e.Item.FindControl("ltlAvator") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlSignature = e.Item.FindControl("ltlSignature") as Literal;
                Literal ltlAddon = e.Item.FindControl("ltlAddon") as Literal;

                string userName = e.Item.DataItem as string;

                UserInfo userInfo = UserManager.GetUserInfo(base.PublishmentSystemInfo.GroupSN, userName);
                string userUrl = base.UserUtils.GetUserUrl(userName);

                ltlAvator.Text = string.Format(@"<a href=""{0}"" target=""_blank""><img src=""{1}""> </a>", userUrl, UserUtils.GetUserAvatarSmallUrl(userName));
                ltlUserName.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", userUrl, userName);
                ltlSignature.Text = base.UserUtils.GetSignature(userName);
                if (string.IsNullOrEmpty(ltlSignature.Text))
                {
                    ltlSignature.Text = userInfo.Signature;
                }
                if (!string.IsNullOrEmpty(ltlSignature.Text))
                {
                    ltlSignature.Text = StringUtils.StripTags(ltlSignature.Text);
                }
                ltlAddon.Text = string.Format("зЂВс: {0}", DateUtils.GetDateAndTimeString(userInfo.CreateDate));
            }
        }
    }
}
