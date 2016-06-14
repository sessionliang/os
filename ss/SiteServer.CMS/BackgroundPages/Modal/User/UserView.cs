using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;


using System;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class UserView : BackgroundBasePage
	{
        protected Literal ltlUserName;
        protected Literal ltlDisplayName;
        protected Literal ltlCreateDate;
        protected Literal ltlCreateIPAddress;
        protected Literal ltlGroup;
        protected Literal ltlLastActivityDate;
        protected Literal ltlEmail;
        protected Literal ltlMobile;

        protected DataList MyDataList;

        private UserInfo userInfo;

        public static string GetOpenWindowString(int publishmentSystemID, int userID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("userID", userID.ToString());
            return PageUtility.GetOpenWindowString("查看会员资料", "modal_userView.aspx", arguments, 600, 460, true);
        }

        public static string GetOpenWindowString(int publishmentSystemID, string userName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("userName", userName);
            return PageUtility.GetOpenWindowString("查看会员资料", "modal_userView.aspx", arguments, 600, 460, true);
        }
	
		public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            int userID = TranslateUtils.ToInt(base.GetQueryString("userID"));
            string userName = base.GetQueryString("userName");
            if (userID == 0)
            {
                this.userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(base.PublishmentSystemInfo.GroupSN, userName);
            }
            else
            {
                this.userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(userID);
            }

            if (this.userInfo == null)
            {
                PageUtils.RedirectToErrorPage("此用户不存在，可能被删除或已注销");
                return;
            }

            ArrayList styleInfoArrayList = TableStyleManager.GetUserTableStyleInfoArrayList(base.PublishmentSystemInfo.GroupSN);

            ArrayList arraylist = new ArrayList();
            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                if (styleInfo.IsVisible)
                {
                    Pair pair = new Pair(styleInfo.DisplayName, TableInputParser.GetContentByTableStyle(this.userInfo.Attributes[styleInfo.AttributeName.ToLower()], styleInfo));
                    arraylist.Add(pair);
                }
            }

            this.ltlUserName.Text = userInfo.UserName;
            this.ltlDisplayName.Text = userInfo.DisplayName;
            this.ltlCreateDate.Text = DateUtils.GetDateAndTimeString(userInfo.CreateDate);
            this.ltlCreateIPAddress.Text = userInfo.CreateIPAddress;
            this.ltlGroup.Text = UserGroupManager.GetGroupName(base.PublishmentSystemInfo.GroupSN, userInfo.GroupID);
            this.ltlLastActivityDate.Text = DateUtils.GetDateAndTimeString(userInfo.LastActivityDate);
            this.ltlEmail.Text = userInfo.Email;
            this.ltlMobile.Text = userInfo.Mobile;

            this.MyDataList.DataSource = arraylist;
            this.MyDataList.ItemDataBound += new DataListItemEventHandler(MyDataList_ItemDataBound);
            this.MyDataList.DataBind();
		}

        void MyDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Pair pair = (Pair)e.Item.DataItem;

                Literal ltlDataKey = e.Item.FindControl("ltlDataKey") as Literal;
                Literal ltlDataValue = e.Item.FindControl("ltlDataValue") as Literal;

                ltlDataKey.Text = pair.Key;
                ltlDataValue.Text = pair.Value.ToString();
            }
        }
	}
}
