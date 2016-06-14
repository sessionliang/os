using System;
using System.Collections;
using System.Text;
using BaiRong.Core;
using System.Collections.Specialized;
using SiteServer.CMS.Core.Security;
using BaiRong.Model;
using SiteServer.CMS.Model;


namespace SiteServer.CMS.Core
{
    public class ChannelLoading
    {
        public static string GetRedirectUrl(int publishmentSystemID, int currentNodeID)
        {
            string redirectUrl = string.Empty;
            if (currentNodeID != 0 && currentNodeID != publishmentSystemID)
            {
                redirectUrl = PageUtils.GetCMSUrl(string.Format("background_channel.aspx?PublishmentSystemID={0}&CurrentNodeID={1}", publishmentSystemID, currentNodeID));
            }
            else
            {
                redirectUrl = PageUtils.GetCMSUrl(string.Format("background_channel.aspx?PublishmentSystemID={0}", publishmentSystemID));
            }

            return redirectUrl;
        }

        public static string GetChannelEditOpenWindowString(int publishmentSystemID, int nodeID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return JsUtils.Layer.GetOpenLayerString("快速修改栏目", PageUtils.GetCMSUrl("modal_channelEdit.aspx"), arguments);
        }

        public static string GetTemplateFilePathRuleOpenWindowString(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("页面命名规则", PageUtils.GetSTLUrl("modal_templateFilePathRule.aspx"), arguments);
        }

        public static string GetConfigurationCreateChannelOpenWindowString(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("栏目生成设置", PageUtils.GetSTLUrl("modal_configurationCreateChannel.aspx"), arguments, 550, 400);
        }

        public static string GetConfigurationCreateIncludeOpenWindowString(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("包含文件生成设置", PageUtils.GetSTLUrl("modal_configurationCreateInclude.aspx"), arguments, 520, 360);
        }

        public static string GetCrossSiteTransEditOpenWindowString(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            return PageUtility.GetOpenWindowString("跨站转发设置", "modal_crossSiteTransEdit.aspx", arguments, 650, 500);
        }

        public static string GetConfigurationSigninOpenWindowString(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            return PageUtility.GetOpenWindowString("签收设置", "modal_configurationSignin.aspx", arguments, 480, 400);
        }

        //public static string GetConfigurationCommentOpenWindowStringxxx(int publishmentSystemID, int nodeID)
        //{
        //    NameValueCollection arguments = new NameValueCollection();
        //    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
        //    arguments.Add("NodeID", nodeID.ToString());
        //    return PageUtility.GetOpenWindowString("评论设置", "modal_configurationComment.aspx", arguments, 480, 420);
        //}

        public static string GetChannelRowHtml(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, bool enabled, ELoadingType loadingType, NameValueCollection additional)
        {
            NodeTreeItem nodeTreeItem = NodeTreeItem.CreateInstance(nodeInfo, enabled);
            string title = nodeTreeItem.GetItemHtml(loadingType, ChannelLoading.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID), additional);

            string rowHtml = string.Empty;

            if (loadingType == ELoadingType.ContentTree)
            {
                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
	<td align=""left"" nowrap>
		{1}
	</td>
</tr>
", nodeInfo.ParentsCount + 1, title);
            }
            else if (loadingType == ELoadingType.Channel)
            {
                string upLink = string.Empty;
                string downLink = string.Empty;
                string editUrl = string.Empty;
                string checkBoxHtml = string.Empty;

                if (enabled)
                {
                    if (AdminUtility.HasChannelPermissions(nodeInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelEdit))
                    {
                        string urlEdit = PageUtils.GetCMSUrl(string.Format("background_channelEdit.aspx?NodeID={0}&PublishmentSystemID={1}&ReturnUrl={2}", nodeInfo.NodeID, nodeInfo.PublishmentSystemID, StringUtils.ValueToUrl(ChannelLoading.GetRedirectUrl(nodeInfo.PublishmentSystemID, nodeInfo.NodeID))));
                        editUrl = string.Format("<a href=\"{0}\">编辑</a>", urlEdit);
                        string urlSubtract = PageUtils.GetCMSUrl(string.Format("background_channel.aspx?PublishmentSystemID={0}&Subtract=True&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
                        upLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlSubtract);
                        string urlAdd = PageUtils.GetCMSUrl(string.Format("background_channel.aspx?PublishmentSystemID={0}&Add=True&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
                        downLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlAdd);
                    }
                    checkBoxHtml = string.Format("<input type='checkbox' name='ChannelIDCollection' value='{0}' />", nodeInfo.NodeID);
                }

                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
    <td>{1}</td>
    <td>{2}</td>
    <td><nobr>{3}</nobr></td>
    <td class=""center"">
	    {4}
    </td>
    <td class=""center"">
	    {5}
    </td>
    <td class=""center"">
	    {6}
    </td>
    <td class=""center"">
	    {7}
    </td>
</tr>
", nodeInfo.ParentsCount + 1, title, nodeInfo.NodeGroupNameCollection, nodeInfo.NodeIndexName, upLink, downLink, editUrl, checkBoxHtml);
            }
            else if (loadingType == ELoadingType.SiteAnalysis)
            {
                string contentAddNum = string.Empty;
                string contentUpdateNum = string.Empty;
                string commentAddNum = string.Empty;

                DateTime startDate = TranslateUtils.ToDateTime(additional["StartDate"]);
                DateTime endDate = TranslateUtils.ToDateTime(additional["EndDate"]);

                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                int num = DataProvider.ContentDAO.GetCountOfContentAdd(tableName, publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, startDate, endDate, string.Empty);
                contentAddNum = (num == 0) ? "0" : string.Format("<strong>{0}</strong>", num);

                num = DataProvider.ContentDAO.GetCountOfContentUpdate(tableName, publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, startDate, endDate, string.Empty);
                contentUpdateNum = (num == 0) ? "0" : string.Format("<strong>{0}</strong>", num);

                num = DataProvider.CommentDAO.GetCountChecked(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, startDate, endDate);
                commentAddNum = (num == 0) ? "0" : string.Format("<strong>{0}</strong>", num);

                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
	<td>
		<nobr>{1}</nobr>
	</td>
	<td>
		{2}
	</td>
	<td>
		{3}
	</td>
	<td>
		{4}
	</td>
</tr>
", nodeInfo.ParentsCount + 1, title, contentAddNum, contentUpdateNum, commentAddNum);
            }
            else if (loadingType == ELoadingType.TemplateFilePathRule)
            {
                string editLink = string.Empty;

                string filePath = string.Empty;

                if (enabled)
                {
                    string showPopWinString = ChannelLoading.GetTemplateFilePathRuleOpenWindowString(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);
                    editLink = string.Format("<a href=\"javascript:;\" onclick=\"{0}\">更改</a>", showPopWinString);
                }
                filePath = PageUtility.GetInputChannelUrl(publishmentSystemInfo, nodeInfo, publishmentSystemInfo.Additional.VisualType);

                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
	<td>
		<nobr>{1}</nobr>
	</td>
	<td>
		<nobr>{2}</nobr>
	</td>
	<td class=""center"">
		{3}
	</td>
</tr>
", nodeInfo.ParentsCount + 1, title, filePath, editLink);
            }
            else if (loadingType == ELoadingType.ConfigurationCreateDetails)
            {
                string editChannelLink = string.Empty;
                string editIncludeLink = string.Empty;

                string nodeNames = string.Empty;
                string includeFiles = string.Empty;

                if (enabled)
                {
                    string showPopWinString = ChannelLoading.GetConfigurationCreateChannelOpenWindowString(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);
                    editChannelLink = string.Format("<a href=\"javascript:;\" onclick=\"{0}\">触发栏目</a>", showPopWinString);
                    showPopWinString = ChannelLoading.GetConfigurationCreateIncludeOpenWindowString(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);
                    editIncludeLink = string.Format("<a href=\"javascript:;\" onclick=\"{0}\">触发包含文件</a>", showPopWinString);
                }

                if (nodeInfo.Additional.Attributes.Count > 0)
                {
                    StringBuilder nodeNameBuilder = new StringBuilder();
                    ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(nodeInfo.Additional.CreateChannelIDsIfContentChanged);
                    foreach (int theNodeID in nodeIDArrayList)
                    {
                        NodeInfo theNodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, theNodeID);
                        if (theNodeInfo != null)
                        {
                            nodeNameBuilder.Append(theNodeInfo.NodeName).Append(",");
                        }
                    }
                    if (nodeNameBuilder.Length > 0)
                    {
                        nodeNameBuilder.Length--;
                        nodeNames = nodeNameBuilder.ToString();
                    }

                    StringBuilder includeFileBuilder = new StringBuilder();
                    ArrayList includeFileArrayList = TranslateUtils.StringCollectionToArrayList(nodeInfo.Additional.CreateIncludeFilesIfContentChanged);
                    foreach (string includeFile in includeFileArrayList)
                    {
                        includeFileBuilder.Append(includeFile).Append(",");
                    }
                    if (includeFileBuilder.Length > 0)
                    {
                        includeFileBuilder.Length--;
                        includeFiles = includeFileBuilder.ToString();
                    }
                }

                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
	<td>
		<nobr>{1}</nobr>
	</td>
	<td>
		{2}
	</td>
    <td>
		{3}
	</td>
	<td class=""center"">
		{4}
	</td>
    <td class=""center"">
		{5}
	</td>
</tr>
", nodeInfo.ParentsCount + 1, title, nodeNames, includeFiles, editChannelLink, editIncludeLink);
            }
            else if (loadingType == ELoadingType.ConfigurationCrossSiteTrans)
            {
                string editLink = string.Empty;

                string contribute = string.Empty;

                if (enabled)
                {
                    string showPopWinString = ChannelLoading.GetCrossSiteTransEditOpenWindowString(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);
                    editLink = string.Format("<a href=\"javascript:;\" onclick=\"{0}\">更改</a>", showPopWinString);
                }

                contribute = CrossSiteTransUtility.GetDescription(nodeInfo.PublishmentSystemID, nodeInfo);

                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
	<td>{1}</td>
	<td>{2}</td>
	<td class=""center"" width=""50"">{3}</td>
</tr>
", nodeInfo.ParentsCount + 1, title, contribute, editLink);
            }
            else if (loadingType == ELoadingType.ConfigurationSignin)
            {
                string editLink = string.Empty;

                if (enabled)
                {
                    string showPopWinString = ChannelLoading.GetConfigurationSigninOpenWindowString(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);
                    editLink = string.Format("<a href=\"javascript:;\" onclick=\"{0}\">更改</a>", showPopWinString);
                }

                //string contribute = CrossSiteTransUtility.GetDescription(nodeInfo.PublishmentSystemID, nodeInfo);
                string isSign = "";
                string SignUser = "";
                if (nodeInfo.Additional.IsSignin)
                {
                    isSign = "是";
                }
                else
                {
                    isSign = "否";
                }
                //if (!string.IsNullOrEmpty(nodeInfo.Additional.SigninUserGroupCollection))
                //{
                //    ArrayList groupIDlist = TranslateUtils.StringCollectionToIntArrayList(nodeInfo.Additional.SigninUserGroupCollection);
                //    UserGroupInfo userGroupInfo = null;
                //    foreach (int groupID in groupIDlist)
                //    {
                //        userGroupInfo = DataProvider.UserGroupDAO.GetUserGroupMessage(groupID);
                //        SignUser += userGroupInfo.GroupName + ',';
                //    }
                //    SignUser = SignUser.TrimEnd(',');
                //}
                //else
                //{
                SignUser = nodeInfo.Additional.SigninUserNameCollection;
                //}

                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
	<td>{1}</td>
    <td>{2}</td>
	<td class=""center"">{3}</td>
	<td class=""center"">{4}</td>
</tr>
", nodeInfo.ParentsCount + 1, title, SignUser, isSign, editLink);
            }
            else if (loadingType == ELoadingType.ChannelSelect || loadingType == ELoadingType.GovPublicChannelAdd || loadingType == ELoadingType.GovPublicChannelTree)
            {
                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
	<td nowrap>{1}</td>
</tr>
", nodeInfo.ParentsCount + 1, title);
            }
            else if (loadingType == ELoadingType.EvaluationNodeTree || loadingType == ELoadingType.TrialApplyNodeTree || loadingType == ELoadingType.TrialReportNodeTree || loadingType == ELoadingType.SurveyNodeTree || loadingType == ELoadingType.TrialAnalysisNodeTree || loadingType == ELoadingType.SurveyAnalysisNodeTree || loadingType == ELoadingType.CompareNodeTree)
            {
                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
	<td align=""left"" nowrap>
		{1}
	</td>
</tr>
", nodeInfo.ParentsCount + 1, title);
            }

            //            else if (loadingType == ELoadingType.GovPublicChannel)
            //            {
            //                string editUrl = string.Empty;
            //                string upLink = string.Empty;
            //                string downLink = string.Empty;
            //                string checkBoxHtml = string.Empty;

            //                if (!EContentModelTypeUtils.Equals(EContentModelType.GovPublic, nodeInfo.ContentModelID))
            //                {
            //                    enabled = false;
            //                }

            //                if (enabled)
            //                {
            //                    editUrl = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", SiteServer.CMS.BackgroundPages.Modal.GovPublicChannelAdd.GetOpenWindowStringToEdit(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, string.Empty));

            //                    string urlUp = PageUtils.GetWCMUrl(string.Format("background_govPublicChannel.aspx?PublishmentSystemID={0}&Subtract=True&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
            //                    upLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlUp);

            //                    string urlDown = PageUtils.GetWCMUrl(string.Format("background_govPublicChannel.aspx?PublishmentSystemID={0}&Add=True&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
            //                    downLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlDown);

            //                    checkBoxHtml = string.Format("<input type='checkbox' name='ChannelIDCollection' value='{0}' />", nodeInfo.NodeID);
            //                }

            //                string channelCode = DataProvider.GovPublicChannelDAO.GetCode(nodeInfo.NodeID);

            //                rowHtml = string.Format(@"
            //<tr treeItemLevel=""{0}"">
            //    <td>{1}</td>
            //    <td>{2}</td>
            //    <td class=""center"">{3}</td>
            //    <td class=""center"">{4}</td>
            //    <td class=""center"">{5}</td>
            //    <td class=""center"">{6}</td>
            //</tr>
            //", nodeInfo.ParentsCount + 1, title, channelCode, upLink, downLink, editUrl, checkBoxHtml);
            //            }
            //            else if (loadingType == ELoadingType.GovInteractChannel)
            //            {
            //                string editUrl = string.Empty;
            //                string upLink = string.Empty;
            //                string downLink = string.Empty;
            //                string styleAddUrl = string.Empty;
            //                string checkBoxHtml = string.Empty;

            //                if (enabled)
            //                {
            //                    int applyStyleID = DataProvider.GovInteractChannelDAO.GetApplyStyleID(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);
            //                    editUrl = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", SiteServer.CMS.BackgroundPages.Modal.GovInteractChannelAdd.GetOpenWindowStringToEdit(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, string.Empty));

            //                    string urlUp = PageUtils.GetWCMUrl(string.Format("background_govInteractChannel.aspx?PublishmentSystemID={0}&Subtract=True&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
            //                    upLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlUp);

            //                    string urlDown = PageUtils.GetWCMUrl(string.Format("background_govInteractChannel.aspx?PublishmentSystemID={0}&Add=True&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
            //                    downLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlDown);

            //                    styleAddUrl = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">提交设置</a>", PageUtility.ModalSTL.TagStyleGovInteractApplyAdd_GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, applyStyleID));
            //                    checkBoxHtml = string.Format("<input type='checkbox' name='ChannelIDCollection' value='{0}' />", nodeInfo.NodeID);
            //                }

            //                string summary = DataProvider.GovInteractChannelDAO.GetSummary(nodeInfo.NodeID);

            //                rowHtml = string.Format(@"
            //<tr treeItemLevel=""{0}"">
            //    <td>{1}</td>
            //    <td>{2}</td>
            //    <td class=""center"">{3}</td>
            //    <td class=""center"">{4}</td>
            //    <td class=""center"">{5}</td>
            //    <td class=""center"">{6}</td>
            //    <td class=""center"">{7}</td>
            //</tr>
            //", nodeInfo.ParentsCount + 1, title, summary, upLink, downLink, styleAddUrl, editUrl, checkBoxHtml);
            //            }
            return rowHtml;
        }

        public static string GetScript(PublishmentSystemInfo publishmentSystemInfo, ELoadingType loadingType, NameValueCollection additional)
        {
            return NodeTreeItem.GetScript(publishmentSystemInfo, loadingType, additional);
        }

        public static string GetScriptOnLoad(int publishmentSystemID, int currentNodeID)
        {
            if (currentNodeID != 0 && currentNodeID != publishmentSystemID)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, currentNodeID);
                if (nodeInfo != null)
                {
                    string path = string.Empty;
                    if (nodeInfo.ParentID == publishmentSystemID)
                    {
                        path = currentNodeID.ToString();
                    }
                    else
                    {
                        path = nodeInfo.ParentsPath.Substring(nodeInfo.ParentsPath.IndexOf(",") + 1) + "," + currentNodeID.ToString();
                    }
                    return NodeTreeItem.GetScriptOnLoad(path);
                }
            }
            return string.Empty;
        }
         
    }
}
