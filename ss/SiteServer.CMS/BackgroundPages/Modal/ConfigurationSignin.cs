using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using BaiRong.Model;
using System.Collections.Generic;
using BaiRong.Controls;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    //设置签收项
    public class ConfigurationSignin : BackgroundBasePage
	{
        public RadioButtonList TypeList;

        public PlaceHolder phGroup;
        public CheckBoxList GroupIDList;

        public PlaceHolder phUser;
        public TextBox UserNameList;

        public DropDownList Priority;
        public DateTimeTextBox EndDate;       

        private int nodeID;
        
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"));

			if (!IsPostBack)
			{
                ESigninPriorityUtils.AddListItems(this.Priority);
                ControlUtils.SelectListItemsIgnoreCase(this.Priority, ESigninPriorityUtils.GetValue(ESigninPriority.Normal));
                ListItem listItem = new ListItem("用户组", "Group");
                listItem.Selected = true;
                this.TypeList.Items.Add(listItem);
                listItem = new ListItem("用户", "User");
                this.TypeList.Items.Add(listItem);

                //foreach (UserGroupInfo groupInfo in UserGroupManager.GetGroupInfoArrayList())
                //{
                //    this.GroupIDList.Items.Add(new ListItem(groupInfo.GroupName, groupInfo.GroupID.ToString()));
                //}
                #region 编辑初始化

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);

                if (nodeInfo.Additional.IsSignin)
                {
                    if (nodeInfo.Additional.IsSigninGroup)
                    {
                        this.phGroup.Visible = true;
                        this.phUser.Visible = false;
                        this.TypeList.SelectedValue = "Group";
                        ArrayList groupIDArrayList = TranslateUtils.StringCollectionToIntArrayList(nodeInfo.Additional.SigninUserGroupCollection);
                        foreach (int groupID in groupIDArrayList)
                        {
                            this.GroupIDList.Items[groupID-1].Selected = true;
                        }
                    }
                    else
                    {
                        this.phGroup.Visible = false;
                        this.phUser.Visible = true;
                        this.TypeList.SelectedValue = "User";
                        this.UserNameList.Text = nodeInfo.Additional.SigninUserNameCollection;
                    }
                    this.Priority.SelectedValue = nodeInfo.Additional.SigninPriority.ToString();
                    this.EndDate.Text = nodeInfo.Additional.SigninEndDate.ToString();

                }
                #endregion
				
			}
		}

        public void TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.TypeList.SelectedValue == "Group")
            {
                this.phGroup.Visible = true;
                this.phUser.Visible = false;
            }
            else
            {
                this.phGroup.Visible = false;
                this.phUser.Visible = true;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);

                nodeInfo.Additional.SigninPriority = TranslateUtils.ToInt(this.Priority.SelectedValue);
                nodeInfo.Additional.SigninEndDate = this.EndDate.Text;
                if (this.TypeList.SelectedValue == "Group")
                {
                    string GroupID = "";
                    for (int i = 0; i < this.GroupIDList.Items.Count; i++)
                    {
                        if (this.GroupIDList.Items[i].Selected)
                        {
                            GroupID += this.GroupIDList.Items[i].Value + ","; //用户组列表
                        }
                    }
                    nodeInfo.Additional.SigninUserGroupCollection = GroupID.TrimEnd(',');
                    nodeInfo.Additional.SigninUserNameCollection = "";
                    nodeInfo.Additional.IsSigninGroup = true;
                    
                }
                else if (this.TypeList.SelectedValue == "User")
                {
                    nodeInfo.Additional.SigninUserGroupCollection = "";
                    nodeInfo.Additional.SigninUserNameCollection = UserNameList.Text.Trim();
                    nodeInfo.Additional.IsSigninGroup = false;
                }
                
                DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);
                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
                isChanged = false;
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageWithoutRefresh(Page, "alert('内容签收设置成功！');");
            }
        }

	}
}
