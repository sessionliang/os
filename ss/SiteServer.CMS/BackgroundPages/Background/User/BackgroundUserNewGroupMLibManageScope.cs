using System;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundUserNewGroupMLibManageScope : BackgroundBasePage
    {
        public Literal NodeTree;
        public Literal llPublishmentSystem;

        private int itemID;

        private string GetNodeTreeHtml()
        {
            StringBuilder htmlBuilder = new StringBuilder();
            ArrayList mLibScopeInfoArrayList = base.Session[BackgroundUserNewGroupMLibSite.AllMLibPublishmentSystemArrayListKey] as ArrayList;
            if (mLibScopeInfoArrayList == null)
            {
                PageUtils.RedirectToErrorPage("超出时间范围，请重新进入！");
                return string.Empty;
            }
            ArrayList nodeIDArrayList = new ArrayList();
            foreach (MLibScopeInfo mLibScopeInfo in mLibScopeInfoArrayList)
            {
                if (mLibScopeInfo.PublishmentSystemID == base.PublishmentSystemID)
                {
                    nodeIDArrayList.Add(mLibScopeInfo.NodeID);
                }
            }

            #region 用户组设置的投稿范围
            ArrayList userMLibScopeInfoArrayList = base.Session[BackgroundUserNewGroupMLibSite.MLibScopeArrayListKey] as ArrayList;
            ArrayList userNodeIDArrayList = new ArrayList();
            foreach (MLibScopeInfo mLibScopeInfo in userMLibScopeInfoArrayList)
            {
                if (mLibScopeInfo.PublishmentSystemID == base.PublishmentSystemID)
                {
                    userNodeIDArrayList.Add(mLibScopeInfo.NodeID);
                }
            }
            #endregion

            string treeDirectoryUrl = PageUtils.GetIconUrl("tree");

            htmlBuilder.Append("<span id='ChannelSelectControl'>");
            ArrayList theNodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
            bool[] IsLastNodeArray = new bool[theNodeIDArrayList.Count];
            foreach (int theNodeID in theNodeIDArrayList)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, theNodeID);
                int NodeID = nodeInfo.NodeID;
                string NodeName = nodeInfo.NodeName;
                int ParentsCount = nodeInfo.ParentsCount;
                int ChildrenCount = nodeInfo.ChildrenCount;
                bool IsLastNode = nodeInfo.IsLastNode;
                int ContentNum = nodeInfo.ContentNum;
                htmlBuilder.Append(GetTitle(nodeInfo, treeDirectoryUrl, IsLastNodeArray, nodeIDArrayList, userNodeIDArrayList));
                htmlBuilder.Append("<br/>");
            }
            htmlBuilder.Append("</span>");
            return htmlBuilder.ToString();
        }

        private string GetTitle(NodeInfo nodeInfo, string treeDirectoryUrl, bool[] IsLastNodeArray, IList nodeIDArrayList, ArrayList isCheckedNodeID)
        {
            StringBuilder itemBuilder = new StringBuilder();
            if (nodeInfo.NodeID == base.PublishmentSystemID)
            {
                nodeInfo.IsLastNode = true;
            }
            if (nodeInfo.IsLastNode == false)
            {
                IsLastNodeArray[nodeInfo.ParentsCount] = false;
            }
            else
            {
                IsLastNodeArray[nodeInfo.ParentsCount] = true;
            }
            for (int i = 0; i < nodeInfo.ParentsCount; i++)
            {
                if (IsLastNodeArray[i])
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_empty.gif\"/>", treeDirectoryUrl);
                }
                else
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_line.gif\"/>", treeDirectoryUrl);
                }
            }
            if (nodeInfo.IsLastNode)
            {
                if (nodeInfo.ChildrenCount > 0)
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_plusbottom.gif\"/>", treeDirectoryUrl);
                }
                else
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_minusbottom.gif\"/>", treeDirectoryUrl);
                }
            }
            else
            {
                if (nodeInfo.ChildrenCount > 0)
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_plusmiddle.gif\"/>", treeDirectoryUrl);
                }
                else
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_minusmiddle.gif\"/>", treeDirectoryUrl);
                }
            }

            string check = "";
            if (nodeIDArrayList.Contains(nodeInfo.NodeID))
            {
                string ischeck = "";
                if (isCheckedNodeID.Contains(nodeInfo.NodeID))
                {
                    ischeck = "checked";
                }
                check = string.Format(@"<input type=""checkbox"" name=""NodeIDCollection"" value=""{0}"" " + ischeck + " {1}/>", nodeInfo.NodeID, nodeInfo.NodeName);
            }

            string disabled = "";
            if (!base.IsOwningNodeID(nodeInfo.NodeID))
            {
                disabled = "disabled";
                check = "";
            }

            itemBuilder.AppendFormat(@"<label class=""checkbox inline"">{1} {3}</label>&nbsp; ", nodeInfo.NodeID, check, disabled, nodeInfo.NodeName);

            return itemBuilder.ToString();
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.itemID = base.GetIntQueryString("ItemID");
            this.llPublishmentSystem.Text = base.PublishmentSystemInfo.PublishmentSystemName;

            this.NodeTree.Text = GetNodeTreeHtml();

            if (!base.IsPostBack)
            {
            }


        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {

                ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToArrayList(Request.Form["NodeIDCollection"]);

                ArrayList mLibScopeInfoArrayList = (ArrayList)base.Session[BackgroundUserNewGroupMLibSite.MLibScopeArrayListKey];
                ArrayList arraylist = new ArrayList();
                ArrayList lowsList = new ArrayList();
                if (nodeIDArrayList.Count > 0)
                {
                    if (mLibScopeInfoArrayList != null)
                    {
                        bool has = false;
                        foreach (string nodeid in nodeIDArrayList)
                        {
                            has = false;
                            foreach (MLibScopeInfo mLibScopeInfo in mLibScopeInfoArrayList)
                            {
                                if (mLibScopeInfo.PublishmentSystemID == base.PublishmentSystemID && mLibScopeInfo.NodeID == TranslateUtils.ToInt(nodeid))
                                {
                                    has = true;
                                    mLibScopeInfo.IsChecked = false;
                                    mLibScopeInfo.UserName = AdminManager.Current.UserName;

                                    arraylist.Add(mLibScopeInfo);
                                }
                                else
                                {
                                    if (mLibScopeInfo.PublishmentSystemID != base.PublishmentSystemID)
                                    {
                                        arraylist.Add(mLibScopeInfo);
                                    }
                                }
                            }
                            if (!has) { lowsList.Add(nodeid); }
                        }
                    }
                    foreach (string nodeid in lowsList)
                    {
                        MLibScopeInfo info = new MLibScopeInfo();
                        info.PublishmentSystemID = base.PublishmentSystemID;
                        info.NodeID = TranslateUtils.ToInt(nodeid);
                        info.UserName = AdminManager.Current.UserName;
                        arraylist.Add(info);
                    }
                }
                else
                {
                    foreach (MLibScopeInfo mLibScopeInfo in mLibScopeInfoArrayList)
                    {
                        if (mLibScopeInfo.PublishmentSystemID != base.PublishmentSystemID)
                        {
                            arraylist.Add(mLibScopeInfo);
                        }
                    }
                }
                base.Session[BackgroundUserNewGroupMLibSite.MLibScopeArrayListKey] = arraylist;


                if (base.GetQueryString("GroupName") == null)
                {
                    PageUtils.Redirect(PageUtils.GetPlatformUrl(string.Format(@"background_userNewGroupMLibSite.aspx?Return=True&ItemID={1}", this.itemID)));
                }
                else
                {
                    string groupName = base.GetQueryString("GroupName");
                    PageUtils.Redirect(PageUtils.GetPlatformUrl(string.Format("background_userNewGroupMLibSite.aspx?Return=True&GroupName={0}&ItemID={1}", groupName, this.itemID)));
                }
            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            if (base.GetQueryString("GroupName") == null)
            {
                PageUtils.Redirect(PageUtils.GetPlatformUrl(string.Format(@"background_userNewGroupMLibSite.aspx?Return=True&ItemID={1}", this.itemID)));
            }
            else
            {
                string groupName = base.GetQueryString("GroupName");
                PageUtils.Redirect(PageUtils.GetPlatformUrl(string.Format("background_userNewGroupMLibSite.aspx?Return=True&GroupName={0}&ItemID={1}", groupName, this.itemID)));
            }
        }

    }
}
