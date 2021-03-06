using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;


using System.Collections.Generic; 

namespace BaiRong.BackgroundPages.Modal
{
    /// <summary>
    /// by 20160119 增加新的用户组功能
    /// 
    /// 将原有的AddToUserGroup功能改为用户类别
    /// 
    /// </summary>
    public class AddToUserNewGroup : BackgroundBasePage
    {
        protected DropDownList UserGroupIDDropDownList;

        private List<int> userIDList;

        public static string GetOpenWindowString()
        {
            NameValueCollection arguments = new NameValueCollection();
            return PageUtilityPF.GetOpenWindowStringWithCheckBoxValue("设置用户组", "modal_addToUserNewGroup.aspx", arguments, "UserIDCollection", "请选择需要设置组别的用户！", 400, 220);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.userIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserIDCollection"));

            if (!IsPostBack)
            {
                ArrayList userGroupInfoArrayList = BaiRongDataProvider.UserNewGroupDAO.GetInfoList(" ParentID !=0  ");
                foreach (UserNewGroupInfo theUserGroupInfo in userGroupInfoArrayList)
                {
                    ListItem listItem = new ListItem(theUserGroupInfo.ItemName, theUserGroupInfo.ItemID.ToString());
                    this.UserGroupIDDropDownList.Items.Add(listItem);
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            int groupID = TranslateUtils.ToInt(this.UserGroupIDDropDownList.SelectedValue);

            foreach (int userID in this.userIDList)
            {
                UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(userID);

                userInfo.NewGroupID = groupID;

                BaiRongDataProvider.UserDAO.Update(userInfo);  
            }

            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "设置用户组", string.Empty);

            JsUtils.OpenWindow.CloseModalPage(Page);
        }

    }
}
