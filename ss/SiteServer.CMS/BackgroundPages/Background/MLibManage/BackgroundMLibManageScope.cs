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
    public class BackgroundMLibManageScope : BackgroundBasePage
    {
        public Literal NodeTree;
        public Literal llPublishmentSystem;


        private string GetNodeTreeHtml()
        {
            StringBuilder htmlBuilder = new StringBuilder();
            ArrayList mLibScopeInfoArrayList = base.Session[BackgroundMLibManageScopeSite.MLibPublishmentSystemArrayListKey] as ArrayList;
            if (mLibScopeInfoArrayList == null)
            {
                PageUtils.RedirectToErrorPage("超出时间范围，请重新进入！");
                return string.Empty;
            }
            ArrayList nodeIDArrayList = new ArrayList();
            ArrayList isCheckedNodeIDArrayList = new ArrayList();
            foreach (MLibScopeInfo mLibScopeInfo in mLibScopeInfoArrayList)
            {
                if (mLibScopeInfo.PublishmentSystemID == base.PublishmentSystemID)
                {
                    nodeIDArrayList.Add(mLibScopeInfo.NodeID);
                    if (mLibScopeInfo.IsChecked)
                    {
                        isCheckedNodeIDArrayList.Add(mLibScopeInfo.NodeID);
                    }
                }
            }

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
                htmlBuilder.Append(GetTitle(nodeInfo, treeDirectoryUrl, IsLastNodeArray, nodeIDArrayList, isCheckedNodeIDArrayList));
                htmlBuilder.Append("<br/>");
            }
            htmlBuilder.Append("</span>");
            htmlBuilder.Append(@"<script type=""text/javascript"" language=""javascript"">
 function selectVal(_id){{
$(""#a_""+_id).attr(""href"",$(""#a_""+_id).attr(""href"")+""&IsChecked=""+$(""#ck_""+_id).prop(""checked""));
}} 
</script>");
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
                check = "checked";
            }
            string ischeck = "";
            if (isCheckedNodeID.Contains(nodeInfo.NodeID))
            {
                ischeck = "checked";
            }

            string disabled = "";
            if (!base.IsOwningNodeID(nodeInfo.NodeID))
            {
                disabled = "disabled";
                check = "";
            }

            itemBuilder.AppendFormat(@"<label class=""checkbox inline""><input type=""checkbox"" name=""NodeIDCollection"" value=""{0}"" {1} {2}/> {3}</label>&nbsp;<label class=""checkbox inline""><input type=""checkbox"" id=""ck_{0}"" name=""IsCheckNodeIDCollection"" onchange=""selectVal('{0}')"" value=""{0}"" {6} />{4}</label><a id=""a_{0}"" class=""checkbox inline"" href=""{5}"">设置显示字段</a>", nodeInfo.NodeID, check, disabled, nodeInfo.NodeName, string.Format("&nbsp;<span style=\"font-size:8pt;font-family:arial\" class=\"gray\">{0}</span>", "需要审批"), BackgroundMLibManageTableStyleContent.GetRedirectUrl(base.PublishmentSystemID, nodeInfo.NodeID) + "&IsChecked=" + (ischeck == "" ? false : true), ischeck);

            return itemBuilder.ToString();
        }

        private string GetTitle(NodeInfo nodeInfo, string treeDirectoryUrl, bool[] IsLastNodeArray, IList nodeIDArrayList)
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
                check = "checked";
            }

            string disabled = "";
            if (!base.IsOwningNodeID(nodeInfo.NodeID))
            {
                disabled = "disabled";
                check = "";
            }

            itemBuilder.AppendFormat(@"<label class=""checkbox inline""><input type=""checkbox"" name=""NodeIDCollection"" value=""{0}"" {1} {2}/> {3}</label>&nbsp;<label class=""checkbox inline""><input type=""checkbox"" name=""IsCheckNodeIDCollection"" value=""{0}"" {1} {2}/>{4}</label><a class=""checkbox inline"" href=""{5}"">设置显示字段</a>", nodeInfo.NodeID, check, disabled, nodeInfo.NodeName, string.Format("&nbsp;<span style=\"font-size:8pt;font-family:arial\" class=\"gray\">{0}</span>", "需要审批"), BackgroundMLibManageTableStyleContent.GetRedirectUrl(base.PublishmentSystemID, nodeInfo.NodeID));

            return itemBuilder.ToString();
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

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
                ArrayList isCheckNodeIDList = TranslateUtils.StringCollectionToArrayList(Request.Form["IsCheckNodeIDCollection"]);

                ArrayList mLibScopeInfoArrayList = (ArrayList)base.Session[BackgroundMLibManageScopeSite.MLibPublishmentSystemArrayListKey];
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
                                    foreach (string checkid in isCheckNodeIDList)
                                    {
                                        if (nodeid == checkid)
                                            mLibScopeInfo.IsChecked = true;
                                    }
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
                        foreach (string checkid in isCheckNodeIDList)
                        {
                            if (nodeid == checkid)
                                info.IsChecked = true;
                        }
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
                base.Session[BackgroundMLibManageScopeSite.MLibPublishmentSystemArrayListKey] = arraylist;

                PageUtils.Redirect(PageUtils.GetPlatformUrl("background_mlibManageScopeSite.aspx?Return=True"));
            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(PageUtils.GetPlatformUrl("background_mlibManageScopeSite.aspx?Return=True"));
        }

    }
}
