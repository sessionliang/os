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
    public class BackgroundUserNewGroupMLibSite : BackgroundBasePage
    { 
        public PlaceHolder phMLibAddUser;
        public PlaceHolder phMLibValidityDate;
        public PlaceHolder phMlibNum;
        public PlaceHolder phIsMLibCheck;
        public PlaceHolder phMLibScope;

        public Label lbGroupName;
        public TextBox tbMLibAddUser; 
        public TextBox tbMLibValidityDate;
        public TextBox MlibNum;
        public RadioButtonList IsMLibCheck;
        public RadioButtonList rblIsUseMLibScope;
        public Literal ltlPublishmentSystems; 

        private SiteServer.CMS.Model.UserNewGroupInfo usergInfo;
        private int itemID; 

        public const string AllMLibPublishmentSystemArrayListKey = "AllMLibPublishmentSystemArrayListKey";
        public const string MLibScopeArrayListKey = "MLibScopeArrayListKey";

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            if (ConfigManager.Additional.IsUseMLib == false)
            {
                PageUtils.RedirectToErrorPage("投稿中心尚未开启.请在投稿设置启用投稿");
                return;
            }

            this.itemID = base.GetIntQueryString("ItemID");

            //ArrayList mLibPublishmentSystemArrayList = DataProvider.MLibScopeDAO.GetInfoList();
            //base.Session[AllMLibPublishmentSystemArrayListKey] = mLibPublishmentSystemArrayList;

            ArrayList hasInfoList = new ArrayList();

            if (!IsPostBack)
            { 
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserGroup,   "用户组投稿设置", AppManager.User.Permission.Usercenter_UserGroup);

                EBooleanUtils.AddListItems(this.IsMLibCheck, "是", "否");
                EBooleanUtils.AddListItems(this.rblIsUseMLibScope, "是", "否");

                #region 获取投稿设置的默认值 
                this.phMLibAddUser.Visible = ConfigManager.Additional.IsUnifiedMLibAddUser;
                this.phMLibValidityDate.Visible = ConfigManager.Additional.IsUnifiedMLibValidityDate;
                this.phMlibNum.Visible = ConfigManager.Additional.IsUnifiedMLibNum;
                this.phIsMLibCheck.Visible = ConfigManager.Additional.MLibCheckType;

                this.tbMLibAddUser.Text = ConfigManager.Additional.UnifiedMLibAddUser;

                this.IsMLibCheck.SelectedValue = (!ConfigManager.Additional.MLibCheckType).ToString();
                this.tbMLibValidityDate.Text = ConfigManager.Additional.UnifiedMLibValidityDate.ToString();
                this.MlibNum.Text = ConfigManager.Additional.UnifiedMlibNum.ToString();
                #endregion

                this.rblIsUseMLibScope.SelectedValue = true.ToString();

                //if (base.GetQueryString("Return") == null)
                //{
                //    ArrayList mLibScopeInfoList = new ArrayList();
                //    base.Session[MLibScopeArrayListKey] = mLibScopeInfoList;
                //}

                usergInfo = DataProvider.UserNewGroupDAO.GetContentInfo(this.itemID);
                if (usergInfo != null)
                {
                    this.lbGroupName.Text = usergInfo.ItemName;
                    if (ConfigManager.Additional.IsUseMLib && !string.IsNullOrEmpty(usergInfo.SetXML))
                    { 
                        this.tbMLibAddUser.Text = usergInfo.Additional.MLibAddUser;
                        this.tbMLibValidityDate.Text = usergInfo.Additional.MLibValidityDate.ToString();
                        this.MlibNum.Text = usergInfo.Additional.MlibNum.ToString();
                        this.rblIsUseMLibScope.SelectedValue = usergInfo.Additional.IsUseMLibScope.ToString();

                        //this.mLibPublishmentSystemIDs = usergInfo.Additional.MLibPublishmentSystemIDs;
                        //this.mLibScope = usergInfo.Additional.MLibScope;
                        //if (this.mLibScope!="null")
                        //{
                        //    ArrayList mLibScopeList = TranslateUtils.StringCollectionToArrayList(this.mLibScope);
                        //    ArrayList newMLibScopeList = new ArrayList();
                        //    foreach (string mLibScopeStr in mLibScopeList)
                        //    {
                        //        string[] mlibs = mLibScopeStr.Split('_');
                        //        MLibScopeInfo mLibScopeObj = new MLibScopeInfo();
                        //        mLibScopeObj.PublishmentSystemID = TranslateUtils.ToInt(mlibs[0]);
                        //        mLibScopeObj.NodeID = TranslateUtils.ToInt(mlibs[1]);
                        //        newMLibScopeList.Add(mLibScopeObj);
                        //    }
                        //    base.Session[MLibScopeArrayListKey] = newMLibScopeList;
                        //}
                    }
                }


            }

            //if (base.GetQueryString("Return") != null)
            //{ 
            //    this.rblIsUseMLibScope.SelectedValue = false.ToString();
            //    this.phMLibScope.Visible = true;
            //    ArrayList mLibScopeInfoList = (ArrayList)base.Session[MLibScopeArrayListKey];
            //    foreach (MLibScopeInfo mLibScopeInfo in mLibScopeInfoList)
            //    {
            //        if (!hasInfoList.Contains(mLibScopeInfo.PublishmentSystemID))
            //            hasInfoList.Add(mLibScopeInfo.PublishmentSystemID);
            //    }
            //    this.mLibPublishmentSystemIDs = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(hasInfoList);
            //}
            ////如果不使用默认的投稿范围
            //if (EBooleanUtils.Equals(EBoolean.False, this.rblIsUseMLibScope.SelectedValue))
            //{
            //    this.phMLibScope.Visible = EBooleanUtils.Equals(EBoolean.False, this.rblIsUseMLibScope.SelectedValue);
            //    ArrayList allPublishmentSystemIDArrayList = TranslateUtils.StringCollectionToArrayList(ConfigManager.Additional.MLibPublishmentSystemIDs);

            //    ArrayList mLibPublishmentSystemIDs = TranslateUtils.StringCollectionToArrayList(this.mLibPublishmentSystemIDs);

            //    ltlPublishmentSystems.Text = this.GetPublishmentSystemsHtml(allPublishmentSystemIDArrayList, mLibPublishmentSystemIDs);

            //}
        }



        public string GetPublishmentSystemsHtml(ArrayList allPublishmentSystemIDArrayList, ArrayList managedPublishmentSystemIDArrayList)
        {
            StringBuilder htmlBuilder = new StringBuilder();

            htmlBuilder.Append("<table width='100%' cellpadding='4' cellspacing='0' border='0'>");
            htmlBuilder.Append("<table width='100%' cellpadding='4' cellspacing='0' border='0'>");
            int count = 1;
            foreach (string pid in allPublishmentSystemIDArrayList)
            {
                int publishmentSystemID = TranslateUtils.ToInt(pid);
                PublishmentSystemInfo psInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                if (psInfo == null)
                    break;
                string imageName = "cantedit";
                if (managedPublishmentSystemIDArrayList.Contains(publishmentSystemID.ToString()))
                {
                    imageName = "canedit";
                }

                string space = "";
                if (count % 4 == 0)
                {
                    space = "<TR>";
                }

                string withGroupName = "";
                if (base.GetQueryString("GroupName") != null)
                {
                    withGroupName = "&GroupName=" + base.GetQueryString("GroupName");
                }

                string withItemID = "";
                if (base.GetQueryString("ItemID") != null)
                {
                    withItemID = "&ItemID=" + base.GetQueryString("ItemID");
                }

                string pageUrl = PageUtils.GetPlatformUrl(string.Format("background_userNewGroupMlibManageScope.aspx?PublishmentSystemID={0}{1}{2}", publishmentSystemID, withGroupName, withItemID));
                string content = string.Format(@"			<td height=20>                        <img id='PublishmentSystemImage_{0}' align='absmiddle' border='0' src='../pic/{1}.gif'/>					    <a href='{2}' onclick=="">{3}&nbsp;{4}</a>{5}                    </td>				", publishmentSystemID, imageName, pageUrl, psInfo.PublishmentSystemName, EPublishmentSystemTypeUtils.GetIconHtml(psInfo.PublishmentSystemType), space);
                htmlBuilder.Append(content);
                count++;
            }
            htmlBuilder.Append("</TABLE>");
            return htmlBuilder.ToString();
        }
        public override void Submit_OnClick(object sender, EventArgs E)
        {
            try
            {

                if (AdminManager.GetAdminInfo(this.tbMLibAddUser.Text) == null)
                {
                    base.FailMessage("保存失败：稿件发布者所输入的管理员不存在");
                    return;
                }
                if (AdminManager.GetAdminInfo(this.tbMLibAddUser.Text).IsLockedOut)
                {
                    base.FailMessage("保存失败：稿件发布者所输入的管理员被锁定，不能使用");
                    return;
                }
                if (!AdminManager.HasChannelPermissionByAdmin(this.tbMLibAddUser.Text))
                {
                    if (PublishmentSystemManager.GetPublishmentSystem(this.tbMLibAddUser.Text).Count == 0)
                    {
                        base.FailMessage("保存失败：管理员无站点与栏目权限，不可设置为稿件发布者");
                        return;
                    }
                }
                //ArrayList pidList = new ArrayList();
                //ArrayList pidNidList = new ArrayList();
                //if (!TranslateUtils.ToBool(this.rblIsUseMLibScope.SelectedValue))
                //{
                //    ArrayList mLibScopeInfoArrayList = (ArrayList)base.Session[MLibScopeArrayListKey];
                //    if (mLibScopeInfoArrayList.Count > 0)
                //    {
                //        foreach (MLibScopeInfo mLibScopeInfo in mLibScopeInfoArrayList)
                //        {
                //            if (!pidList.Contains(mLibScopeInfo.PublishmentSystemID))
                //                pidList.Add(mLibScopeInfo.PublishmentSystemID);
                //            pidNidList.Add(mLibScopeInfo.PublishmentSystemID + "_" + mLibScopeInfo.NodeID);
                //        }
                //    }
                //    else
                //    {
                //        base.FailMessage("保存失败：请设置投稿范围");
                //        return;
                //    }
                //}
                if (this.itemID > 0)
                {

                    usergInfo = DataProvider.UserNewGroupDAO.GetContentInfo(this.itemID);
                    usergInfo.Additional.IsUseMLibScope = TranslateUtils.ToBool(this.rblIsUseMLibScope.SelectedValue);
                    usergInfo.Additional.MLibAddUser = this.tbMLibAddUser.Text;
                    usergInfo.Additional.MlibNum = TranslateUtils.ToInt(this.MlibNum.Text);
                    usergInfo.Additional.MLibValidityDate = TranslateUtils.ToInt(this.tbMLibValidityDate.Text);
                    //usergInfo.Additional.MLibPublishmentSystemIDs = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(pidList);
                    //usergInfo.Additional.MLibScope = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(pidNidList);
                    usergInfo.SetXML = usergInfo.Additional.ToString();
                    DataProvider.UserNewGroupDAO.Update(usergInfo);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "用户组修改");
                    base.SuccessMessage("用户组修改成功");
                }
                else
                {
                    base.FailMessage("保存失败：未找到用户组");                
                }
                PageUtils.Redirect(string.Format("{0}&PublishmentSystemID={1}" ,PageUtils.GetPlatformUrl("background_userNewGroup.aspx?Return=True"),base.PublishmentSystemID));
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "保存失败：" + ex.Message);
            }
        }


        /// <summary>
        /// 是否使用投稿设置的投稿范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void rblIsUseMLibScope_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phMLibScope.Visible = EBooleanUtils.Equals(EBoolean.False, this.rblIsUseMLibScope.SelectedValue);
        }


        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(string.Format("{0}&PublishmentSystemID={1}" ,PageUtils.GetPlatformUrl("background_userNewGroup.aspx?Return=True"),base.PublishmentSystemID));
        }

    }
}
