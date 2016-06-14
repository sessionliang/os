using System;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Security;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace SiteServer.CMS.Controls
{
    /// <summary>
    /// 获取树形结构的数据集合委托
    /// </summary>
    /// <returns></returns>
    public delegate void DelegateLoadItemArrayList<T>(Tree sender, EventArgs e) where T : TreeBaseItem;

    public class Tree : Control
    {
        #region 公共属性

        /// <summary>
        /// 加载树形结构的数据集合事件
        /// </summary>
        public event DelegateLoadItemArrayList<TreeBaseItem> OnLoadItemArrayList;

        /// <summary>
        /// 集合
        /// </summary>
        public List<TreeBaseItem> ItemArrayList = new List<TreeBaseItem>();

        /// <summary>
        /// 加载分类类型
        /// </summary>
        public string ClassifyType;

        /// <summary>
        /// 动作类型（树，管理）
        /// </summary>
        public string ActionType = "ClassifyTree";

        /// <summary>
        /// 分类跳转地址
        /// </summary>
        public string LinkUrl;

        /// <summary>
        /// 分类文件夹打开地址
        /// </summary>
        public string RedirectUrl;

        /// <summary>
        /// 是否显示内容数目
        /// </summary>
        public bool ShowCount = true;

        public int ShowLayer = 0;

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!string.IsNullOrEmpty(this.ClassifyType))
            {
                //加载数据
                ItemArrayList.AddRange(TreeManager.GetItemInfoArrayListByParentID(int.Parse(base.Page.Request.QueryString["PublishmentSystemID"]), 0, this.ClassifyType));
            }
            //用户手动操作
            if (OnLoadItemArrayList != null)
                OnLoadItemArrayList(this, e);

        }

        private PublishmentSystemInfo publishmentSystemInfo;
        string rightPageURL = string.Empty;

        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder builder = new StringBuilder();

            int publishmentSystemID = int.Parse(base.Page.Request.QueryString["PublishmentSystemID"]);
            this.publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (this.publishmentSystemInfo == null)
                this.publishmentSystemInfo = new PublishmentSystemInfo();
            this.rightPageURL = StringUtils.ValueFromUrl(base.Page.Request.QueryString["RightPageURL"]);
            NameValueCollection additional = new NameValueCollection();
            if (!string.IsNullOrEmpty(LinkUrl))
                additional.Add("LinkUrl", StringUtils.ValueFromUrl(LinkUrl));
            else if (!string.IsNullOrEmpty(this.rightPageURL))
                additional.Add("LinkUrl", this.rightPageURL);

            if (!string.IsNullOrEmpty(RedirectUrl))
                additional.Add("RedirectUrl", StringUtils.ValueFromUrl(RedirectUrl));
            if (ShowLayer > 0)
                additional.Add("ShowLayer", ShowLayer.ToString());


            string scripts = Tree.GetScript(this.publishmentSystemInfo, additional, this.ClassifyType, this.ActionType);
            builder.Append(scripts);
            if (base.Page.Request.QueryString["PublishmentSystemID"] != null)
            {

                try
                {
                    foreach (TreeBaseItem item in ItemArrayList)
                    {
                        bool enabled = item.Enabled;
                        additional["itemID"] = item.ItemID.ToString();
                        builder.Append(Tree.GetItemRowHtml(this.publishmentSystemInfo, item, this.ClassifyType, additional, this.ShowCount, this.ShowLayer));
                    }
                }
                catch (Exception ex)
                {
                    PageUtils.RedirectToErrorPage(ex.Message);
                }
            }
            writer.Write(builder);
        }

        /// <summary>
        /// 获取每一个分类的html
        /// </summary>
        /// <param name="publishmentSystemInfo"></param>
        /// <param name="itemInfo"></param>
        /// <param name="classifyType"></param>
        /// <param name="additional"></param>
        /// <returns></returns>
        public static string GetItemRowHtml(PublishmentSystemInfo publishmentSystemInfo, TreeBaseItem itemInfo, string classifyType, NameValueCollection additional, bool showCount, int showLayer)
        {
            TreeItem treeItem = TreeItem.CreateInstance(itemInfo, classifyType);

            if (publishmentSystemInfo == null)
                publishmentSystemInfo = new PublishmentSystemInfo();

            if (additional == null)
            {
                additional = new NameValueCollection();
                additional.Add("LinkUrl", additional["linkUrl"]);
                additional.Add("RedirectUrl", additional["RedirectUrl"]);
            }
            string title = treeItem.GetItemHtml(Tree.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, itemInfo.ItemID, additional["redirectUrl"]), additional, false, showCount, showLayer);

            string rowHtml = string.Empty;

            rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
	<td align=""left"" nowrap>
		{1}
	</td>
</tr>
", itemInfo.ParentsCount + 1, title);

            return rowHtml;
        }


        /// <summary>
        /// 获取每一个分类的html
        /// </summary>
        /// <param name="publishmentSystemInfo"></param>
        /// <param name="itemInfo"></param>
        /// <param name="classifyType"></param>
        /// <param name="additional"></param>
        /// <returns></returns>
        public static string GetItemRowHtmlForManage(PublishmentSystemInfo publishmentSystemInfo, TreeBaseItem itemInfo, string classifyType, NameValueCollection additional)
        {
            return GetItemRowHtmlForManage(publishmentSystemInfo, itemInfo, classifyType, additional, 0);
        }

        /// <summary>
        /// 获取每一个分类的html
        /// </summary>
        /// <param name="publishmentSystemInfo"></param>
        /// <param name="itemInfo"></param>
        /// <param name="classifyType"></param>
        /// <param name="additional"></param>
        /// <returns></returns>
        public static string GetItemRowHtmlForManage(PublishmentSystemInfo publishmentSystemInfo, TreeBaseItem itemInfo, string classifyType, NameValueCollection additional, int showLayer)
        {
            TreeItem treeItem = TreeItem.CreateInstance(itemInfo, classifyType);

            if (publishmentSystemInfo == null)
                publishmentSystemInfo = new PublishmentSystemInfo();

            if (additional == null)
            {
                additional = new NameValueCollection();
                additional.Add("LinkUrl", additional["linkUrl"]);
                additional.Add("RedirectUrl", additional["redirectUrl"]);
            }


            string title = treeItem.GetItemHtml(Tree.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, itemInfo.ItemID, additional["redirectUrl"]), additional, false, showLayer);

            string checkBoxHtml = string.Empty;
            string rowHtml = string.Empty;
            string upLink = "javascript:;";
            string downLink = "javascript:;";
            string editLink = "javascript:;";
            if (itemInfo.Enabled)
            {
                string urlEdit = PageUtils.GetCMSUrl(string.Format("{3}?ItemID={0}&PublishmentSystemID={1}&ReturnUrl={2}", itemInfo.ItemID, itemInfo.PublishmentSystemID, StringUtils.ValueToUrl(additional["returnUrl"]), additional["editLink"]));
                string urlSubtract = PageUtils.GetCMSUrl(string.Format("{2}?PublishmentSystemID={0}&Subtract=True&ItemID={1}", itemInfo.PublishmentSystemID, itemInfo.ItemID, additional["upLink"]));
                string urlAdd = PageUtils.GetCMSUrl(string.Format("{2}?PublishmentSystemID={0}&Add=True&ItemID={1}", itemInfo.PublishmentSystemID, itemInfo.ItemID, additional["downLink"]));

                string paramStr = "";
                paramStr = additional["linkParam"];//by 20151113 sofuny  增加参数
                if (paramStr != "")
                {
                    urlEdit = PageUtils.GetCMSUrl(string.Format("{3}ItemID={0}&PublishmentSystemID={1}{4}&ReturnUrl={2}", itemInfo.ItemID, itemInfo.PublishmentSystemID, StringUtils.ValueToUrl(additional["returnUrl"]), additional["editLink"].IndexOf('?') > 0 ? additional["editLink"] : additional["editLink"] + "?", paramStr));

                    urlSubtract = PageUtils.GetCMSUrl(string.Format("{2}PublishmentSystemID={0}&Subtract=True&ItemID={1}{3}", itemInfo.PublishmentSystemID, itemInfo.ItemID, additional["upLink"].IndexOf('?') > 0 ? additional["upLink"] : additional["upLink"] + "?", paramStr));

                    urlAdd = PageUtils.GetCMSUrl(string.Format("{2}PublishmentSystemID={0}&Add=True&ItemID={1}{3}", itemInfo.PublishmentSystemID, itemInfo.ItemID, additional["downLink"].IndexOf('?') > 0 ? additional["downLink"] : additional["downLink"] + "?", paramStr));
                }

                editLink = string.Format("<a href=\"{0}\">编辑</a>", urlEdit);


                upLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlSubtract);


                downLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlAdd);

                checkBoxHtml = string.Format("<input type='checkbox' name='ItemIDCollection' value='{0}' />", itemInfo.ItemID);
            }
            else//by 20151111 sofuny
            {
                upLink = " ";
                downLink = " ";
                editLink = " ";
            }

            string showAttribute = additional["showAttribute"];//by 20151113 sofuny  显示列

            rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
    <td>{1}</td>
    <td class=""center"">
	    {2}
    </td>
    <td class=""center"">
	    {3}
    </td>
    <td class=""center"">
	    {4}
    </td>
    <td class=""center"">
	    {5}
    </td>
    <td class=""center"">
	    {6}
    </td>
</tr>
", itemInfo.ParentsCount + 1, title, showAttribute != null ? itemInfo.ContentNum.ToString() : itemInfo.ItemIndexName, upLink, downLink, editLink, checkBoxHtml);

            return rowHtml;
        }

        /// <summary>
        /// 点击文件夹跳转地址
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="itemID"></param>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        public static string GetRedirectUrl(int publishmentSystemID, int itemID, string redirectUrl)
        {
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                if (itemID != 0 && itemID != publishmentSystemID)
                {
                    if (redirectUrl.IndexOf('?') > 0) //by 20151029 sofuny
                        redirectUrl = PageUtils.GetCMSUrl(string.Format("{0}&PublishmentSystemID={1}&CurrentItemID={2}", redirectUrl, publishmentSystemID, itemID));
                    else
                        redirectUrl = PageUtils.GetCMSUrl(string.Format("{0}?PublishmentSystemID={1}&CurrentItemID={2}", redirectUrl, publishmentSystemID, itemID));
                }
                else
                {
                    if (redirectUrl.IndexOf('?') > 0)
                        redirectUrl = PageUtils.GetCMSUrl(string.Format("{0}&PublishmentSystemID={1}", redirectUrl, publishmentSystemID));
                    else
                        redirectUrl = PageUtils.GetCMSUrl(string.Format("{0}?PublishmentSystemID={1}", redirectUrl, publishmentSystemID));
                }
            }
            else
            {
                redirectUrl = "javascript:;";
            }
            return redirectUrl;
        }

        /// <summary>
        /// 动作js
        /// </summary>
        /// <param name="publishmentSystemInfo"></param>
        /// <param name="additional"></param>
        /// <param name="classifyType"></param>
        /// <returns></returns>
        public static string GetScript(PublishmentSystemInfo publishmentSystemInfo, NameValueCollection additional, string classifyType, string actionType)
        {
            if (publishmentSystemInfo == null)
                publishmentSystemInfo = new PublishmentSystemInfo();
            return TreeItem.GetScript(publishmentSystemInfo, additional, classifyType, actionType);
        }
    }
}