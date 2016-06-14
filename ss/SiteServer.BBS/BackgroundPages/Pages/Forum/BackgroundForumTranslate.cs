using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Text;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;

using SiteServer.BBS.Model;
using SiteServer.BBS.Core;
using System.Collections.Generic;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundForumTranslate : BackgroundBasePage
    {

        public ListBox ForumIDFrom;
        public DropDownList ForumIDTo;

        private string returnUrl;
        private bool[] isLastNodeArray;
        private StringCollection ForumIDCollection;

        public static string GetRedirectUrl(int publishmentSystemID, string returnUrl)
        {
            return PageUtils.GetBBSUrl(string.Format("background_forumTranslate.aspx?publishmentSystemID={0}&returnUrl={1}", publishmentSystemID, StringUtils.ValueToUrl(returnUrl)));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Forum, "版块合并", AppManager.BBS.Permission.BBS_Forum);

                if (!string.IsNullOrEmpty(base.GetQueryString("ForumIDCollection")))
                    ForumIDCollection = TranslateUtils.StringCollectionToStringCollection(base.GetQueryString("ForumIDCollection"), ',');
                ForumIDFromDataBind();
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            int targetForumID = TranslateUtils.ToInt(ForumIDTo.SelectedValue);
            if (targetForumID <= 0)
            {
                base.FailMessage("请选择要合并到的目标版块！");
                return;
            }
            List<int> sourceForumIDList = ControlUtils.GetSelectedListControlValueIntList(this.ForumIDFrom);
            if (sourceForumIDList.Count == 0)
            {
                base.FailMessage("请选择要合并的版块！");
                return;
            }

            try
            {
                foreach (int sourceForumID in sourceForumIDList)
                {
                    DataProvider.ThreadDAO.TranslateThreadByForumID(base.PublishmentSystemID, sourceForumID, targetForumID);
                }

                base.SuccessMessage("版块合并成功！");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "版块合并失败！");
                LogUtils.AddErrorLog(ex);
            }
            if (!string.IsNullOrEmpty(this.returnUrl))
                base.AddWaitAndRedirectScript(this.returnUrl);
            else
                base.AddWaitAndRedirectScript(BackgroundForumTranslate.GetRedirectUrl(base.PublishmentSystemID, this.returnUrl));
        }

        public string ReturnUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.returnUrl))
                {
                    return BackgroundForumTranslate.GetRedirectUrl(base.PublishmentSystemID, string.Empty);
                }
                return this.returnUrl;
            }
        }

        private void ForumIDFromDataBind()
        {
            ArrayList list = DataProvider.ForumDAO.GetForumIDArrayList(base.PublishmentSystemID);
            if (list != null)
            {
                this.isLastNodeArray = new bool[list.Count + 1];
                ForumInfo forumInfo = null;
                ListItem listItem = null;
                string text = "", values = "";
                for (int i = 0; i < list.Count; i++)
                {
                    forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, TranslateUtils.ToInt(list[i].ToString()));
                    if (forumInfo != null)
                    {
                        text = ForumManager.GetSelectText(forumInfo, this.isLastNodeArray, true);
                        if (forumInfo.ParentID > 0)
                            values = forumInfo.ForumID.ToString();
                        else
                            values = "";
                        listItem = new ListItem(text, values);
                        if (ForumIDCollection != null && ForumIDCollection.Contains(forumInfo.ForumID.ToString()))
                            listItem.Selected = true;
                        ForumIDFrom.Items.Add(listItem);
                        listItem = new ListItem(text, values);
                        ForumIDTo.Items.Add(listItem);
                    }
                }
            }
        }

        /// <summary>
        /// 暂时不用了
        /// </summary>
        /// <param name="forumInfo"></param>
        /// <returns></returns>
        private string GetTitle(ForumInfo forumInfo)
        {
            string str = "";
            if (forumInfo == null)
                return str;
            if (!forumInfo.IsLastNode)
                this.isLastNodeArray[forumInfo.ParentsCount] = false;
            else
                this.isLastNodeArray[forumInfo.ParentsCount] = true;
            for (int i = 0; i < forumInfo.ParentsCount; i++)
            {
                if (this.isLastNodeArray[i])
                    str = String.Concat(str, "　");
                else
                    str = String.Concat(str, "│");
            }
            if (forumInfo.IsLastNode)
                str = String.Concat(str, "└");
            else
                str = String.Concat(str, "├");
            str = String.Concat(str, forumInfo.ForumName);
            //if(forumInfo.top != 0) {
            //    str = string.Format("{0} ({1})", str, nodeInfo.ContentNum);
            //}
            return str;
        }
    }
}