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


namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundForumEdit : BackgroundBasePage
    {
        public TextBox txtForumName;
        public TextBox txtIndexName;
        public TextBox txtColor;
        public TextBox txtIconUrl;
        public Button btnSelectImage;
        public Button btnUploadImage;
        public TextBox txtSummary;
        public TextBox txtMetaKeywords;
        public TextBox txtMetaDescription;
        public TextBox txtLinkUrl;
        public DropDownList ddlColumns;
        public RadioButtonList rblIsOnlyDisplaySubForums;
        public RadioButtonList rblIsDisplayForumInfo;
        public RadioButtonList rblForumSummaryType;
        public RadioButtonList rblIsDisplayTopThread;
        public RadioButtonList rblThreadOrderField;
        public RadioButtonList rblThreadOrderType;
        public RadioButtonList rblThreadCheckType;
        public RadioButtonList rblIsEditThread;
        public RadioButtonList rblIsOpenRecycle;
        public RadioButtonList rblIsAllowHtml;
        public RadioButtonList rblIsAllowImg;
        public RadioButtonList rblIsAllowMultimedia;
        public RadioButtonList rblIsAllowEmotionSymbol;
        public RadioButtonList rblIsOpenDisturbCode;
        public RadioButtonList rblIsAllowAnonymousThread;
        public RadioButtonList rblIsOpenWatermark;
        public TextBox txtAllowAccessoryType;
        public RadioButtonList rblThreadAutoCloseType;
        public PlaceHolder phThreadAutoCloseType;
        public TextBox txtThreadAutoCloseWithDateNum;
        public RadioButtonList rblThreadState;
        public RadioButtonList rblThreadCategoryType;
        public RadioButtonList rblIsReadByCategory;
        public RadioButtonList rblThreadCategoryDisplayType;

        //权限相关
        public TextBox txtAccessPassword;
        public TextBox txtAccessUserNames;
        public Repeater PermissionRepeater;
        public Repeater UserGroupCreditsRepeater;
        public PlaceHolder phUserGroupSpecials;
        public Repeater UserGroupSpecialsRepeater;
        public Repeater UserGroupSystemsRepeater;

        //NodeProperty-----------no user---
        public DropDownList TemplateID;
        public TextBox Content;
        public HyperLink CreateChannelRule;
        public HyperLink CreateContentRule;

        private int forumID;
        private ArrayList permissionArrayList;
        private SortedList userGroupIDWithForbidden;

        public static string GetRedirectUrl(int publishmentSystemID, int forumID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_forumEdit.aspx?publishmentSystemID={0}&forumID={1}", publishmentSystemID, forumID));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            this.forumID = base.GetIntQueryString("forumID");
            ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, this.forumID);
            if (forumInfo != null)
            {
                if (!base.IsPostBack)
                {
                    base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Forum, "编辑板块", AppManager.BBS.Permission.BBS_Forum);

                    rblForumSummaryType.DataSource = EnumHelper.EnumListToTable(typeof(EForumSummaryType));
                    rblForumSummaryType.DataBind();
                    rblThreadOrderField.DataSource = EnumHelper.EnumListToTable(typeof(EThreadOrderField));
                    rblThreadOrderField.DataBind();
                    rblThreadOrderType.DataSource = EnumHelper.EnumListToTable(typeof(EThreadOrderType));
                    rblThreadOrderType.DataBind();
                    rblThreadCheckType.DataSource = EnumHelper.EnumListToTable(typeof(EThreadCheckType));
                    rblThreadCheckType.DataBind();
                    rblThreadAutoCloseType.DataSource = EnumHelper.EnumListToTable(typeof(EThreadAutoCloseType));
                    rblThreadAutoCloseType.DataBind();
                    rblThreadState.DataSource = EnumHelper.EnumListToTable(typeof(EThreadStateType));
                    rblThreadState.DataBind();
                    rblThreadCategoryType.DataSource = EnumHelper.EnumListToTable(typeof(EThreadCategoryType));
                    rblThreadCategoryType.DataBind();
                    rblThreadCategoryDisplayType.DataSource = EnumHelper.EnumListToTable(typeof(EThreadCategoryDisplayType));
                    rblThreadCategoryDisplayType.DataBind();
                    this.txtIconUrl.Attributes.Add("onchange", string.Format("ShowImg({0}, '{1}','{2}','{3}')", "this", "preview_txtIconUrl", ConfigUtils.Instance.ApplicationPath, "/bbs"));
                    string showPopWinString = string.Empty;


                    //string showPopWinString = Modal.FilePathRule.GetOpenWindowString(base.PublishmentSystemID, true, this.ChannelFilePathRule.ClientID);
                    //this.CreateChannelRule.Attributes.Add("onclick", showPopWinString);

                    //showPopWinString = Modal.FilePathRule.GetOpenWindowString(base.PublishmentSystemID, false, this.ContentFilePathRule.ClientID);
                    //this.CreateContentRule.Attributes.Add("onclick", showPopWinString);

                    showPopWinString = Modal.SelectImage.GetOpenWindowString(base.PublishmentSystemID, forumInfo, this.txtIconUrl.ClientID);
                    this.btnSelectImage.Attributes.Add("onclick", showPopWinString);

                    showPopWinString = Modal.UploadImageSingle.GetOpenWindowString(base.PublishmentSystemID, forumInfo.ForumID, string.Empty, this.txtIconUrl.ClientID);
                    this.btnUploadImage.Attributes.Add("onclick", showPopWinString);

                    //TemplateID.Items.Insert(0, new ListItem("<未设置>", "0"));
                    //ControlUtils.SelectListItems(TemplateID, forumInfo.TemplateID.ToString());

                    txtForumName.Text = forumInfo.ForumName;
                    txtIndexName.Text = forumInfo.IndexName;
                    txtColor.Text = forumInfo.Color;
                    txtIconUrl.Text = forumInfo.IconUrl;
                    txtSummary.Text = forumInfo.Summary;
                    txtMetaKeywords.Text = forumInfo.MetaKeywords;
                    txtMetaDescription.Text = forumInfo.MetaDescription;
                    txtLinkUrl.Text = forumInfo.LinkUrl;
                    txtThreadAutoCloseWithDateNum.Text = forumInfo.Additional.ThreadAutoCloseWithDateNum.ToString();
                    txtAllowAccessoryType.Text = forumInfo.Additional.AllowAccessoryType;
                    ControlUtils.SelectListItemsIgnoreCase(ddlColumns, forumInfo.Columns.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblIsOnlyDisplaySubForums, forumInfo.Additional.IsOnlyDisplaySubForums.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblIsDisplayForumInfo, forumInfo.Additional.IsDisplayForumInfo.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblForumSummaryType, forumInfo.Additional.ForumSummaryType);
                    ControlUtils.SelectListItemsIgnoreCase(rblIsDisplayTopThread, forumInfo.Additional.IsDisplayTopThread.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblForumSummaryType, forumInfo.Additional.ForumSummaryType);
                    ControlUtils.SelectListItemsIgnoreCase(rblThreadOrderField, forumInfo.Additional.ThreadOrderField);
                    ControlUtils.SelectListItemsIgnoreCase(rblThreadOrderType, forumInfo.Additional.ThreadOrderType);
                    ControlUtils.SelectListItemsIgnoreCase(rblThreadCheckType, forumInfo.Additional.ThreadCheckType);
                    ControlUtils.SelectListItemsIgnoreCase(rblIsEditThread, forumInfo.Additional.IsEditThread.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblIsOpenRecycle, forumInfo.Additional.IsOpenRecycle.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblThreadAutoCloseType, forumInfo.Additional.ThreadAutoCloseType);
                    ControlUtils.SelectListItemsIgnoreCase(rblIsOpenRecycle, forumInfo.Additional.IsOpenRecycle.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblIsAllowHtml, forumInfo.Additional.IsAllowHtml.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblIsAllowImg, forumInfo.Additional.IsAllowImg.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblIsAllowMultimedia, forumInfo.Additional.IsAllowMultimedia.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblIsAllowEmotionSymbol, forumInfo.Additional.IsAllowEmotionSymbol.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblIsOpenDisturbCode, forumInfo.Additional.IsOpenDisturbCode.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblIsAllowAnonymousThread, forumInfo.Additional.IsAllowAnonymousThread.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblIsOpenWatermark, forumInfo.Additional.IsOpenWatermark.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblThreadState, forumInfo.Additional.ThreadState);
                    ControlUtils.SelectListItemsIgnoreCase(rblThreadCategoryType, forumInfo.Additional.ThreadCategoryType);
                    ControlUtils.SelectListItemsIgnoreCase(rblIsReadByCategory, forumInfo.Additional.IsReadByCategory.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblThreadCategoryDisplayType, forumInfo.Additional.ThreadCategoryDisplayType);

                    if (forumInfo.Additional.ThreadAutoCloseType.ToLower() != EThreadAutoCloseType.NoAutoClose.ToString().ToLower())
                    {
                        phThreadAutoCloseType.Visible = true;
                    }

                    this.txtAccessPassword.Text = forumInfo.Additional.AccessPassword;
                    this.txtAccessUserNames.Text = forumInfo.Additional.AccessUserNames;
                    //this.Content.PublishmentSystemID = base.PublishmentSystemID;
                    //this.Content.Text = StringUtilityBBS.TextEditorContentDecode(forumInfo.Content, ConfigUtils.Instance.ApplicationPath, base.PublishmentSystemInfo.PublishmentSystemUrl);

                    //this.ChannelPermissionRepeater.DataSource = this.pagePermissionArrayList;
                    //this.ChannelPermissionRepeater.ItemDataBound += new RepeaterItemEventHandler(ChannelPermissionRepeater_ItemDataBound);
                    //this.ChannelPermissionRepeater.DataBind();

                    //this.ChannelUserGroupRepeater.DataSource = UserGroupManager.GetGroupInfoArrayList();
                    //this.ChannelUserGroupRepeater.ItemDataBound += new RepeaterItemEventHandler(ChannelUserGroupRepeater_ItemDataBound);
                    //this.ChannelUserGroupRepeater.DataBind();

                    this.permissionArrayList = EPermissionUtils.GetArrayList();
                    this.userGroupIDWithForbidden = DataProvider.PermissionsDAO.GetUserGroupIDWithForbiddenSortedList(base.PublishmentSystemInfo.GroupSN, this.forumID);

                    this.PermissionRepeater.DataSource = this.permissionArrayList;
                    this.PermissionRepeater.ItemDataBound += new RepeaterItemEventHandler(PermissionRepeater_ItemDataBound);
                    this.PermissionRepeater.DataBind();

                    ArrayList groupInfoArrayList = UserGroupManager.GetGroupInfoArrayList(base.PublishmentSystemInfo.GroupSN);
                    ArrayList credits = new ArrayList();
                    ArrayList specials = new ArrayList();
                    ArrayList systems = new ArrayList();
                    foreach (UserGroupInfo groupInfo in groupInfoArrayList)
                    {
                        if (groupInfo.GroupType == EUserGroupType.Credits)
                        {
                            credits.Add(groupInfo);
                        }
                        else if (groupInfo.GroupType == EUserGroupType.Specials)
                        {
                            specials.Add(groupInfo);
                        }
                        else
                        {
                            systems.Add(groupInfo);
                        }
                    }

                    this.UserGroupCreditsRepeater.DataSource = credits;
                    this.UserGroupCreditsRepeater.ItemDataBound += new RepeaterItemEventHandler(UserGroupRepeater_ItemDataBound);
                    this.UserGroupCreditsRepeater.DataBind();

                    if (specials.Count > 0)
                    {
                        this.phUserGroupSpecials.Visible = true;
                        this.UserGroupSpecialsRepeater.DataSource = specials;
                        this.UserGroupSpecialsRepeater.ItemDataBound += new RepeaterItemEventHandler(UserGroupRepeater_ItemDataBound);
                        this.UserGroupSpecialsRepeater.DataBind();
                    }

                    this.UserGroupSystemsRepeater.DataSource = systems;
                    this.UserGroupSystemsRepeater.ItemDataBound += new RepeaterItemEventHandler(UserGroupRepeater_ItemDataBound);
                    this.UserGroupSystemsRepeater.DataBind();
                }
            }
        }

        public static void PermissionRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PermissionItemDataBound(e);
            }
        }

        public void UserGroupRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UserGroupRepeaterItemDataBound(e);
            }
        }

        private static void PermissionItemDataBound(RepeaterItemEventArgs e)
        {
            EPermission permission = (EPermission)e.Item.DataItem;

            Literal ltlPermission = (Literal)e.Item.FindControl("ltlPermission");
            ltlPermission.Text = "禁止" + EPermissionUtils.GetText(permission);
        }

        private void UserGroupRepeaterItemDataBound(RepeaterItemEventArgs e)
        {
            string parmName = "userpermissions";

            CheckBox userGroupCheckBox = (CheckBox)e.Item.FindControl("UserGroup");
            Literal userPermissions = (Literal)e.Item.FindControl("UserPermissions");

            UserGroupInfo groupInfo = e.Item.DataItem as UserGroupInfo;

            userGroupCheckBox.Text = groupInfo.GroupName;
            userGroupCheckBox.Attributes.Add("onclick", "_checkAll(this.parentNode.parentNode, this.checked);");

            StringBuilder builder = new StringBuilder();
            string forbidden = this.userGroupIDWithForbidden[groupInfo.GroupID] as string;
            ArrayList forbiddenArrayList = TranslateUtils.StringCollectionToArrayList(forbidden);
            foreach (EPermission permission in this.permissionArrayList)
            {
                string permissionValue = EPermissionUtils.GetValue(permission);
                if (forbiddenArrayList.Contains(permissionValue))
                {
                    builder.AppendFormat("<td class='{0}'><input type='checkbox' checked name='{1}' id='{1}' value='{2}_{0}'></td>", permissionValue, parmName, groupInfo.GroupID);
                }
                else
                {
                    builder.AppendFormat("<td class='{0}'><input type='checkbox' name='{1}' id='{1}' value='{2}_{0}'></td>", permissionValue, parmName, groupInfo.GroupID);
                }
            }
            userPermissions.Text = builder.ToString();
        }

        public void ThreadAutoCloseType_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ControlUtils.GetSelectedListControlValueCollection(rblThreadAutoCloseType).ToLower() != EThreadAutoCloseType.NoAutoClose.ToString().ToLower())
                phThreadAutoCloseType.Visible = true;
            else
                phThreadAutoCloseType.Visible = false;
            ClientScript.RegisterStartupScript(typeof(Page), "srcipt_test", "<script>_toggleTab(3,4);</script>");
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                ForumInfo forumInfo = null;
                try
                {
                    forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, this.forumID);
                    if (!forumInfo.IndexName.Equals(txtIndexName.Text) && txtIndexName.Text.Length != 0)
                    {
                        ArrayList IndexNameList = DataProvider.ForumDAO.GetIndexNameArrayList(base.PublishmentSystemID);
                        if (IndexNameList.IndexOf(txtIndexName.Text) != -1)
                        {
                            base.FailMessage("版块属性修改失败，版块索引已存在！");
                            return;
                        }
                    }

                    forumInfo.ForumName = txtForumName.Text;
                    forumInfo.IndexName = txtIndexName.Text;
                    forumInfo.IconUrl = txtIconUrl.Text;
                    forumInfo.Summary = txtSummary.Text;
                    forumInfo.MetaDescription = txtMetaDescription.Text;
                    forumInfo.MetaKeywords = txtMetaKeywords.Text;
                    forumInfo.Color = txtColor.Text;
                    forumInfo.LinkUrl = txtLinkUrl.Text;
                    forumInfo.Additional.ThreadAutoCloseWithDateNum = TranslateUtils.ToInt(txtThreadAutoCloseWithDateNum.Text);
                    forumInfo.Additional.AllowAccessoryType = txtAllowAccessoryType.Text.Trim();
                    forumInfo.Columns = TranslateUtils.ToInt(ddlColumns.SelectedValue);
                    //forumInfo.TemplateID = (TemplateID.Items.Count > 0) ? int.Parse(TemplateID.SelectedValue) : 0;
                    forumInfo.Additional.IsOnlyDisplaySubForums = TranslateUtils.ToBool(rblIsOnlyDisplaySubForums.SelectedValue);
                    forumInfo.Additional.IsDisplayForumInfo = TranslateUtils.ToBool(rblIsDisplayForumInfo.SelectedValue);
                    forumInfo.Additional.ForumSummaryType = ControlUtils.GetSelectedListControlValueCollection(rblForumSummaryType);
                    forumInfo.Additional.IsDisplayTopThread = TranslateUtils.ToBool(rblIsDisplayTopThread.SelectedValue);
                    forumInfo.Additional.ThreadOrderField = ControlUtils.GetSelectedListControlValueCollection(rblThreadOrderField);
                    forumInfo.Additional.ThreadOrderType = ControlUtils.GetSelectedListControlValueCollection(rblThreadOrderType);
                    forumInfo.Additional.ThreadCheckType = ControlUtils.GetSelectedListControlValueCollection(rblThreadCheckType);
                    forumInfo.Additional.IsEditThread = TranslateUtils.ToBool(rblIsEditThread.SelectedValue);
                    forumInfo.Additional.IsOpenRecycle = TranslateUtils.ToBool(rblIsOpenRecycle.SelectedValue);
                    forumInfo.Additional.IsAllowHtml = TranslateUtils.ToBool(rblIsAllowHtml.SelectedValue);
                    forumInfo.Additional.IsAllowImg = TranslateUtils.ToBool(rblIsAllowImg.SelectedValue);
                    forumInfo.Additional.IsAllowMultimedia = TranslateUtils.ToBool(rblIsAllowMultimedia.SelectedValue);
                    forumInfo.Additional.IsAllowEmotionSymbol = TranslateUtils.ToBool(rblIsAllowEmotionSymbol.SelectedValue);
                    forumInfo.Additional.IsOpenDisturbCode = TranslateUtils.ToBool(rblIsOpenDisturbCode.SelectedValue);
                    forumInfo.Additional.IsAllowAnonymousThread = TranslateUtils.ToBool(rblIsAllowAnonymousThread.SelectedValue);
                    forumInfo.Additional.IsOpenWatermark = TranslateUtils.ToBool(rblIsOpenWatermark.SelectedValue);
                    forumInfo.Additional.ThreadAutoCloseType = ControlUtils.GetSelectedListControlValueCollection(rblThreadAutoCloseType);
                    forumInfo.Additional.ThreadState = ControlUtils.GetSelectedListControlValueCollection(rblThreadState);
                    forumInfo.Additional.ThreadCategoryType = ControlUtils.GetSelectedListControlValueCollection(rblThreadCategoryType);
                    forumInfo.Additional.IsReadByCategory = TranslateUtils.ToBool(rblIsReadByCategory.SelectedValue);
                    forumInfo.Additional.ThreadCategoryDisplayType = ControlUtils.GetSelectedListControlValueCollection(rblThreadCategoryDisplayType);

                    forumInfo.Additional.AccessPassword = this.txtAccessPassword.Text;
                    forumInfo.Additional.AccessUserNames = this.txtAccessUserNames.Text;

                    forumInfo.ExtendValues = forumInfo.Additional.ToString();

                    DataProvider.ForumDAO.UpdateForumInfo(base.PublishmentSystemID, forumInfo);

                    SortedList theUserGroupIDWithForbidden = this.GetUserGroupIDWithForbidden();
                    DataProvider.PermissionsDAO.Save(base.PublishmentSystemID, this.forumID, theUserGroupIDWithForbidden);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("版块修改失败：{0}", ex.Message));
                    LogUtils.AddErrorLog(ex);
                    return;
                }
                StringUtilityBBS.AddLog(base.PublishmentSystemID, this.forumID, 0, 0, "修改版块", string.Format("版块:{0}", txtForumName.Text));
                base.SuccessMessage("版块修改成功！");
                base.AddWaitAndRedirectScript(BackgroundForum.GetRedirectUrl(base.PublishmentSystemID));
            }
        }

        private SortedList GetUserGroupIDWithForbidden()
        {
            SortedList theUserGroupIDWithForbidden = new SortedList();
            if (!string.IsNullOrEmpty(base.Request.Form["userpermissions"]))
            {
                ArrayList valueArrayList = TranslateUtils.StringCollectionToArrayList(base.Request.Form["userpermissions"]);

                foreach (string value in valueArrayList)
                {
                    int userGroupID = TranslateUtils.ToInt(value.Split('_')[0]);
                    string forbidden = value.Split('_')[1];
                    string forbiddens = theUserGroupIDWithForbidden[userGroupID] as string;
                    if (string.IsNullOrEmpty(forbiddens))
                    {
                        forbiddens = forbidden;
                    }
                    else
                    {
                        forbiddens += "," + forbidden;
                    }
                    theUserGroupIDWithForbidden[userGroupID] = forbiddens;
                }
            }
            return theUserGroupIDWithForbidden;
        }

        public string PreviewImageSrc
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtIconUrl.Text))
                {
                    string extension = PathUtils.GetExtension(this.txtIconUrl.Text);
                    if (EFileSystemTypeUtils.IsImage(extension))
                     // return PageUtils.ParseNavigationUrl(this.txtIconUrl.Text);
                        return PageUtilityBBS.ParseNavigationUrl(this.txtIconUrl.Text);
                    else if (EFileSystemTypeUtils.IsFlash(extension))
                        return PageUtils.GetIconUrl("flash.jpg");
                    else if (EFileSystemTypeUtils.IsPlayer(extension))
                        return PageUtils.GetIconUrl("player.gif");
                }
                return PageUtils.GetIconUrl("empty.gif");
            }
        }
    }
}