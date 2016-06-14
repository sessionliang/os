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
using BaiRong.Model;



namespace BaiRong.BackgroundPages
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

        public ListBox lbHorizontalLevel;

        private int levelID;

        public static string GetRedirectUrl(int levelID)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_permission.aspx?LevelID={0}", levelID));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            this.levelID = base.GetIntQueryString("LevelID");

            if (!base.IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserLevelCredit, "用户等级权限设置", AppManager.User.Permission.Usercenter_Setting);

                UserLevelInfo userLevelInfo = UserLevelManager.GetLevelInfo("", this.levelID);
                if (userLevelInfo != null)
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowHide, userLevelInfo.Additional.IsAllowHide.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowVisit, userLevelInfo.Additional.IsAllowVisit.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowSignature, userLevelInfo.Additional.IsAllowSignature.ToString());
                    ETriStateUtils.AddListItems(this.rblSearchType, "允许搜索主题标题、内容", "允许搜索主题标题", "不允许");
                    ControlUtils.SelectListItemsIgnoreCase(this.rblSearchType, userLevelInfo.Additional.SearchType);
                    this.tbSearchInterval.Text = userLevelInfo.Additional.SearchInterval.ToString();

                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowRead, userLevelInfo.Additional.IsAllowRead.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowPost, userLevelInfo.Additional.IsAllowPost.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowReply, userLevelInfo.Additional.IsAllowReply.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowPoll, userLevelInfo.Additional.IsAllowPoll.ToString());
                    this.tbMaxPostPerDay.Text = userLevelInfo.Additional.MaxPostPerDay.ToString();
                    this.tbPostInterval.Text = userLevelInfo.Additional.PostInterval.ToString();

                    ETriStateUtils.AddListItems(this.rblUploadType, "允许上传附件，不奖励或扣除积分", "允许上传附件，按照版块设置奖励或扣除积分", "不允许上传附件");
                    ControlUtils.SelectListItemsIgnoreCase(this.rblUploadType, userLevelInfo.Additional.UploadType);
                    ETriStateUtils.AddListItems(this.rblDownloadType, "允许下载附件，不奖励或扣除积分", "允许下载附件，按照版块设置奖励或扣除积分", "不允许下载附件");
                    ControlUtils.SelectListItemsIgnoreCase(this.rblDownloadType, userLevelInfo.Additional.DownloadType);
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsAllowSetAttachmentPermissions, userLevelInfo.Additional.IsAllowSetAttachmentPermissions.ToString());
                    this.tbMaxSize.Text = userLevelInfo.Additional.MaxSize.ToString();
                    this.tbMaxSizePerDay.Text = userLevelInfo.Additional.MaxSizePerDay.ToString();
                    this.tbMaxNumPerDay.Text = userLevelInfo.Additional.MaxNumPerDay.ToString();
                    this.tbAttachmentExtensions.Text = userLevelInfo.Additional.AttachmentExtensions;
                }

                ArrayList levelInfoArrayList = UserLevelManager.GetLevelInfoArrayList("");
                foreach (UserLevelInfo levelInfo in levelInfoArrayList)
                {
                    if (levelID != levelInfo.ID)
                    {
                        this.lbHorizontalLevel.Items.Add(new ListItem(levelInfo.LevelName, levelInfo.ID.ToString()));
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                UserLevelInfo levelInfo = UserLevelManager.GetLevelInfo("", this.levelID);
                try
                {
                    levelInfo.Additional.IsAllowVisit = TranslateUtils.ToBool(this.rblIsAllowVisit.SelectedValue);
                    levelInfo.Additional.IsAllowHide = TranslateUtils.ToBool(this.rblIsAllowHide.SelectedValue);
                    BaiRongDataProvider.UserLevelDAO.UpdateWithoutCredits("", levelInfo);
                    levelInfo.Additional.IsAllowSignature = TranslateUtils.ToBool(this.rblIsAllowSignature.SelectedValue);
                    levelInfo.Additional.SearchType = this.rblSearchType.SelectedValue;
                    levelInfo.Additional.SearchInterval = TranslateUtils.ToInt(this.tbSearchInterval.Text, levelInfo.Additional.SearchInterval);

                    levelInfo.Additional.IsAllowRead = TranslateUtils.ToBool(this.rblIsAllowRead.SelectedValue);
                    levelInfo.Additional.IsAllowPost = TranslateUtils.ToBool(this.rblIsAllowPost.SelectedValue);
                    levelInfo.Additional.IsAllowReply = TranslateUtils.ToBool(this.rblIsAllowReply.SelectedValue);
                    levelInfo.Additional.IsAllowPoll = TranslateUtils.ToBool(this.rblIsAllowPoll.SelectedValue);
                    levelInfo.Additional.MaxPostPerDay = TranslateUtils.ToInt(this.tbMaxPostPerDay.Text, levelInfo.Additional.MaxPostPerDay);
                    levelInfo.Additional.PostInterval = TranslateUtils.ToInt(this.tbPostInterval.Text, levelInfo.Additional.PostInterval);

                    levelInfo.Additional.UploadType = this.rblUploadType.SelectedValue;
                    levelInfo.Additional.DownloadType = this.rblDownloadType.SelectedValue;
                    levelInfo.Additional.IsAllowSetAttachmentPermissions = TranslateUtils.ToBool(this.rblIsAllowSetAttachmentPermissions.SelectedValue);
                    levelInfo.Additional.MaxSize = TranslateUtils.ToInt(this.tbMaxSize.Text);
                    levelInfo.Additional.MaxSizePerDay = TranslateUtils.ToInt(this.tbMaxSizePerDay.Text);
                    levelInfo.Additional.MaxNumPerDay = TranslateUtils.ToInt(this.tbMaxNumPerDay.Text);
                    levelInfo.Additional.AttachmentExtensions = this.tbAttachmentExtensions.Text;
                    BaiRongDataProvider.UserLevelDAO.UpdateWithoutCredits("", levelInfo);

                    ArrayList strLevelIDArrayList = ControlUtils.GetSelectedListControlValueArrayList(this.lbHorizontalLevel);
                    if (strLevelIDArrayList.Count > 0)
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
                            foreach (string strLevelID in strLevelIDArrayList)
                            {
                                UserLevelInfo gInfo = UserLevelManager.GetLevelInfo("", TranslateUtils.ToInt(strLevelID));
                                if (gInfo != null)
                                {
                                    gInfo.Additional.SetExtendedAttribute(nameValueCollection);
                                    BaiRongDataProvider.UserLevelDAO.UpdateWithoutCredits("", gInfo);
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
                base.AddWaitAndRedirectScript(BackgroundUserLevel.GetRedirectUrl());
            }
        }
    }
}