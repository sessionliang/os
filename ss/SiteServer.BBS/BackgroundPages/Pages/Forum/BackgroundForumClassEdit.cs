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

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundForumClassEdit : BackgroundBasePage
    {

        public TextBox txtForumName;
        public TextBox txtIndexName;
        public TextBox txtColor;
        public DropDownList ddlColumns;
        public RadioButtonList rblThreadState;

        private int forumID;

        public static string GetRedirectUrl(int publishmentSystemID, int forumID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_forumClassEdit.aspx?publishmentSystemID={0}&forumID={1}", publishmentSystemID, forumID));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            this.forumID = base.GetIntQueryString("ForumID");
            ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, this.forumID);
            if (forumInfo != null)
            {
                if (!base.IsPostBack)
                {
                    base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Forum, "编辑分区", AppManager.BBS.Permission.BBS_Forum);

                    rblThreadState.DataSource = EnumHelper.EnumListToTable(typeof(EThreadStateType));
                    rblThreadState.DataBind();
                    txtForumName.Text = forumInfo.ForumName;
                    txtIndexName.Text = forumInfo.IndexName;
                    txtColor.Text = forumInfo.Color;
                    ControlUtils.SelectListItemsIgnoreCase(ddlColumns, forumInfo.Columns.ToString());
                    ControlUtils.SelectListItemsIgnoreCase(rblThreadState, forumInfo.Additional.ThreadState);
                }
            }
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
                            base.FailMessage("分区属性修改失败，分区索引已存在！");
                            return;
                        }
                    }
                    forumInfo.ForumName = txtForumName.Text;
                    forumInfo.IndexName = txtIndexName.Text;
                    forumInfo.Color = txtColor.Text;
                    forumInfo.Columns = TranslateUtils.ToInt(ddlColumns.SelectedValue);
                    forumInfo.Additional.ThreadState = ControlUtils.GetSelectedListControlValueCollection(rblThreadState);
                    forumInfo.ExtendValues = forumInfo.Additional.ToString();

                    DataProvider.ForumDAO.UpdateForumInfo(base.PublishmentSystemID, forumInfo);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("分区修改失败：{0}", ex.Message));
                    LogUtils.AddErrorLog(ex);
                    return;
                }
                StringUtilityBBS.AddLog(base.PublishmentSystemID, this.forumID, 0, 0, "修改分区", string.Format("分区:{0}", txtForumName.Text));
                base.SuccessMessage("分区修改成功！");
                base.AddWaitAndRedirectScript(BackgroundForum.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
    }
}