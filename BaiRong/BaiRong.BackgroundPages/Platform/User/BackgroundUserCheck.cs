using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;


using System.Collections.Generic;

namespace BaiRong.BackgroundPages
{
    public class BackgroundUserCheck : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button Check;
        public Button Delete;

        public static string GetRedirectUrl()
        {
            return PageUtils.GetPlatformUrl(string.Format("background_userCheck.aspx?"));
        }

        public static string GetViewHtml(int userID, string userName)
        {
            string showPopWinString = Modal.UserView.GetOpenWindowString(userName);
            return string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", showPopWinString, userName);
        }

        public string GetDateTime(DateTime datetime)
        {
            string retval = string.Empty;
            if (datetime > DateUtils.SqlMinValue)
            {
                retval = DateUtils.GetDateString(datetime);
            }
            return retval;
        }

        public string GetSendMailHtml(string email)
        {
            string showPopWinString = Modal.UserSendMail.GetOpenWindowStringToEmails(email);
            return string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", showPopWinString, email);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                List<int> userIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserIDCollection"));
                try
                {
                    foreach (int userID in userIDList)
                    {
                        BaiRongDataProvider.UserDAO.Delete(userID);
                    }

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "ɾ���û�", string.Empty);

                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }
            else if (base.GetQueryString("Check") != null)
            {
                List<int> userIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserIDCollection"));
                try
                {
                    BaiRongDataProvider.UserDAO.Check(userIDList);

                    base.BreadCrumb(AppManager.User.LeftMenu.ID_User, "����û�", AppManager.User.Permission.Usercenter_User);

                    base.SuccessCheckMessage();
                }
                catch (Exception ex)
                {
                    base.FailCheckMessage(ex);
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.ItemsPerPage = 25;
            this.spContents.SelectCommand = BaiRongDataProvider.UserDAO.GetSelectCommand(string.Empty, false);
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.SortField = BaiRongDataProvider.UserDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_User, "������û�", AppManager.User.Permission.Usercenter_User);

                this.spContents.DataBind();

                this.Check.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetPlatformUrl(string.Format("background_userCheck.aspx?Check=True")), "UserIDCollection", "UserIDCollection", "��ѡ����Ҫ��˵Ļ�Ա��", "�˲��������ͨ����ѡ��Ա��ȷ����"));

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetPlatformUrl(string.Format("background_userCheck.aspx?Delete=True")), "UserIDCollection", "UserIDCollection", "��ѡ����Ҫɾ���Ļ�Ա��", "�˲�����ɾ����ѡ��Ա��ȷ����"));
            }
        }

        private string GetUserNameHtml(int userID, string userName, bool isLockedOut)
        {
            string showPopWinString = Modal.UserView.GetOpenWindowString(userName);
            string state = string.Empty;
            if (isLockedOut)
            {
                state = @"<span style=""color:red;"">[�ѱ�����]</span>";
            }
            return string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>{2}", showPopWinString, userName, state);
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UserInfo userInfo = new UserInfo(e.Item.DataItem);

                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlDisplayName = (Literal)e.Item.FindControl("ltlDisplayName");
                Literal ltlUserGroupName = (Literal)e.Item.FindControl("ltlUserGroupName");
                Literal ltlCreateDate = (Literal)e.Item.FindControl("ltlCreateDate");
                Literal ltlSelect = (Literal)e.Item.FindControl("ltlSelect");
                HyperLink hlEditLink = (HyperLink)e.Item.FindControl("hlEditLink");

                ltlUserName.Text = this.GetUserNameHtml(userInfo.UserID, userInfo.UserName, userInfo.IsLockedOut);
                ltlDisplayName.Text = userInfo.DisplayName;

                ltlCreateDate.Text = DateUtils.GetDateAndTimeString(userInfo.CreateDate);

                string userAddUrl = BackgroundUserAdd.GetRedirectUrlToEdit(userInfo.UserID, BackgroundUserCheck.GetRedirectUrl());
                hlEditLink.NavigateUrl = userAddUrl;

                ltlSelect.Text = string.Format(@"<input type=""checkbox"" name=""UserIDCollection"" value=""{0}"" />", userInfo.UserID);
            }
        }
    }
}
