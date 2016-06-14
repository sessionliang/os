using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;

namespace SiteServer.STL.Parser.StlEntity
{
	public class StlSqlEntities
	{
        private StlSqlEntities()
		{
		}

        public const string EntityName = "Sql";              //数据库实体

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(StlParserUtility.ItemIndex, "排序");

                return attributes;
            }
        }

        internal static string Parse(string stlEntity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;

            if (contextInfo.ItemContainer == null || contextInfo.ItemContainer.SqlItem == null) return string.Empty;

            try
            {
                string attributeName = stlEntity.Substring(5, stlEntity.Length - 6);
                if (StringUtils.StartsWithIgnoreCase(attributeName, StlParserUtility.ItemIndex))
                {
                    parsedContent = StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.SqlItem.ItemIndex, attributeName, contextInfo).ToString();
                }
                else
                {
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.SqlItem.DataItem, attributeName, "{0}");
                }
            }
            catch { }

            return parsedContent;
        }
	}
}
