using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Core.IO;
using BaiRong.Core.Data.Provider;


using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using BaiRong.Model;

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundPermission : BackgroundBasePage
    {
        public RadioButtonList rblIsAllowVisit;
        public RadioButtonList rblIsAllowHide;
        public RadioButtonList rblIsAllowSignature;
        public RadioButtonList rblSearchType;
        public TextBox tbSearchInterval;

        public RadioButtonList rblIsAllowRead;
        public RadioButtonList rblIsAllowPost;
        public RadioButtonList rblIsAllowReply;
        public RadioButtonList rblIsAllowPoll;
        public TextBox tbMaxPostPerDay;
        public TextBox tbPostInterval;

        public RadioButtonList rblUploadType;
        public RadioButtonList rblDownloadType;
        public RadioButtonList rblIsAllowSetAttachmentPermissions;
        public TextBox tbMaxSize;
        public TextBox tbMaxSizePerDay;
        public TextBox tbMaxNumPerDay;
        public TextBox tbAttachmentExtensions;

        public ListBox lbHorizontalGroup;

        private int groupID;

        public static string GetRedirectUrl(int publishmentSystemID, int groupID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_permission.aspx?publishmentSystemID={0}&groupID={1}", publishmentSystemID, groupID));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            this.groupID = base.GetIntQueryString("GroupID");

            if (!base.IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_User, "用户组权限设置", AppManager.BBS.Permission.BBS_User);

                //UserGroupInfo groupInfo = UserGroupManager.GetGroupInfo(base.PublishmentSystemInfo.GroupSN, this.groupID);
                UserGroupInfo groupInfo = UserGroupManager.GetGroupInfo(string.Empty, this.groupID);
                if (groupInfo != null)
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowHide, groupInfo.Additional.IsAllowHide.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowVisit, groupInfo.Additional.IsAllowVisit.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowSignature, groupInfo.Additional.IsAllowSignature.ToString());
                    ETriStateUtils.AddListItems(this.rblSearchType, "允许搜索主题标题、内容", "允许搜索主题标题", "不允许");
                    ControlUtils.SelectListItemsIgnoreCase(this.rblSearchType, groupInfo.Additional.SearchType);
                    this.tbSearchInterval.Text = groupInfo.Additional.SearchInterval.ToString();

                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowRead, groupInfo.Additional.IsAllowRead.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowPost, groupInfo.Additional.IsAllowPost.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowReply, groupInfo.Additional.IsAllowReply.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowPoll, groupInfo.Additional.IsAllowPoll.ToString());
                    this.tbMaxPostPerDay.Text = groupInfo.Additional.MaxPostPerDay.ToString();
                    this.tbPostInterval.Text = groupInfo.Additional.PostInterval.ToString();

                    ETriStateUtils.AddListItems(this.rblUploadType, "允许上传附件，不奖励或扣除积分", "允许上传附件，按照版块设置奖励或扣除积分", "不允许上传附件");
                    ControlUtils.SelectListItemsIgnoreCase(this.rblUploadType, groupInfo.Additional.UploadType);
                    ETriStateUtils.AddListItems(this.rblDownloadType, "允许下载附件，不奖励或扣除积分", "允许下载附件，按照版块设置奖励或扣除积分", "不允许下载附件");
                    ControlUtils.SelectListItemsIgnoreCase(this.rblDownloadType, groupInfo.Additional.DownloadType);
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowSetAttachmentPermissions, groupInfo.Additional.IsAllowSetAttachmentPermissions.ToString());
                    this.tbMaxSize.Text = groupInfo.Additional.MaxSize.ToString();
                    this.tbMaxSizePerDay.Text = groupInfo.Additional.MaxSizePerDay.ToString();
                    this.tbMaxNumPerDay.Text = groupInfo.Additional.MaxNumPerDay.ToString();
                    this.tbAttachmentExtensions.Text = groupInfo.Additional.AttachmentExtensions;
                }

                //ArrayList groupInfoArrayList = UserGroupManager.GetGroupInfoArrayList(base.PublishmentSystemInfo.GroupSN);
                ArrayList groupInfoArrayList = UserGroupManager.GetGroupInfoArrayList(string.Empty);
                foreach (UserGroupInfo gInfo in groupInfoArrayList)
                {
                    if (groupID != gInfo.GroupID)
                    {
                        this.lbHorizontalGroup.Items.Add(new ListItem(gInfo.GroupName, gInfo.GroupID.ToString()));
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                //UserGroupInfo groupInfo = UserGroupManager.GetGroupInfo(base.PublishmentSystemInfo.GroupSN, this.groupID);
                UserGroupInfo groupInfo = UserGroupManager.GetGroupInfo(string.Empty, this.groupID);
                try
                {
                    groupInfo.Additional.IsAllowVisit = TranslateUtils.ToBool(this.rblIsAllowVisit.SelectedValue);
                    groupInfo.Additional.IsAllowHide = TranslateUtils.ToBool(this.rblIsAllowHide.SelectedValue);
                    //BaiRongDataProvider.UserGroupDAO.UpdateWithoutCredits(base.PublishmentSystemInfo.GroupSN, groupInfo);
                    BaiRongDataProvider.UserGroupDAO.UpdateWithoutCredits(string.Empty, groupInfo);
                    groupInfo.Additional.IsAllowSignature = TranslateUtils.ToBool(this.rblIsAllowSignature.SelectedValue);
                    groupInfo.Additional.SearchType = this.rblSearchType.SelectedValue;
                    groupInfo.Additional.SearchInterval = TranslateUtils.ToInt(this.tbSearchInterval.Text, groupInfo.Additional.SearchInterval);

                    groupInfo.Additional.IsAllowRead = TranslateUtils.ToBool(this.rblIsAllowRead.SelectedValue);
                    groupInfo.Additional.IsAllowPost = TranslateUtils.ToBool(this.rblIsAllowPost.SelectedValue);
                    groupInfo.Additional.IsAllowReply = TranslateUtils.ToBool(this.rblIsAllowReply.SelectedValue);
                    groupInfo.Additional.IsAllowPoll = TranslateUtils.ToBool(this.rblIsAllowPoll.SelectedValue);
                    groupInfo.Additional.MaxPostPerDay = TranslateUtils.ToInt(this.tbMaxPostPerDay.Text, groupInfo.Additional.MaxPostPerDay);
                    groupInfo.Additional.PostInterval = TranslateUtils.ToInt(this.tbPostInterval.Text, groupInfo.Additional.PostInterval);

                    groupInfo.Additional.UploadType = this.rblUploadType.SelectedValue;
                    groupInfo.Additional.DownloadType = this.rblDownloadType.SelectedValue;
                    groupInfo.Additional.IsAllowSetAttachmentPermissions = TranslateUtils.ToBool(this.rblIsAllowSetAttachmentPermissions.SelectedValue);
                    groupInfo.Additional.MaxSize = TranslateUtils.ToInt(this.tbMaxSize.Text);
                    groupInfo.Additional.MaxSizePerDay = TranslateUtils.ToInt(this.tbMaxSizePerDay.Text);
                    groupInfo.Additional.MaxNumPerDay = TranslateUtils.ToInt(this.tbMaxNumPerDay.Text);
                    groupInfo.Additional.AttachmentExtensions = this.tbAttachmentExtensions.Text;
                    //BaiRongDataProvider.UserGroupDAO.UpdateWithoutCredits(base.PublishmentSystemInfo.GroupSN, groupInfo);
                    BaiRongDataProvider.UserGroupDAO.UpdateWithoutCredits(string.Empty, groupInfo);

                    ArrayList strGroupIDArrayList = ControlUtils.GetSelectedListControlValueArrayList(this.lbHorizontalGroup);
                    if (strGroupIDArrayList.Count > 0)
                    {
                        ArrayList values = TranslateUtils.StringCollectionToArrayList(base.Request["horizontal"]);
                        if (values.Count > 0)
                        {
                            NameValueCollection nameValueCollection = new NameValueCollection();
                            foreach (string value in values)
                            {
                                if (StringUtils.EqualsIgnoreCase(value, "IsAllowVisit"))
                                {
                                    nameValueCollection.Add("IsAllowVisit", this.rblIsAllowVisit.SelectedValue);
                                }
                                else if (StringUtils.EqualsIgnoreCase(value, "IsAllowHide"))
                                {
                                    nameValueCollection.Add("IsAllowHide", this.rblIsAllowHide.SelectedValue);
                                }
                                else if (StringUtils.EqualsIgnoreCase(value, "IsAllowHide"))
                                {
                                    nameValueCollection.Add("IsAllowSignature", this.rblIsAllowSignature.SelectedValue);
                                }
                                else if (StringUtils.EqualsIgnoreCase(value, "SearchType"))
                                {
                                    nameValueCollection.Add("SearchType", this.rblSearchType.SelectedValue);
                                }
                                else if (StringUtils.EqualsIgnoreCase(value, "SearchInterval"))
                                {
                                    nameValueCollection.Add("SearchInterval", this.tbSearchInterval.Text);
                                }
                                //帖子权限
                                else if (StringUtils.EqualsIgnoreCase(value, "IsAllowRead"))
                                {
                                    nameValueCollection.Add("IsAllowRead", this.rblIsAllowRead.SelectedValue);
                                }
                                else if (StringUtils.EqualsIgnoreCase(value, "IsAllowPost"))
                                {
                                    nameValueCollection.Add("IsAllowPost", this.rblIsAllowPost.SelectedValue);
                                }
                                else if (StringUtils.EqualsIgnoreCase(value, "IsAllowReply"))
                                {
                                    nameValueCollection.Add("IsAllowReply", this.rblIsAllowReply.SelectedValue);
                                }
                                else if (StringUtils.EqualsIgnoreCase(value, "IsAllowPoll"))
                                {
                                    nameValueCollection.Add("IsAllowPoll", this.rblIsAllowPoll.SelectedValue);
                                }
                                else if (StringUtils.EqualsIgnoreCase(value, "MaxPostPerDay"))
                                {
                                    nameValueCollection.Add("MaxPostPerDay", this.tbMaxPostPerDay.Text);
                                }
                                else if (StringUtils.EqualsIgnoreCase(value, "PostInterval"))
                                {
                                    nameValueCollection.Add("PostInterval", this.tbPostInterval.Text);
                                }
                                //附件权限
                                else if (StringUtils.EqualsIgnoreCase(value, "UploadType"))
                                {
                                    nameValueCollection.Add("UploadType", this.rblUploadType.SelectedValue);
                                }
                                else if (StringUtils.EqualsIgnoreCase(value, "DownloadType"))
                                {
                                    nameValueCollection.Add("DownloadType", this.rblDownloadType.SelectedValue);
                                }
                                else if (StringUtils.EqualsIgnoreCase(value, "IsAllowSetAttachmentPermissions"))
                                {
                                    nameValueCollection.Add("IsAllowSetAttachmentPermissions", this.rblIsAllowSetAttachmentPermissions.SelectedValue);
                                }
                                else if (StringUtils.EqualsIgnoreCase(value, "MaxSize"))
                                {
                                    nameValueCollection.Add("MaxSize", this.tbMaxSize.Text);
                                }
                                else if (StringUtils.EqualsIgnoreCase(value, "MaxSizePerDay"))
                                {
                                    nameValueCollection.Add("MaxSizePerDay", this.tbMaxSizePerDay.Text);
                                }
                                else if (StringUtils.EqualsIgnoreCase(value, "MaxNumPerDay"))
                                {
                                    nameValueCollection.Add("MaxNumPerDay", this.tbMaxNumPerDay.Text);
                                }
                                else if (StringUtils.EqualsIgnoreCase(value, "AttachmentExtensions"))
                                {
                                    nameValueCollection.Add("AttachmentExtensions", this.tbAttachmentExtensions.Text);
                                }
                            }
                            foreach (string strGroupID in strGroupIDArrayList)
                            {
                                //UserGroupInfo gInfo = UserGroupManager.GetGroupInfo(base.PublishmentSystemInfo.GroupSN, TranslateUtils.ToInt(strGroupID));
                                UserGroupInfo gInfo = UserGroupManager.GetGroupInfo(string.Empty, TranslateUtils.ToInt(strGroupID));
                                if (gInfo != null)
                                {
                                    gInfo.Additional.SetExtendedAttribute(nameValueCollection);
                                    //BaiRongDataProvider.UserGroupDAO.UpdateWithoutCredits(base.PublishmentSystemInfo.GroupSN, gInfo);
                                    BaiRongDataProvider.UserGroupDAO.UpdateWithoutCredits(string.Empty, gInfo);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("用户组权限修改失败：{0}", ex.Message));
                    return;
                }
                base.SuccessMessage("用户组权限修改成功！");
                base.AddWaitAndRedirectScript(BackgroundUserGroup.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
    }
}