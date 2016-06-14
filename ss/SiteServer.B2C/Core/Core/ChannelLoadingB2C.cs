using System;
using System.Collections;
using System.Text;
using BaiRong.Core;
using System.Collections.Specialized;
using BaiRong.Model;
using SiteServer.B2C.Model;

using System.Collections.Generic;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Security;
using SiteServer.B2C.BackgroundPages;

namespace SiteServer.B2C.Core
{
    public class ChannelLoadingB2C
    {
        public static string GetRedirectUrl(int publishmentSystemID, int currentNodeID)
        {
            string redirectUrl = string.Empty;
            if (currentNodeID != 0 && currentNodeID != publishmentSystemID)
            {
                redirectUrl = PageUtils.GetB2CUrl(string.Format("background_channel.aspx?PublishmentSystemID={0}&CurrentNodeID={1}", publishmentSystemID, currentNodeID));
            }
            else
            {
                redirectUrl = PageUtils.GetB2CUrl(string.Format("background_channel.aspx?PublishmentSystemID={0}", publishmentSystemID));
            }

            return redirectUrl;
        }

        public static string GetChannelRowHtml(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, bool enabled, ELoadingType loadingType, NameValueCollection additional)
        {
            NodeTreeItem nodeTreeItem = NodeTreeItem.CreateInstance(nodeInfo, enabled);
            string title = nodeTreeItem.GetItemHtml(loadingType, ChannelLoadingB2C.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID), additional);

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
                string editUrl = string.Empty;
                string upLink = string.Empty;
                string downLink = string.Empty;
                string spec = string.Empty;
                string filter = string.Empty;
                string configuration = string.Empty;
                string checkBoxHtml = string.Empty;

                if (enabled)
                {
                    if (AdminUtility.HasChannelPermissions(nodeInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelEdit))
                    {
                        string urlEdit = PageUtils.GetCMSUrl(string.Format("background_channelEdit.aspx?NodeID={0}&PublishmentSystemID={1}&ReturnUrl={2}", nodeInfo.NodeID, nodeInfo.PublishmentSystemID, StringUtils.ValueToUrl(ChannelLoadingB2C.GetRedirectUrl(nodeInfo.PublishmentSystemID, nodeInfo.NodeID))));
                        editUrl = string.Format("<a href=\"{0}\">编辑</a>", urlEdit);
                        string urlSubtract = PageUtils.GetB2CUrl(string.Format("background_channel.aspx?PublishmentSystemID={0}&Subtract=True&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
                        upLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlSubtract);
                        string urlAdd = PageUtils.GetB2CUrl(string.Format("background_channel.aspx?PublishmentSystemID={0}&Add=True&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
                        downLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlAdd);

                        if (EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.Goods))
                        {
                            string urlSpec = BackgroundSpec.GetRedirectUrl(nodeInfo.PublishmentSystemID, nodeInfo.NodeID); 
                            //PageUtils.GetB2CUrl(string.Format("background_specChannel.aspx?PublishmentSystemID={0}&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
                            if (nodeInfo.Additional.SpecCount == 0)
                            {
                                spec = string.Format(@"<a href=""{0}"">规格项</a>", urlSpec);
                            }
                            else
                            {
                                spec = string.Format(@"<a href=""{0}"" class=""red"">规格项({1})</a>", urlSpec, nodeInfo.Additional.SpecCount);
                            }

                            string urlFilter = PageUtils.GetB2CUrl(string.Format("background_channelFilter.aspx?PublishmentSystemID={0}&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
                            if (nodeInfo.Additional.FilterCount == 0)
                            {
                                filter = string.Format(@"<a href=""{0}"">筛选属性</a>", urlFilter);
                            }
                            else
                            {
                                filter = string.Format(@"<a href=""{0}"" class=""red"">筛选属性({1})</a>", urlFilter, nodeInfo.Additional.FilterCount);
                            }

                            string urlConfiguration = PageUtils.GetB2CUrl(string.Format("background_configuration.aspx?PublishmentSystemID={0}&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
                            configuration = string.Format(@"<a href=""{0}"">设置</a>", urlConfiguration);
                        }
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
    <td class=""center"">
	    {8}
    </td>
    <td class=""center"">
	    {9}
    </td>
    <td class=""center"">
	    {10}
    </td>
</tr>
", nodeInfo.ParentsCount + 1, title, nodeInfo.NodeGroupNameCollection, nodeInfo.NodeIndexName, upLink, downLink, spec, filter, configuration, editUrl, checkBoxHtml);
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
            
            return rowHtml;
        }
    }
}
