using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace SiteServer.WCM.Core
{
    public class ChannelLoadingWCM
    {
        public static string GetChannelRowHtml(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, bool enabled, ELoadingType loadingType, NameValueCollection additional)
        {
            NodeTreeItem nodeTreeItem = NodeTreeItem.CreateInstance(nodeInfo, enabled);
            string title = nodeTreeItem.GetItemHtml(loadingType, ChannelLoading.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID), additional);

            string rowHtml = string.Empty;

            if (loadingType == ELoadingType.GovPublicChannel)
            {
                string editUrl = string.Empty;
                string upLink = string.Empty;
                string downLink = string.Empty;
                string checkBoxHtml = string.Empty;

                if (!EContentModelTypeUtils.Equals(EContentModelType.GovPublic, nodeInfo.ContentModelID))
                {
                    enabled = false;
                }

                if (enabled)
                {
                    editUrl = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", SiteServer.WCM.BackgroundPages.Modal.GovPublicChannelAdd.GetOpenWindowStringToEdit(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, string.Empty));

                    string urlUp = PageUtils.GetWCMUrl(string.Format("background_govPublicChannel.aspx?PublishmentSystemID={0}&Subtract=True&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
                    upLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlUp);

                    string urlDown = PageUtils.GetWCMUrl(string.Format("background_govPublicChannel.aspx?PublishmentSystemID={0}&Add=True&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
                    downLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlDown);

                    checkBoxHtml = string.Format("<input type='checkbox' name='ChannelIDCollection' value='{0}' />", nodeInfo.NodeID);
                }

                string channelCode = DataProvider.GovPublicChannelDAO.GetCode(nodeInfo.NodeID);

                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
    <td>{1}</td>
    <td>{2}</td>
    <td class=""center"">{3}</td>
    <td class=""center"">{4}</td>
    <td class=""center"">{5}</td>
    <td class=""center"">{6}</td>
</tr>
", nodeInfo.ParentsCount + 1, title, channelCode, upLink, downLink, editUrl, checkBoxHtml);
            }
            else if (loadingType == ELoadingType.GovInteractChannel)
            {
                string editUrl = string.Empty;
                string upLink = string.Empty;
                string downLink = string.Empty;
                string styleAddUrl = string.Empty;
                string checkBoxHtml = string.Empty;

                if (enabled)
                {
                    int applyStyleID = DataProvider.GovInteractChannelDAO.GetApplyStyleID(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);
                    editUrl = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", SiteServer.WCM.BackgroundPages.Modal.GovInteractChannelAdd.GetOpenWindowStringToEdit(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, string.Empty));

                    string urlUp = PageUtils.GetWCMUrl(string.Format("background_govInteractChannel.aspx?PublishmentSystemID={0}&Subtract=True&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
                    upLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlUp);

                    string urlDown = PageUtils.GetWCMUrl(string.Format("background_govInteractChannel.aspx?PublishmentSystemID={0}&Add=True&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
                    downLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlDown);

                    styleAddUrl = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">提交设置</a>", PageUtility.ModalSTL.TagStyleGovInteractApplyAdd_GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, applyStyleID));
                    checkBoxHtml = string.Format("<input type='checkbox' name='ChannelIDCollection' value='{0}' />", nodeInfo.NodeID);
                }

                string summary = DataProvider.GovInteractChannelDAO.GetSummary(nodeInfo.NodeID);

                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
    <td>{1}</td>
    <td>{2}</td>
    <td class=""center"">{3}</td>
    <td class=""center"">{4}</td>
    <td class=""center"">{5}</td>
    <td class=""center"">{6}</td>
    <td class=""center"">{7}</td>
</tr>
", nodeInfo.ParentsCount + 1, title, summary, upLink, downLink, styleAddUrl, editUrl, checkBoxHtml);
            }
            return rowHtml;
        }
    }
}
