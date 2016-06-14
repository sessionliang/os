using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;

using BaiRong.Core.Data.Provider;

using BaiRong.Core.Configuration;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.OracleClient;
using BaiRong.Core.Data;
using BaiRong.Core.Service;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;
using SiteServer.CMS.Core.Security;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundUserNewGroupAdd : BackgroundBasePage
    {
        public TextBox tbUserGroupName;
        public TextBox tbDescription;

        private SiteServer.CMS.Model.UserNewGroupInfo usergInfo;
        private int itemID;
        private string mLibPublishmentSystemIDs;
        private string mLibScope;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.itemID = base.GetIntQueryString("ItemID");

            if (!IsPostBack)
            {
                string str = this.itemID != 0 ? "修改" : "添加";
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserGroup, str + "用户组", AppManager.User.Permission.Usercenter_UserGroup);



                usergInfo = DataProvider.UserNewGroupDAO.GetContentInfo(this.itemID);
                if (usergInfo != null)
                {
                    this.tbUserGroupName.Text = usergInfo.ItemName;
                    this.tbDescription.Text = usergInfo.Description;
                    this.tbUserGroupName.Enabled = true;
                }


            }
        }



        public override void Submit_OnClick(object sender, EventArgs E)
        {
            try
            {
                if (this.itemID > 0)
                {

                    usergInfo = DataProvider.UserNewGroupDAO.GetContentInfo(this.itemID);
                    usergInfo.Description = this.tbDescription.Text;
                    DataProvider.UserNewGroupDAO.Update(usergInfo);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "用户组修改");
                    base.SuccessMessage("用户组修改成功");
                }
                else
                {
                    if (DataProvider.UserNewGroupDAO.IsExists(this.tbUserGroupName.Text))
                    {
                        base.FailMessage("保存失败：用户组已存在");
                        return;
                    }
                    SiteServer.CMS.Model.UserNewGroupInfo info = new SiteServer.CMS.Model.UserNewGroupInfo();
                    info.ItemName = this.tbUserGroupName.Text;
                    info.Description = this.tbDescription.Text;

                    SiteServer.CMS.Model.UserNewGroupInfo pinfo = DataProvider.UserNewGroupDAO.GetDefaultInfo();
                    info.ParentID = pinfo.ItemID;

                    DataProvider.UserNewGroupDAO.Insert(info);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "用户组添加");
                    base.SuccessMessage("用户组添加成功");
                }
                PageUtils.Redirect(string.Format("{0}&PublishmentSystemID={1}", PageUtils.GetPlatformUrl("background_userNewGroup.aspx?Return=True"), base.PublishmentSystemID));
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "保存失败：" + ex.Message);
            }
        }


        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(string.Format("{0}&PublishmentSystemID={1}", PageUtils.GetPlatformUrl("background_userNewGroup.aspx?Return=True"), base.PublishmentSystemID));
        }

    }
}
