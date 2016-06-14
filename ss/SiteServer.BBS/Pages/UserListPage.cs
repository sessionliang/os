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

namespace SiteServer.BBS.Pages
{
    public class UserListPage : BasePage
    {
        public Repeater rptUsers;

        protected int pageNum;
        protected PagerInfo pagerInfo;

        public void Page_Load(object sender, EventArgs e)
        {
            int pager = 21;
            this.pageNum = TranslateUtils.ToInt(base.Request.QueryString["page"], 1);
            int startNum = (this.pageNum - 1) * pager + 1;

            rptUsers.DataSource = BaiRongDataProvider.UserDAO.GetStlDataSource(startNum, pager, string.Empty, string.Empty);
            rptUsers.ItemDataBound += new RepeaterItemEventHandler(rptUsers_ItemDataBound);
            rptUsers.DataBind();

            int totalCount = BaiRongDataProvider.UserDAO.GetTotalCount();
            this.pagerInfo = PagerInfo.GetPagerInfo(totalCount, pager, base.Request, string.Empty);
        }

        void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlAvator = e.Item.FindControl("ltlAvator") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlSignature = e.Item.FindControl("ltlSignature") as Literal;
                Literal ltlAddon = e.Item.FindControl("ltlAddon") as Literal;

                string userName = TranslateUtils.EvalString(e.Item.DataItem, UserAttribute.UserName);
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
