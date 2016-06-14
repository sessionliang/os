using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.BBS.Core.TemplateParser.Model;
using SiteServer.BBS.Model;
using System;
using System.Web.UI;

namespace SiteServer.BBS.Core.TemplateParser.Entity
{
    public class SqlEntities
    {
        private SqlEntities()
        {
        }

        public static string ItemIndex = "{bbs.sql.itemIndex}";//排序

        internal static string Parse(string stlEntity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;

            if (contextInfo.ItemContainer == null || contextInfo.ItemContainer.SqlItem == null) return string.Empty;

            try
            {
                string attributeName = stlEntity.Substring(9, stlEntity.Length - 10);
                if (StringUtils.StartsWithIgnoreCase(attributeName, ParserUtility.ItemIndex))
                {
                    parsedContent = ParserUtility.ParseItemIndex(contextInfo.ItemContainer.SqlItem.ItemIndex, attributeName, contextInfo).ToString();
                }
                else if (StringUtils.StartsWithIgnoreCase(attributeName, "linkUrl"))
                {
                    string linkUrl = TranslateUtils.EvalString(contextInfo.ItemContainer.SqlItem.DataItem, "LinkUrl");
                    if (PageUtils.IsProtocolUrl(linkUrl))
                    {
                        parsedContent = linkUrl;
                    }
                    else if (PageUtils.IsVirtualUrl(linkUrl))
                    {
                        parsedContent = PageUtils.ParseNavigationUrl(linkUrl);
                    }
                    else
                    {
                        parsedContent = PageUtilityBBS.GetBBSUrl(pageInfo.PublishmentSystemID, linkUrl);
                    }
                }
                else if (StringUtils.StartsWithIgnoreCase(attributeName, "linkStyle"))
                {
                    string formatString = TranslateUtils.EvalString(contextInfo.ItemContainer.SqlItem.DataItem, "FormatString");
                    parsedContent = StringUtilityBBS.GetHighlightStyle(formatString);
                }
                else if (StringUtils.StartsWithIgnoreCase(attributeName, "linkTarget"))
                {
                    bool isBlank = TranslateUtils.ToBool(TranslateUtils.EvalString(contextInfo.ItemContainer.SqlItem.DataItem, "IsBlank"));
                    if (isBlank)
                    {
                        parsedContent += @"_blank";
                    }
                }
                else
                {
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.SqlItem.DataItem, attributeName, "{0}");
                }
            }
            catch { }

            return parsedContent;
        }

        /// <summary>
        /// 仅用于在模板语言参考中显示
        /// </summary>
        static IDictionary GetSqlEntitiesDirectory()
        {
            ListDictionary dictionary = new ListDictionary();
            dictionary.Add(SqlEntities.ItemIndex, new string[] { "排序", "Sql/ItemIndex" });
            return dictionary;
        }
    }
}
