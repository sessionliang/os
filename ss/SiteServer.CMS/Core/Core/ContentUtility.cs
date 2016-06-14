using BaiRong.Model;
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Model;
using System.Text;
using BaiRong.Core;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.BackgroundPages;
using System.Collections.Generic;
using SiteServer.CMS.BackgroundPages.Modal;


namespace SiteServer.CMS.Core
{
    public class ContentUtility
    {
        private ContentUtility()
        {
        }

        public static string PagePlaceHolder = "[SITESERVER_PAGE]";//内容翻页展位符

        public static string GetTitleFormatString(bool isStrong, bool isEM, bool isU, string color)
        {
            return string.Format("{0}_{1}_{2}_{3}", isStrong, isEM, isU, color);
        }

        public static bool SetTitleFormatControls(string titleFormatString, CheckBox formatStrong, CheckBox formatEM, CheckBox formatU, TextBox formatColor)
        {
            bool isTitleFormatted = false;
            if (!string.IsNullOrEmpty(titleFormatString))
            {
                string[] formats = titleFormatString.Split('_');
                if (formats.Length == 4)
                {
                    formatStrong.Checked = TranslateUtils.ToBool(formats[0]);
                    formatEM.Checked = TranslateUtils.ToBool(formats[1]);
                    formatU.Checked = TranslateUtils.ToBool(formats[2]);
                    formatColor.Text = formats[3];
                    if (formatStrong.Checked || formatEM.Checked || formatU.Checked || !string.IsNullOrEmpty(formatColor.Text))
                    {
                        isTitleFormatted = true;
                    }
                }
            }
            return isTitleFormatted;
        }

        public static bool SetTitleFormatControls(string titleFormatString, out bool formatStrong, out bool formatEM, out bool formatU, out string formatColor)
        {
            bool isTitleFormatted = false;

            formatStrong = formatEM = formatU = false;
            formatColor = string.Empty;

            if (!string.IsNullOrEmpty(titleFormatString))
            {
                string[] formats = titleFormatString.Split('_');
                if (formats.Length == 4)
                {
                    formatStrong = TranslateUtils.ToBool(formats[0]);
                    formatEM = TranslateUtils.ToBool(formats[1]);
                    formatU = TranslateUtils.ToBool(formats[2]);
                    formatColor = formats[3];
                    if (formatStrong || formatEM || formatU || !string.IsNullOrEmpty(formatColor))
                    {
                        isTitleFormatted = true;
                    }
                }
            }
            return isTitleFormatted;
        }

        public static string FormatTitle(string titleFormatString, string title)
        {
            string formattedTitle = title;
            if (!string.IsNullOrEmpty(titleFormatString))
            {
                string[] formats = titleFormatString.Split('_');
                if (formats.Length == 4)
                {
                    bool isStrong = TranslateUtils.ToBool(formats[0]);
                    bool isEM = TranslateUtils.ToBool(formats[1]);
                    bool isU = TranslateUtils.ToBool(formats[2]);
                    string color = formats[3];

                    if (!string.IsNullOrEmpty(color))
                    {
                        if (!color.StartsWith("#"))
                        {
                            color = "#" + color;
                        }
                        formattedTitle = string.Format(@"<span style=""color:{0}"">{1}</span>", color, formattedTitle);
                    }
                    if (isStrong)
                    {
                        formattedTitle = string.Format("<strong>{0}</strong>", formattedTitle);
                    }
                    if (isEM)
                    {
                        formattedTitle = string.Format("<em>{0}</em>", formattedTitle);
                    }
                    if (isU)
                    {
                        formattedTitle = string.Format("<u>{0}</u>", formattedTitle);
                    }
                }
            }
            return formattedTitle;
        }

        public static void PutImagePaths(PublishmentSystemInfo publishmentSystemInfo, BackgroundContentInfo contentInfo, NameValueCollection collection)
        {
            if (contentInfo != null)
            {
                //如果是图片模型
                NodeInfo nodeInfo = DataProvider.NodeDAO.GetNodeInfo(contentInfo.NodeID);
                if (EContentModelTypeUtils.IsPhoto(nodeInfo.ContentModelID))
                {
                    List<PhotoInfo> photoInfoList = DataProvider.PhotoDAO.GetPhotoInfoList(publishmentSystemInfo.PublishmentSystemID, contentInfo.ID);
                    foreach (PhotoInfo photoInfo in photoInfoList)
                    {
                        collection[photoInfo.SmallUrl] = PathUtility.MapPath(publishmentSystemInfo, photoInfo.SmallUrl);
                        collection[photoInfo.MiddleUrl] = PathUtility.MapPath(publishmentSystemInfo, photoInfo.MiddleUrl);
                        collection[photoInfo.LargeUrl] = PathUtility.MapPath(publishmentSystemInfo, photoInfo.LargeUrl);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(contentInfo.ImageUrl) && PageUtility.IsVirtualUrl(contentInfo.ImageUrl))
                    {
                        collection[contentInfo.ImageUrl] = PathUtility.MapPath(publishmentSystemInfo, contentInfo.ImageUrl);
                    }
                    if (!string.IsNullOrEmpty(contentInfo.VideoUrl) && PageUtility.IsVirtualUrl(contentInfo.VideoUrl))
                    {
                        collection[contentInfo.VideoUrl] = PathUtility.MapPath(publishmentSystemInfo, contentInfo.VideoUrl);
                    }
                    if (!string.IsNullOrEmpty(contentInfo.FileUrl) && PageUtility.IsVirtualUrl(contentInfo.FileUrl))
                    {
                        collection[contentInfo.FileUrl] = PathUtility.MapPath(publishmentSystemInfo, contentInfo.FileUrl);
                    }
                }
                ArrayList srcArrayList = RegexUtils.GetOriginalImageSrcs(contentInfo.Content);
                foreach (string src in srcArrayList)
                {
                    if (PageUtility.IsVirtualUrl(src))
                    {
                        collection[src] = PathUtility.MapPath(publishmentSystemInfo, src);
                    }
                }
            }
        }

        public static string GetAutoPageContent(string content, int pageWordNum)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(content))
            {
                content = content.Replace(ContentUtility.PagePlaceHolder, string.Empty);
                AutoPage(builder, content, pageWordNum);
            }
            return builder.ToString();
        }

        private static void AutoPage(StringBuilder builder, string content, int pageWordNum)
        {
            if (content.Length > pageWordNum)
            {
                int i = content.IndexOf("</P>", pageWordNum);
                if (i == -1)
                {
                    i = content.IndexOf("</p>", pageWordNum);
                }

                if (i != -1)
                {
                    int start = i + 4;
                    builder.Append(content.Substring(0, start));
                    content = content.Substring(start);
                    if (!string.IsNullOrEmpty(content))
                    {
                        builder.Append(ContentUtility.PagePlaceHolder);
                        AutoPage(builder, content, pageWordNum);
                    }
                }
                else
                {
                    builder.Append(content);
                }
            }
            else
            {
                builder.Append(content);
            }
        }

        public static int GetRealContentID(ETableStyle tableStyle, string tableName, int contentID)
        {
            string linkUrl = string.Empty;
            int referenceID = BaiRongDataProvider.ContentDAO.GetReferenceID(tableStyle, tableName, contentID, out linkUrl);
            if (referenceID > 0)
            {
                return referenceID;
            }
            return contentID;
        }

        public static ContentInfo GetContentInfo(ETableStyle tableStyle)
        {
            if (tableStyle == ETableStyle.BackgroundContent)
            {
                return new BackgroundContentInfo();
            }
            else if (tableStyle == ETableStyle.GoodsContent)
            {
                return new GoodsContentInfo();
            }
            else if (tableStyle == ETableStyle.BrandContent)
            {
                return new BrandContentInfo();
            }
            else if (tableStyle == ETableStyle.GovPublicContent)
            {
                return new GovPublicContentInfo();
            }
            else if (tableStyle == ETableStyle.GovInteractContent)
            {
                return new GovInteractContentInfo();
            }
            else if (tableStyle == ETableStyle.VoteContent)
            {
                return new VoteContentInfo();
            }
            else if (tableStyle == ETableStyle.JobContent)
            {
                return new JobContentInfo();
            }
            return new ContentInfo();
        }

        public static int GetColumnWidth(ETableStyle tableStyle, string attributeName)
        {
            int width = 80;
            if (StringUtils.EqualsIgnoreCase(attributeName, ContentAttribute.Hits) || StringUtils.EqualsIgnoreCase(attributeName, ContentAttribute.HitsByDay) || StringUtils.EqualsIgnoreCase(attributeName, ContentAttribute.HitsByMonth) || StringUtils.EqualsIgnoreCase(attributeName, ContentAttribute.HitsByWeek))
            {
                width = 50;
            }
            else if (StringUtils.EqualsIgnoreCase(attributeName, ContentAttribute.AddUserName) || StringUtils.EqualsIgnoreCase(attributeName, ContentAttribute.LastEditUserName) || StringUtils.EqualsIgnoreCase(attributeName, ContentAttribute.Check_UserName))
            {
                width = 60;
            }
            else if (StringUtils.EqualsIgnoreCase(attributeName, ContentAttribute.AddDate) || StringUtils.EqualsIgnoreCase(attributeName, ContentAttribute.LastEditDate))
            {
                width = 70;
            }
            else if (StringUtils.EqualsIgnoreCase(attributeName, ContentAttribute.LastHitsDate) || StringUtils.EqualsIgnoreCase(attributeName, ContentAttribute.Check_CheckDate))
            {
                width = 110;
            }
            else if (StringUtils.EqualsIgnoreCase(attributeName, BackgroundContentAttribute.Digg) || StringUtils.EqualsIgnoreCase(attributeName, ContentAttribute.Check_Reasons))
            {
                width = 110;
            }
            return width;
        }

        private static string GetColumnValue(Hashtable valueHashtable, ETableStyle tableStyle, PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo, TableStyleInfo styleInfo)
        {
            string value = string.Empty;
            if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.AddUserName))
            {
                if (!string.IsNullOrEmpty(contentInfo.AddUserName))
                {
                    string key = ContentAttribute.AddUserName + ":" + contentInfo.AddUserName;
                    value = valueHashtable[key] as string;
                    if (value == null)
                    {
                        value = string.Format(@"<a rel=""popover"" class=""popover-hover"" data-content=""{0}"" data-original-title=""管理员"">{1}</a>", AdminManager.GetFullName(contentInfo.AddUserName), AdminManager.GetDisplayName(contentInfo.AddUserName, false));
                        valueHashtable[key] = value;
                    }
                }
            }
            else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.LastEditUserName))
            {
                if (!string.IsNullOrEmpty(contentInfo.LastEditUserName))
                {
                    string key = ContentAttribute.LastEditUserName + ":" + contentInfo.LastEditUserName;
                    value = valueHashtable[key] as string;
                    if (value == null)
                    {
                        value = string.Format(@"<a rel=""popover"" class=""popover-hover"" data-content=""{0}"" data-original-title=""管理员"">{1}</a>", AdminManager.GetFullName(contentInfo.LastEditUserName), AdminManager.GetDisplayName(contentInfo.LastEditUserName, false));
                        valueHashtable[key] = value;
                    }
                }
            }
            else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.Check_UserName))
            {
                string checkUserName = contentInfo.GetExtendedAttribute(ContentAttribute.Check_UserName);
                if (!string.IsNullOrEmpty(checkUserName))
                {
                    string key = ContentAttribute.Check_UserName + ":" + checkUserName;
                    value = valueHashtable[key] as string;
                    if (value == null)
                    {
                        value = string.Format(@"<a rel=""popover"" class=""popover-hover"" data-content=""{0}"" data-original-title=""管理员"">{1}</a>", AdminManager.GetFullName(checkUserName), AdminManager.GetDisplayName(checkUserName, false));

                        valueHashtable[key] = value;
                    }
                }
            }
            else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.Check_CheckDate))
            {
                string checkDate = contentInfo.GetExtendedAttribute(ContentAttribute.Check_CheckDate);
                if (!string.IsNullOrEmpty(checkDate))
                {
                    value = DateUtils.GetDateAndTimeString(TranslateUtils.ToDateTime(checkDate));
                }
            }
            else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.Check_Reasons))
            {
                value = contentInfo.GetExtendedAttribute(ContentAttribute.Check_Reasons);
            }
            else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.AddDate))
            {
                value = DateUtils.GetDateString(contentInfo.AddDate);
            }
            else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.LastEditDate))
            {
                value = DateUtils.GetDateString(contentInfo.LastEditDate);
            }
            else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.SourceID))
            {
                value = SourceManager.GetSourceName(contentInfo.SourceID);
            }
            else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.Tags))
            {
                value = contentInfo.Tags;
            }
            else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.ContentGroupNameCollection))
            {
                value = contentInfo.ContentGroupNameCollection;
            }
            else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.Hits))
            {
                value = contentInfo.Hits.ToString();
            }
            else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.HitsByDay))
            {
                value = contentInfo.HitsByDay.ToString();
            }
            else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.HitsByWeek))
            {
                value = contentInfo.HitsByWeek.ToString();
            }
            else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.HitsByMonth))
            {
                value = contentInfo.HitsByMonth.ToString();
            }
            else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.LastHitsDate))
            {
                value = DateUtils.GetDateAndTimeString(contentInfo.LastHitsDate);
            }
            else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.IsTop))
            {
                value = StringUtils.GetTrueImageHtml(contentInfo.IsTop);
            }
            else
            {
                bool isSettting = false;
                if (tableStyle == ETableStyle.BackgroundContent)
                {
                    isSettting = true;
                    if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, BackgroundContentAttribute.Star))
                    {
                        string showPopWinString = SiteServer.CMS.BackgroundPages.Modal.ContentStarSet.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID);
                        value = string.Format(@"<a href=""javascript:;"" onclick=""{0}"" title=""点击设置评分"">{1}</a>", showPopWinString, StarsManager.GetStarString(publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID));
                    }
                    else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, BackgroundContentAttribute.Digg))
                    {
                        string showPopWinString = SiteServer.CMS.BackgroundPages.Modal.ContentDiggSet.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID);
                        int[] nums = BaiRongDataProvider.DiggDAO.GetCount(publishmentSystemInfo.PublishmentSystemID, contentInfo.ID);
                        string display = string.Format("赞同：{0} 不赞同：{1}", nums[0], nums[1]);
                        value = string.Format(@"<a href=""javascript:;"" onclick=""{0}"" title=""点击设置Digg数"">{1}</a>", showPopWinString, display);
                    }
                    else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, BackgroundContentAttribute.IsColor) || StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, BackgroundContentAttribute.IsHot) || StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, BackgroundContentAttribute.IsRecommend))
                    {
                        value = StringUtils.GetTrueImageHtml(contentInfo.GetExtendedAttribute(styleInfo.AttributeName));
                    }
                    else
                    {
                        isSettting = false;
                    }
                }

                if (!isSettting)
                {
                    value = InputTypeParser.GetContentByTableStyle(contentInfo.GetExtendedAttribute(styleInfo.AttributeName), publishmentSystemInfo, tableStyle, styleInfo);
                }
            }
            return value;
        }

        public static ArrayList GetAllTableStyleInfoArrayList(PublishmentSystemInfo publishmentSystemInfo, ETableStyle tableStyle, ArrayList tableStyleInfoArrayList)
        {
            ArrayList arraylist = new ArrayList();

            TableStyleInfo styleInfo = new TableStyleInfo();
            styleInfo.AttributeName = ContentAttribute.ID;
            styleInfo.DisplayName = "编号";
            arraylist.Add(styleInfo);

            arraylist.AddRange(tableStyleInfoArrayList);

            styleInfo = new TableStyleInfo();
            styleInfo.AttributeName = ContentAttribute.Hits;
            styleInfo.DisplayName = "点击量";
            arraylist.Add(styleInfo);

            if (tableStyle == ETableStyle.BackgroundContent)
            {
                if (publishmentSystemInfo.Additional.IsRelatedByTags)
                {
                    styleInfo = new TableStyleInfo();
                    styleInfo.AttributeName = ContentAttribute.Tags;
                    styleInfo.DisplayName = "标签";
                    arraylist.Add(styleInfo);
                }

                styleInfo = new TableStyleInfo();
                styleInfo.AttributeName = BackgroundContentAttribute.Star;
                styleInfo.DisplayName = "评分";
                arraylist.Add(styleInfo);
            }
            else
            {
                styleInfo = new TableStyleInfo();
                styleInfo.AttributeName = ContentAttribute.AddDate;
                styleInfo.DisplayName = "添加时间";
                arraylist.Add(styleInfo);
            }

            return arraylist;
        }

        public static ArrayList GetColumnTableStyleInfoArrayList(PublishmentSystemInfo publishmentSystemInfo, ETableStyle tableStyle, ArrayList tableStyleInfoArrayList)
        {
            ArrayList arraylist = new ArrayList();

            if (tableStyleInfoArrayList != null)
            {
                foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
                {
                    arraylist.Add(tableStyleInfo);
                }
            }

            TableStyleInfo styleInfo = null;

            styleInfo = new TableStyleInfo(0, 0, string.Empty, ContentAttribute.AddUserName, 0, "添加者", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
            arraylist.Add(styleInfo);

            styleInfo = new TableStyleInfo(0, 0, string.Empty, ContentAttribute.LastEditUserName, 0, "修改者", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
            arraylist.Add(styleInfo);

            styleInfo = new TableStyleInfo(0, 0, string.Empty, ContentAttribute.LastEditDate, 0, "修改时间", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
            arraylist.Add(styleInfo);

            styleInfo = new TableStyleInfo(0, 0, string.Empty, ContentAttribute.Check_UserName, 0, "审核者", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
            arraylist.Add(styleInfo);

            styleInfo = new TableStyleInfo(0, 0, string.Empty, ContentAttribute.Check_CheckDate, 0, "审核时间", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
            arraylist.Add(styleInfo);

            styleInfo = new TableStyleInfo(0, 0, string.Empty, ContentAttribute.Check_Reasons, 0, "审核原因", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
            arraylist.Add(styleInfo);

            styleInfo = new TableStyleInfo(0, 0, string.Empty, ContentAttribute.SourceID, 0, "来源标识", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
            arraylist.Add(styleInfo);

            if (publishmentSystemInfo.Additional.IsRelatedByTags)
            {
                styleInfo = new TableStyleInfo(0, 0, string.Empty, ContentAttribute.Tags, 0, "标签", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
                arraylist.Add(styleInfo);
            }

            styleInfo = new TableStyleInfo(0, 0, string.Empty, ContentAttribute.ContentGroupNameCollection, 0, "所属内容组", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
            arraylist.Add(styleInfo);

            styleInfo = new TableStyleInfo(0, 0, string.Empty, ContentAttribute.Hits, 0, "点击量", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
            arraylist.Add(styleInfo);

            styleInfo = new TableStyleInfo(0, 0, string.Empty, ContentAttribute.HitsByDay, 0, "日点击", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
            arraylist.Add(styleInfo);

            styleInfo = new TableStyleInfo(0, 0, string.Empty, ContentAttribute.HitsByWeek, 0, "周点击", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
            arraylist.Add(styleInfo);

            styleInfo = new TableStyleInfo(0, 0, string.Empty, ContentAttribute.HitsByMonth, 0, "月点击", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
            arraylist.Add(styleInfo);

            styleInfo = new TableStyleInfo(0, 0, string.Empty, ContentAttribute.LastHitsDate, 0, "最后点击时间", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
            arraylist.Add(styleInfo);

            if (tableStyle == ETableStyle.BackgroundContent)
            {
                styleInfo = new TableStyleInfo(0, 0, string.Empty, BackgroundContentAttribute.Star, 0, "评分", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
                arraylist.Add(styleInfo);

                styleInfo = new TableStyleInfo(0, 0, string.Empty, BackgroundContentAttribute.Digg, 0, "Digg", string.Empty, true, true, false, EInputType.Text, string.Empty, false, string.Empty);
                arraylist.Add(styleInfo);
            }

            return arraylist;
        }

        public static string GetColumnHeadRowsHtml(ArrayList tableStyleInfoArrayList, StringCollection attributesOfDisplay, ETableStyle tableStyle, PublishmentSystemInfo publishmentSystemInfo)
        {
            StringBuilder builder = new StringBuilder();

            ArrayList arrayList = GetColumnTableStyleInfoArrayList(publishmentSystemInfo, tableStyle, tableStyleInfoArrayList);
            foreach (TableStyleInfo styleInfo in arrayList)
            {
                if (attributesOfDisplay.Contains(styleInfo.AttributeName))
                {
                    builder.AppendFormat(@"<td width=""{0}"">{1}</td>", ContentUtility.GetColumnWidth(tableStyle, styleInfo.AttributeName), styleInfo.DisplayName);
                }
            }

            return builder.ToString();
        }

        public static string GetCommandHeadRowsHtml(ETableStyle tableStyle, PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo)
        {
            StringBuilder builder = new StringBuilder();

            EContentModelType modelType = EContentModelTypeUtils.GetEnumType(nodeInfo.ContentModelID);

            if (modelType == EContentModelType.Photo)
            {
                builder.Append(@"<td class=""center"" width=""50"">&nbsp;</td>");
            }
            else if (modelType == EContentModelType.Teleplay)
            {
                builder.Append(@"<td class=""center"" width=""50"">&nbsp;</td>");
            }
            else if (modelType == EContentModelType.Goods)
            {
                builder.Append(@"<td class=""center"" width=""70"">&nbsp;</td>");
                builder.Append(@"<td class=""center"" width=""70"">&nbsp;</td>");
            }
            else if (modelType == EContentModelType.Job)
            {
                builder.Append(@"<td class=""center"" width=""50"">&nbsp;</td>");
            }

            if (publishmentSystemInfo.Additional.IsContentCommentable && modelType != EContentModelType.Job)
            {
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.CommentCheck, AppManager.CMS.Permission.Channel.CommentDelete))
                {
                    builder.Append(@"<td class=""center"" width=""50"">&nbsp;</td>");
                }
            }
            return builder.ToString();
        }

        
        public static string GetFunctionHeadRowsHtml(ETableStyle tableStyle, PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo)
        {
            StringBuilder builder = new StringBuilder();

            EContentModelType modelType = EContentModelTypeUtils.GetEnumType(nodeInfo.ContentModelID);
            #region by 20160225 sofuny 增加评价功能
            if (nodeInfo.Additional.IsUseEvaluation)
            {
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.WebSite.Evaluation))
                {
                    builder.Append(@"<td class=""center"" width=""50"">&nbsp;</td>");
                }
            }
            if (nodeInfo.Additional.IsUseTrial)
            {
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.WebSite.Trial))
                {
                    builder.Append(@"<td class=""center"" width=""100"">&nbsp;</td><td class=""center"" width=""100"">&nbsp;</td>");
                }
            }
            if (nodeInfo.Additional.IsUseSurvey)
            {
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.WebSite.Survey))
                {
                    builder.Append(@"<td class=""center"" width=""80"">&nbsp;</td>");
                }
            }
            if (nodeInfo.Additional.IsUseCompare)
            {
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.WebSite.Compare))
                {
                    builder.Append(@"<td class=""center"" width=""80"">&nbsp;</td>");
                }
            }
            #endregion

            return builder.ToString();
        }


        public static string GetColumnItemRowsHtml(ArrayList tableStyleInfoArrayList, StringCollection attributesOfDisplay, Hashtable valueHashtable, ETableStyle tableStyle, PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo)
        {
            StringBuilder builder = new StringBuilder();

            ArrayList arrayList = GetColumnTableStyleInfoArrayList(publishmentSystemInfo, tableStyle, tableStyleInfoArrayList);
            foreach (TableStyleInfo styleInfo in arrayList)
            {
                if (attributesOfDisplay.Contains(styleInfo.AttributeName))
                {
                    string value = GetColumnValue(valueHashtable, tableStyle, publishmentSystemInfo, contentInfo, styleInfo);
                    builder.AppendFormat(@"<td width=""{0}"">{1}</td>", ContentUtility.GetColumnWidth(tableStyle, styleInfo.AttributeName), value);
                }
            }

            return builder.ToString();
        }

        public static string GetCommandItemRowsHtml(ETableStyle tableStyle, PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, ContentInfo contentInfo, string pageUrl)
        {
            StringBuilder builder = new StringBuilder();

            EContentModelType modelType = EContentModelTypeUtils.GetEnumType(nodeInfo.ContentModelID);

            if (modelType == EContentModelType.Photo)
            {
                string contentPhotoUploadUrl = BackgroundContentPhotoUpload.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, pageUrl);
                builder.AppendFormat(@"<td class=""center"" width=""50""><a href=""{0}"">图片</a><span style=""color:gray"">({1})</span></td>", contentPhotoUploadUrl, contentInfo.Photos);
            }
            else if (modelType == EContentModelType.Teleplay)
            {
                string contentTeleplayUploadUrl = BackgroundContentTeleplayUpload.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, pageUrl);
                builder.AppendFormat(@"<td class=""center"" width=""50""><a href=""{0}"">剧集</a><span style=""color:gray"">({1})</span></td>", contentTeleplayUploadUrl, contentInfo.Teleplays);
            }
            else if (modelType == EContentModelType.Goods)
            {
                int skuCount = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(GoodsContentAttribute.SKUCount));
                string specContentUrl = PageUtils.GetB2CUrl(string.Format("background_specContent.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&SN={3}&ReturnUrl={4}", publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, contentInfo.GetExtendedAttribute(GoodsContentAttribute.SN), StringUtils.ValueToUrl(pageUrl)));
                builder.AppendFormat(@"<td class=""center""><a href=""{0}"">货品设置({1})</a></td>", specContentUrl, skuCount);

                string contentPhotoUploadUrl = BackgroundContentPhotoUpload.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, pageUrl);
                builder.AppendFormat(@"<td class=""center""><a href=""{0}"">商品相册({1})</a></td>", contentPhotoUploadUrl, contentInfo.Photos);
            }
            else if (modelType == EContentModelType.Job)
            {
                int resumeNum = DataProvider.ResumeContentDAO.GetCount(publishmentSystemInfo.PublishmentSystemID, contentInfo.ID);
                string urlResume = PageUtils.GetCMSUrl(string.Format("background_resumeContent.aspx?PublishmentSystemID={0}&JobContentID={1}&ReturnUrl={2}", publishmentSystemInfo.PublishmentSystemID, contentInfo.ID, StringUtils.ValueToUrl(pageUrl)));
                builder.AppendFormat(@"<td class=""center"" width=""50""><a href=""{0}"">简历</a><span style=""color:gray"">({1})</span></td>", urlResume, resumeNum);
            }

            if (publishmentSystemInfo.Additional.IsContentCommentable && modelType != EContentModelType.Job)
            {
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, AppManager.CMS.Permission.Channel.CommentCheck, AppManager.CMS.Permission.Channel.CommentDelete))
                {
                    string urlComment = PageUtils.GetCMSUrl(string.Format("background_comment.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&ReturnUrl={3}", publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, StringUtils.ValueToUrl(pageUrl)));
                    builder.AppendFormat(@"<td class=""center"" width=""50""><a href=""{0}"">评论</a><span style=""color:gray"">({1})</span></td>", urlComment, contentInfo.Comments);
                }
            }

            return builder.ToString();
        }


        public static string GetFunctionItemRowsHtml(ETableStyle tableStyle, PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, ContentInfo contentInfo, string pageUrl)
        {

            StringBuilder builder = new StringBuilder();

            EContentModelType modelType = EContentModelTypeUtils.GetEnumType(nodeInfo.ContentModelID);

            #region by 20160225 sofuny 增加评价功能
            if (nodeInfo.Additional.IsUseEvaluation)
            {
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.WebSite.Evaluation))
                {
                    string urlComment = SelectFunctionColumns.GetOpenWindowStringToEvaluation(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, contentInfo.ID, true);
                    builder.AppendFormat(@"<td class=""center"" width=""50""><a href=""javascript:;"" onclick=""{0}"">评价字段</a><span style=""color:gray""></span>", urlComment);

                    urlComment = PageUtils.GetCMSUrl(string.Format("background_evaluation.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&ReturnUrl={3}", publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, StringUtils.ValueToUrl(pageUrl), ETableStyle.EvaluationContent, publishmentSystemInfo.PublishmentSystemID));
                    builder.AppendFormat(@"<br/><a href=""{0}"">评价内容</a><span style=""color:gray""></span></td>", urlComment);
                }
            }
            #endregion
            #region by 20160303 sofuny 增加试用功能
            if (nodeInfo.Additional.IsUseTrial)
            {
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.WebSite.Trial))
                {
                    string urlComment = SelectFunctionColumns.GetOpenWindowStringToTrialApply(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, contentInfo.ID, true);
                    builder.AppendFormat(@"<td class=""center"" width=""50""><a href=""javascript:;"" onclick=""{0}"">试用申请字段</a><span style=""color:gray""></span>", urlComment);

                    urlComment = PageUtils.GetCMSUrl(string.Format("background_trialApply.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&ReturnUrl={3}", publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, StringUtils.ValueToUrl(pageUrl), ETableStyle.EvaluationContent, publishmentSystemInfo.PublishmentSystemID));
                    builder.AppendFormat(@"<br/><a href=""{0}"">试用申请记录</a><span style=""color:gray""></span></td>", urlComment);

                    urlComment = SelectFunctionColumns.GetOpenWindowStringToTrialReport(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, contentInfo.ID, true);
                    builder.AppendFormat(@"<td class=""center"" width=""50""><a href=""javascript:;"" onclick=""{0}"">试用报告字段</a><span style=""color:gray""></span>", urlComment);

                    urlComment = PageUtils.GetCMSUrl(string.Format("background_trialReport.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&ReturnUrl={3}", publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, StringUtils.ValueToUrl(pageUrl), ETableStyle.EvaluationContent, publishmentSystemInfo.PublishmentSystemID));
                    builder.AppendFormat(@"<br/><a href=""{0}"">试用报告内容</a><span style=""color:gray""></span></td>", urlComment);
                }
            }
            #endregion
            #region by 201600309 sofuny 增加调查问卷功能
            if (nodeInfo.Additional.IsUseSurvey)
            {
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.WebSite.Survey))
                {
                    string urlComment = SelectFunctionColumns.GetOpenWindowStringToSurvey(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, contentInfo.ID, true);
                    builder.AppendFormat(@"<td class=""center"" width=""80""><a href=""javascript:;"" onclick=""{0}"">调查问卷字段</a><span style=""color:gray""></span>", urlComment);

                    urlComment = PageUtils.GetCMSUrl(string.Format("background_surveyQuestionnaire.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&ReturnUrl={3}", publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, StringUtils.ValueToUrl(pageUrl), ETableStyle.SurveyContent, publishmentSystemInfo.PublishmentSystemID));
                    builder.AppendFormat(@"<br/><a href=""{0}"">调查问卷</a><span style=""color:gray""></span></td>", urlComment);
                }
            }
            #endregion
            #region by 20160316 sofuny 增加比较反馈功能
            if (nodeInfo.Additional.IsUseCompare)
            {
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.WebSite.Compare))
                {
                    string urlComment = SelectFunctionColumns.GetOpenWindowStringToCompare(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, contentInfo.ID, true);
                    builder.AppendFormat(@"<td class=""center"" width=""80""><a href=""javascript:;"" onclick=""{0}"">比较反馈字段</a><span style=""color:gray""></span>", urlComment);

                    urlComment = PageUtils.GetCMSUrl(string.Format("background_compare.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&ReturnUrl={3}", publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, StringUtils.ValueToUrl(pageUrl), ETableStyle.CompareContent, publishmentSystemInfo.PublishmentSystemID));
                    builder.AppendFormat(@"<br/><a href=""{0}"">比较反馈</a><span style=""color:gray""></span></td>", urlComment);
                }
            }
            #endregion
            return builder.ToString();
        }

        public static void Translate(PublishmentSystemInfo publishmentSystemInfo, string idsCollection, string translateCollection, ETranslateContentType translateType)
        {
            if (!string.IsNullOrEmpty(idsCollection) && !string.IsNullOrEmpty(translateCollection))
            {
                ArrayList translateArrayList = TranslateUtils.StringCollectionToArrayList(translateCollection);
                foreach (string translate in translateArrayList)
                {
                    if (!string.IsNullOrEmpty(translate))
                    {
                        string[] translates = translate.Split('_');
                        if (translates.Length == 2)
                        {
                            int targetPublishmentSystemID = TranslateUtils.ToInt(translates[0]);
                            int targetNodeID = TranslateUtils.ToInt(translates[1]);

                            if (targetPublishmentSystemID > 0 && targetNodeID > 0)
                            {
                                PublishmentSystemInfo targetPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID);

                                string targetTableName = NodeManager.GetTableName(targetPublishmentSystemInfo, targetNodeID);

                                ArrayList idsArrayList = TranslateUtils.StringCollectionToArrayList(idsCollection);
                                foreach (string ids in idsArrayList)
                                {
                                    string[] nodeIDWithContentID = ids.Split('_');
                                    if (nodeIDWithContentID.Length == 2)
                                    {
                                        int nodeID = TranslateUtils.ToInt(nodeIDWithContentID[0]);
                                        int contentID = TranslateUtils.ToInt(nodeIDWithContentID[1]);

                                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                                        ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                                        if (contentInfo != null)
                                        {
                                            if (translateType == ETranslateContentType.Copy)
                                            {
                                                FileUtility.MoveFileByContentInfo(publishmentSystemInfo, targetPublishmentSystemInfo, contentInfo);

                                                contentInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                contentInfo.SourceID = contentInfo.NodeID;
                                                contentInfo.NodeID = targetNodeID;
                                                contentInfo.Attributes[ContentAttribute.TranslateContentType] = ETranslateContentType.Copy.ToString();
                                                //contentInfo.Attributes.Add(ContentAttribute.TranslateContentType, ETranslateContentType.Copy.ToString());
                                                int theContentID = DataProvider.ContentDAO.Insert(targetTableName, targetPublishmentSystemInfo, contentInfo);
                                                if (EContentModelTypeUtils.IsPhoto(nodeInfo.ContentModelID))
                                                {
                                                    List<PhotoInfo> photoInfoList = DataProvider.PhotoDAO.GetPhotoInfoList(publishmentSystemInfo.PublishmentSystemID, contentID);
                                                    if (photoInfoList.Count > 0)
                                                    {
                                                        foreach (PhotoInfo photoInfo in photoInfoList)
                                                        {
                                                            photoInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                            photoInfo.ContentID = theContentID;

                                                            FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.SmallUrl);
                                                            FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.MiddleUrl);
                                                            FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.LargeUrl);

                                                            DataProvider.PhotoDAO.Insert(photoInfo);
                                                        }
                                                    }
                                                }
                                                if (contentInfo.IsChecked)
                                                {
                                                    string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateContent(targetPublishmentSystemID, contentInfo.NodeID, theContentID);
                                                    AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                                                }
                                            }
                                            else if (translateType == ETranslateContentType.Cut)
                                            {
                                                FileUtility.MoveFileByContentInfo(publishmentSystemInfo, targetPublishmentSystemInfo, contentInfo);
                                                contentInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                contentInfo.SourceID = contentInfo.NodeID;
                                                contentInfo.NodeID = targetNodeID;
                                                contentInfo.Attributes[ContentAttribute.TranslateContentType] = ETranslateContentType.Cut.ToString();
                                                //contentInfo.Attributes.Add(ContentAttribute.TranslateContentType, ETranslateContentType.Cut.ToString());
                                                if (StringUtils.EqualsIgnoreCase(tableName, targetTableName))
                                                {
                                                    contentInfo.Taxis = DataProvider.ContentDAO.GetTaxisToInsert(targetTableName, targetNodeID, contentInfo.IsTop);
                                                    DataProvider.ContentDAO.Update(targetTableName, targetPublishmentSystemInfo, contentInfo);
                                                }
                                                else
                                                {
                                                    DataProvider.ContentDAO.Insert(targetTableName, targetPublishmentSystemInfo, contentInfo);
                                                    DataProvider.ContentDAO.DeleteContents(publishmentSystemInfo.PublishmentSystemID, tableName, TranslateUtils.ToArrayList(contentID), nodeID);
                                                }

                                                DataProvider.NodeDAO.UpdateContentNum(publishmentSystemInfo, nodeID, true);
                                                DataProvider.NodeDAO.UpdateContentNum(targetPublishmentSystemInfo, targetNodeID, true);

                                                if (EContentModelTypeUtils.IsPhoto(nodeInfo.ContentModelID))
                                                {
                                                    List<PhotoInfo> photoInfoList = DataProvider.PhotoDAO.GetPhotoInfoList(publishmentSystemInfo.PublishmentSystemID, contentID);
                                                    if (photoInfoList.Count > 0)
                                                    {
                                                        foreach (PhotoInfo photoInfo in photoInfoList)
                                                        {
                                                            photoInfo.PublishmentSystemID = targetPublishmentSystemID;

                                                            FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.SmallUrl);
                                                            FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.MiddleUrl);
                                                            FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.LargeUrl);

                                                            DataProvider.PhotoDAO.Update(photoInfo);
                                                        }
                                                    }
                                                }
                                                if (contentInfo.IsChecked)
                                                {
                                                    string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateContent(targetPublishmentSystemID, contentInfo.NodeID, contentInfo.ID);
                                                    AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                                                }
                                            }
                                            else if (translateType == ETranslateContentType.Reference)
                                            {
                                                if (contentInfo.ReferenceID == 0)
                                                {
                                                    contentInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                    contentInfo.SourceID = contentInfo.NodeID;
                                                    contentInfo.NodeID = targetNodeID;
                                                    contentInfo.ReferenceID = contentID;
                                                    contentInfo.Attributes[ContentAttribute.TranslateContentType] = ETranslateContentType.Reference.ToString();
                                                    //contentInfo.Attributes.Add(ContentAttribute.TranslateContentType, ETranslateContentType.Reference.ToString());
                                                    DataProvider.ContentDAO.Insert(targetTableName, targetPublishmentSystemInfo, contentInfo);
                                                }
                                            }
                                            else if (translateType == ETranslateContentType.ReferenceContent)
                                            {
                                                if (contentInfo.ReferenceID == 0)
                                                {
                                                    contentInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                    contentInfo.SourceID = contentInfo.NodeID;
                                                    contentInfo.NodeID = targetNodeID;
                                                    contentInfo.ReferenceID = contentID;
                                                    contentInfo.Attributes[ContentAttribute.TranslateContentType] = ETranslateContentType.ReferenceContent.ToString();
                                                    //contentInfo.Attributes.Add(ContentAttribute.TranslateContentType, ETranslateContentType.ReferenceContent.ToString());
                                                    int theContentID = DataProvider.ContentDAO.Insert(targetTableName, targetPublishmentSystemInfo, contentInfo);
                                                    if (EContentModelTypeUtils.IsPhoto(nodeInfo.ContentModelID))
                                                    {
                                                        List<PhotoInfo> photoInfoList = DataProvider.PhotoDAO.GetPhotoInfoList(publishmentSystemInfo.PublishmentSystemID, contentID);
                                                        if (photoInfoList.Count > 0)
                                                        {
                                                            foreach (PhotoInfo photoInfo in photoInfoList)
                                                            {
                                                                photoInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                                photoInfo.ContentID = theContentID;

                                                                FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.SmallUrl);
                                                                FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.MiddleUrl);
                                                                FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.LargeUrl);

                                                                DataProvider.PhotoDAO.Insert(photoInfo);
                                                            }
                                                        }
                                                    }
                                                    if (contentInfo.IsChecked)
                                                    {
                                                        string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateContent(targetPublishmentSystemID, contentInfo.NodeID, theContentID);
                                                        AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                StringUtility.AddLog(publishmentSystemInfo.PublishmentSystemID, "转移内容");
            }
        }

        public static Hashtable GetIDsHashtable(NameValueCollection queryString)
        {
            Hashtable idsHashtable = new Hashtable();

            if (!string.IsNullOrEmpty(queryString["IDsCollection"]))
            {
                ArrayList idsArrayList = TranslateUtils.StringCollectionToArrayList(queryString["IDsCollection"]);
                foreach (string ids in idsArrayList)
                {
                    int nodeID = TranslateUtils.ToInt(ids.Split('_')[0]);
                    int contentID = TranslateUtils.ToInt(ids.Split('_')[1]);
                    ArrayList contentIDArrayList = idsHashtable[nodeID] as ArrayList;
                    if (contentIDArrayList == null)
                    {
                        contentIDArrayList = new ArrayList();
                    }
                    contentIDArrayList.Add(contentID);
                    idsHashtable[nodeID] = contentIDArrayList;
                }
            }
            else
            {
                int nodeID = TranslateUtils.ToInt(queryString["NodeID"]);
                ArrayList contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(queryString["ContentIDCollection"]);
                idsHashtable[nodeID] = contentIDArrayList;
            }

            return idsHashtable;
        }
    }
}
