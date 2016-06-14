using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System;

namespace BaiRong.BackgroundPages.Modal
{
    public class UserView : BackgroundBasePage
    {
        protected Literal ltlUserName;
        protected Literal ltlDisplayName;
        protected Literal ltlCreateDate;
        protected Literal ltlCreateIPAddress;
        protected Literal ltlCreateUserName;
        protected Literal ltlLastActivityDate;
        protected Literal ltlEmail;
        protected Literal ltlMobile;

        protected DataList MyDataList;

        private UserInfo userInfo;

        public static string GetOpenWindowString(string userName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("UserName", userName);
            return JsUtils.OpenWindow.GetOpenWindowString("查看会员资料", "../platform/modal_userView.aspx", arguments, 600, 460, true);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            string userName = base.Request.QueryString["UserName"];
            this.userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(string.Empty, userName);

            ArrayList relatedIdentities = new ArrayList();
            relatedIdentities.Add(0);
            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.User, BaiRongDataProvider.UserDAO.TABLE_NAME, relatedIdentities);

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
            //this.ltlCreateUserName.Text = userInfo.CreateUserName;
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
