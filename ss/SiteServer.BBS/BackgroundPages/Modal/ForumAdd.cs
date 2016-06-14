using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Collections;
using SiteServer.BBS.Core;
using BaiRong.Core;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class ForumAdd : BackgroundBasePage
    {
        protected DropDownList ParentForumID;
        protected TextBox ForumNames;

        private bool[] isLastNodeArray;

        public static string GetOpenWindowString(int publishmentSystemID, int forumID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ForumID", forumID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("快速添加板块", PageUtils.GetBBSUrl("modal_forumAdd.aspx"), arguments, 500, 550);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            int forumID = base.GetIntQueryString("ForumID");

            if (!IsPostBack)
            {
                ListItem listitem = new ListItem("无父板块", string.Empty);
                ParentForumID.Items.Add(listitem);

                ArrayList forumIDArrayList = DataProvider.ForumDAO.GetForumIDArrayList(base.PublishmentSystemID);
                int forumCount = forumIDArrayList.Count;
                this.isLastNodeArray = new bool[forumCount + 1];
                foreach (int theForumID in forumIDArrayList)
                {
                    ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, theForumID);
                    int itemForumID = forumInfo.ForumID;
                    string forumName = forumInfo.ForumName;
                    int parentsCount = forumInfo.ParentsCount;
                    bool isLastNode = forumInfo.IsLastNode;
                    string value = itemForumID.ToString();

                    //listitem = new ListItem(this.GetTitle(itemForumID, forumName, parentsCount, isLastNode), value);
                    listitem = new ListItem(ForumManager.GetSelectText(forumInfo, isLastNodeArray, false), value);
                    if (itemForumID == forumID)
                    {
                        listitem.Selected = true;
                    }
                    ParentForumID.Items.Add(listitem);
                }
            }
        }

        /// <summary>
        /// 暂时不用了
        /// </summary>
        /// <param name="forumID"></param>
        /// <param name="forumName"></param>
        /// <param name="parentsCount"></param>
        /// <param name="isLastNode"></param>
        /// <returns></returns>
        public string GetTitle(int forumID, string forumName, int parentsCount, bool isLastNode)
        {
            string str = "";
            if (forumID == 0)
            {
                isLastNode = true;
            }
            if (isLastNode == false)
            {
                this.isLastNodeArray[parentsCount] = false;
            }
            else
            {
                this.isLastNodeArray[parentsCount] = true;
            }
            for (int i = 0; i < parentsCount; i++)
            {
                if (this.isLastNodeArray[i])
                {
                    str = String.Concat(str, "　");
                }
                else
                {
                    str = String.Concat(str, "│");
                }
            }
            if (isLastNode == true)
            {
                str = String.Concat(str, "└");
            }
            else
            {
                str = String.Concat(str, "├");
            }
            str = String.Concat(str, forumName);
            return str;
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            int parentForumID = TranslateUtils.ToInt(this.ParentForumID.SelectedValue);

            try
            {
                Hashtable insertedForumIDHashtable = new Hashtable();//key为栏目的级别，1为第一级栏目
                insertedForumIDHashtable[1] = parentForumID;

                string[] forumNameArray = this.ForumNames.Text.Split('\n');
                ArrayList indexNameList = null;
                foreach (string item in forumNameArray)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        //count为栏目的级别
                        int count = (StringUtils.GetStartCount('－', item) == 0) ? StringUtils.GetStartCount('-', item) : StringUtils.GetStartCount('－', item);
                        string forumName = item.Substring(count, item.Length - count);
                        string indexName = string.Empty;
                        count++;

                        if (!string.IsNullOrEmpty(forumName) && insertedForumIDHashtable.Contains(count))
                        {
                            if (StringUtils.Contains(forumName, "(") && StringUtils.Contains(forumName, ")"))
                            {
                                int length = forumName.IndexOf(')') - forumName.IndexOf('(');
                                if (length > 0)
                                {
                                    indexName = forumName.Substring(forumName.IndexOf('(') + 1, length);
                                    forumName = forumName.Substring(0, forumName.IndexOf('('));
                                }
                            }
                            forumName = forumName.Trim();
                            indexName = indexName.Trim(' ', '(', ')');
                            if (!string.IsNullOrEmpty(indexName))
                            {
                                if (indexNameList == null)
                                {
                                    indexNameList = DataProvider.ForumDAO.GetIndexNameArrayList(base.PublishmentSystemID);
                                }
                                if (indexNameList.IndexOf(indexName) != -1)
                                {
                                    indexName = string.Empty;
                                }
                                else
                                {
                                    indexNameList.Add(indexName);
                                }
                            }

                            int parentID = (int)insertedForumIDHashtable[count];
                            int insertedForumID = DataProvider.ForumDAO.InsertForumInfo(base.PublishmentSystemID, parentID, forumName, indexName);
                            insertedForumIDHashtable[count + 1] = insertedForumID;
                        }
                    }
                }

                StringUtilityBBS.AddLog(base.PublishmentSystemID, "快速添加板块", string.Format("父板块:{0},板块:{1}", ForumManager.GetForumName(base.PublishmentSystemID, parentForumID), this.ForumNames.Text.Replace('\n', ',')));

                isChanged = true;
            }
            catch(Exception ex)
            {
                isChanged = false;
                base.FailMessage(ex, ex.Message);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundForum.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
    }
}
