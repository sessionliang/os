using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections;
using System;
using SiteServer.BBS.Core.TemplateParser.Model;
using SiteServer.BBS.Model;
using SiteServer.BBS.Core.TemplateParser.Element;

namespace SiteServer.BBS.Core.TemplateParser.ListTemplate
{
    public class TemplateUtility
	{
		private TemplateUtility()
		{
		}

        public static string GetThreadsItemTemplateString(string templateString, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);
            ThreadInfo threadInfo = new ThreadInfo(itemContainer.ThreadItem.DataItem);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContextType = contextType;
            contextInfo.ItemContainer = itemContainer;
            contextInfo.ContainerClientID = containerClientID;
            contextInfo.ForumID = threadInfo.ForumID;
            contextInfo.ThreadID = threadInfo.ID;
            contextInfo.ThreadInfo = threadInfo;

            string theTemplateString = string.Empty;

            if (string.IsNullOrEmpty(theTemplateString))
            {
                theTemplateString = templateString;
            }

            StringBuilder innerBuilder = new StringBuilder(theTemplateString);
            ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopThreadItem(pageInfo);

            return innerBuilder.ToString();
        }

        public static string GetForumsItemTemplateString(string templateString, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);

            int forumID = TranslateUtils.EvalInt(itemContainer.ForumItem.DataItem, ForumAttribute.ForumID);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContextType = contextType;
            contextInfo.ItemContainer = itemContainer;
            contextInfo.ContainerClientID = containerClientID;
            contextInfo.ForumID = forumID;

            StringBuilder innerBuilder = new StringBuilder(templateString);
            ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopForumItem(pageInfo);

            return innerBuilder.ToString();
        }

        public static string GetPostsTemplateString(string templateString, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContextType = EContextType.Post;
            contextInfo.ItemContainer = itemContainer;
            contextInfo.ContainerClientID = containerClientID;

            StringBuilder innerBuilder = new StringBuilder(templateString);
            ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopPostItem(pageInfo);

            return innerBuilder.ToString();
        }

        public static string GetSqlContentsTemplateString(string templateString, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContextType = contextType;
            contextInfo.ContainerClientID = containerClientID;
            contextInfo.ItemContainer = itemContainer;

            StringBuilder innerBuilder = new StringBuilder(templateString);
            ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopSqlItem(pageInfo);

            return innerBuilder.ToString();
        }

        public static void PutContentsDisplayInfoToMyDataList(ParsedDataList MyDataList, ContentsDisplayInfo displayInfo)
		{
			MyDataList.RepeatColumns = displayInfo.Columns;
			MyDataList.RepeatDirection = displayInfo.Direction;
			MyDataList.Height = displayInfo.Height;
			MyDataList.Width = displayInfo.Width;
            if (!string.IsNullOrEmpty(displayInfo.Align))
            {
                MyDataList.HorizontalAlign = ParserUtility.ToHorizontalAlign(displayInfo.Align);
            }
			MyDataList.ItemStyle.Height = displayInfo.ItemHeight;
			MyDataList.ItemStyle.Width = displayInfo.ItemWidth;
            MyDataList.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            if (!string.IsNullOrEmpty(displayInfo.ItemAlign))
            {
                MyDataList.ItemStyle.HorizontalAlign = ParserUtility.ToHorizontalAlign(displayInfo.ItemAlign);
            }
            MyDataList.ItemStyle.VerticalAlign = VerticalAlign.Top;
            if (!string.IsNullOrEmpty(displayInfo.ItemVerticalAlign))
            {
                MyDataList.ItemStyle.VerticalAlign = ParserUtility.ToVerticalAlign(displayInfo.ItemVerticalAlign);
            }
            if (!string.IsNullOrEmpty(displayInfo.ItemClass))
            {
                MyDataList.ItemStyle.CssClass = displayInfo.ItemClass;
            }

            if (displayInfo.Layout == ELayout.Table)
			{
				MyDataList.RepeatLayout = RepeatLayout.Table;
			}
			else if(displayInfo.Layout == ELayout.Flow)
			{
				MyDataList.RepeatLayout = RepeatLayout.Flow;
			}
			
			foreach (string attributeName in displayInfo.OtherAttributes)
			{
				MyDataList.AddAttribute(attributeName, displayInfo.OtherAttributes[attributeName]);
			}
		}

        private static bool IsNumberInRange(int number, string range)
        {
            if (!string.IsNullOrEmpty(range))
            {
                if (range.IndexOf(',') != -1)
                {
                    string[] intArr = range.Split(',');
                    foreach (string intStr in intArr)
                    {
                        if (TranslateUtils.ToInt(intStr.Trim()) == number)
                        {
                            return true;
                        }
                    }
                }
                else if (range.IndexOf('_') != -1)
                {
                    int startVal = TranslateUtils.ToInt(range.Split('_')[0].Trim());
                    int endVal = TranslateUtils.ToInt(range.Split('_')[1].Trim());
                    if (number >= startVal && number <= endVal)
                    {
                        return true;
                    }
                }
                else if (range.IndexOf('-') != -1)
                {
                    int startVal = TranslateUtils.ToInt(range.Split('_')[0].Trim());
                    int endVal = TranslateUtils.ToInt(range.Split('_')[1].Trim());
                    if (number >= startVal && number <= endVal)
                    {
                        return true;
                    }
                }
                else if (TranslateUtils.ToInt(range.Trim()) == number)
                {
                    return true;
                }
            }
            return false;
        }
	}
}
