using System.Text.RegularExpressions;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.Model;

namespace SiteServer.STL.Parser
{
	/// <summary>
	/// StlPageElementParser 的摘要说明。
	/// </summary>
	public class StlPageElementParser
	{
		private StlPageElementParser()
		{
		}


		//在内容页中对“翻页项容器”（stl:pageItems）元素进行解析，此元素在生成页面时单独解析，不包含在ParseStlElement方法中。
		public static string ParseStlPageInContentPage(string stlElement, PageInfo pageInfo, int nodeID, int contentID, int currentPageIndex, int pageCount)
		{
            return StlPageItems.Parse(stlElement, pageInfo, nodeID, contentID, currentPageIndex, pageCount, pageCount, EContextType.Content);
		}

		//在栏目页中对“翻页项容器”（stl:pageItems）元素进行解析，此元素在生成页面时单独解析，不包含在ParseStlElement方法中。
		public static string ParseStlPageInChannelPage(string stlElement, PageInfo pageInfo, int nodeID, int currentPageIndex, int pageCount, int totalNum)
		{
            return StlPageItems.Parse(stlElement, pageInfo, nodeID, 0, currentPageIndex, pageCount, totalNum, EContextType.Channel);
		}

        public static string ParseStlPageInSearchPage(string stlElement, PageInfo pageInfo, string ajaxDivID, int nodeID, int currentPageIndex, int pageCount, int totalNum)
        {
            return StlPageItems.ParseInSearchPage(stlElement, pageInfo, ajaxDivID, nodeID, currentPageIndex, pageCount, totalNum);
        }

        public static string ParseStlPageInDynamicPage(string stlElement, PageInfo pageInfo, string pageUrl, int nodeID, int currentPageIndex, int pageCount, int totalNum, bool isPageRefresh, string ajaxDivID)
        {
            return StlPageItems.ParseInDynamicPage(stlElement, pageInfo, pageUrl, nodeID, currentPageIndex, pageCount, totalNum, isPageRefresh, ajaxDivID);
        }

		public static string ParseStlPageItems(string htmlInStlPageElement, PageInfo pageInfo, int nodeID, int contentID, int currentPageIndex, int pageCount, int totalNum, bool isXmlContent, EContextType contextType)
		{
			string html = htmlInStlPageElement;

            MatchCollection mc = StlParserUtility.GetStlEntityRegex("pageItem").Matches(html);
            for (int i = 0; i < mc.Count; i++)
            {
                string stlEntity = mc[i].Value;
                string pageHtml = StlPageItem.ParseEntity(stlEntity, pageInfo, nodeID, contentID, currentPageIndex, pageCount, totalNum, isXmlContent, contextType);
                html = html.Replace(stlEntity, pageHtml);
            }

            mc = StlParserUtility.REGEX_STL_ELEMENT.Matches(html);
			for (int i = 0; i < mc.Count; i++)
			{
				string stlElement = mc[i].Value;
                string pageHtml = StlPageItem.ParseElement(stlElement, pageInfo, nodeID, contentID, currentPageIndex, pageCount, totalNum, isXmlContent, contextType);
				html = html.Replace(stlElement, pageHtml);
			}
            
			return html;
		}

        public static string ParseStlPageItemsInSearchPage(string htmlInStlPageElement, PageInfo pageInfo, string ajaxDivID, int nodeID, int currentPageIndex, int pageCount, int totalNum)
        {
            string html = htmlInStlPageElement;

            MatchCollection mc = StlParserUtility.GetStlEntityRegex("pageItem").Matches(html);
            for (int i = 0; i < mc.Count; i++)
            {
                string stlEntity = mc[i].Value;
                string pageHtml = StlPageItem.ParseEntityInSearchPage(stlEntity, pageInfo, ajaxDivID, nodeID, currentPageIndex, pageCount, totalNum);
                html = html.Replace(stlEntity, pageHtml);
            }

            mc = StlParserUtility.REGEX_STL_ELEMENT.Matches(html);
            for (int i = 0; i < mc.Count; i++)
            {
                string stlElement = mc[i].Value;
                string pageHtml = StlPageItem.ParseElementInSearchPage(stlElement, pageInfo, ajaxDivID, nodeID, currentPageIndex, pageCount, totalNum);
                html = html.Replace(stlElement, pageHtml);
            }

            return html;
        }

        public static string ParseStlPageItemsInDynamicPage(string htmlInStlPageElement, PageInfo pageInfo, string pageUrl, int nodeID, int currentPageIndex, int pageCount, int totalNum, bool isPageRefresh, string ajaxDivID)
        {
            string html = htmlInStlPageElement;

            MatchCollection mc = StlParserUtility.GetStlEntityRegex("pageItem").Matches(html);
            for (int i = 0; i < mc.Count; i++)
            {
                string stlEntity = mc[i].Value;
                string pageHtml = StlPageItem.ParseEntityInDynamicPage(stlEntity, pageInfo, pageUrl, nodeID, currentPageIndex, pageCount, totalNum, isPageRefresh, ajaxDivID);
                html = html.Replace(stlEntity, pageHtml);
            }

            mc = StlParserUtility.REGEX_STL_ELEMENT.Matches(html);
            for (int i = 0; i < mc.Count; i++)
            {
                string stlElement = mc[i].Value;
                string pageHtml = StlPageItem.ParseElementInDynamicPage(stlElement, pageInfo, pageUrl, nodeID, currentPageIndex, pageCount, totalNum, isPageRefresh, ajaxDivID);
                html = html.Replace(stlElement, pageHtml);
            }

            return html;
        }

		
		//在内容页中对“翻页”（stl:pageItem）元素进行解析，此元素在生成页面时单独解析，不包含在ParseStlElement方法中。
		public static string ParseStlPageItemInContentPage(string stlElement, PageInfo pageInfo, int nodeID, int contentID, int currentPageIndex, int pageCount, int totalNum)
		{
			return StlPageItem.ParseElement(stlElement, pageInfo, nodeID, contentID, currentPageIndex, pageCount, totalNum, false, EContextType.Content);
		}

		//在栏目页中对“翻页”（stl:pageItem）元素进行解析，此元素在生成页面时单独解析，不包含在ParseStlElement方法中。
		public static string ParseStlPageItemInChannelPage(string stlElement, PageInfo pageInfo, int nodeID, int currentPageIndex, int pageCount, int totalNum)
		{
            return StlPageItem.ParseElement(stlElement, pageInfo, nodeID, 0, currentPageIndex, pageCount, totalNum, false, EContextType.Channel);
		}

        public static string ParseStlPageItemInSearchPage(string stlElement, PageInfo pageInfo, string ajaxDivID, int nodeID, int currentPageIndex, int pageCount, int totalNum)
        {
            return StlPageItem.ParseElementInSearchPage(stlElement, pageInfo, ajaxDivID, nodeID, currentPageIndex, pageCount, totalNum);
        }

        public static string ParseStlPageItemInDynamicPage(string stlElement, PageInfo pageInfo, string pageUrl, int nodeID, int currentPageIndex, int pageCount, int totalNum, bool isPageRefresh, string ajaxDivID)
        {
            return StlPageItem.ParseElementInDynamicPage(stlElement, pageInfo, pageUrl, nodeID, currentPageIndex, pageCount, totalNum, isPageRefresh, ajaxDivID);
        }
	}
}
