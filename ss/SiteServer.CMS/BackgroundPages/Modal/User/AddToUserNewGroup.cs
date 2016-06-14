using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;


using System.Collections.Generic;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    /// <summary>
    /// by 20160119 �����µ��û��鹦��
    /// 
    /// ��ԭ�е�AddToUserGroup���ܸ�Ϊ�û����
    /// 
    /// </summary>
    public class AddToUserNewGroup : BackgroundBasePage
    {
        protected DropDownList UserGroupIDDropDownList;

        private List<int> userIDList;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowStringWithCheckBoxValue("�����û���", "modal_addToUserNewGroup.aspx", arguments, "UserIDCollection", "��ѡ����Ҫ���������û���", 400, 220);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.userIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserIDCollection"));

            if (!IsPostBack)
            {
                ArrayList userGroupInfoArrayList = DataProvider.UserNewGroupDAO.GetInfoList(" ParentID !=0 ");
                foreach (SiteServer.CMS.Model.UserNewGroupInfo theUserGroupInfo in userGroupInfoArrayList)
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

                DataProvider.UserNewGroupDAO.UpdateContentNum(groupID, 1);

            }

            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "�����û���", string.Empty);

            JsUtils.OpenWindow.CloseModalPage(Page);
        }

    }
}
