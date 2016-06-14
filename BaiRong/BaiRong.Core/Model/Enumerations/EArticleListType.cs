using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model
{
    public enum EArticleListType
    {
        Summary,			//摘要
        Content,			//全文
        TitleOnly,			//仅显示标题
    }

    public class EArticleListTypeUtils
    {
        public static string GetValue(EArticleListType type)
        {
            if (type == EArticleListType.Summary)
            {
                return "Summary";
            }
            else if (type == EArticleListType.Content)
            {
                return "Content";
            }
            else if (type == EArticleListType.TitleOnly)
            {
                return "TitleOnly";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EArticleListType type)
        {
            if (type == EArticleListType.Summary)
            {
                return "摘要";
            }
            else if (type == EArticleListType.Content)
            {
                return "全文";
            }
            else if (type == EArticleListType.TitleOnly)
            {
                return "仅显示标题";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EArticleListType GetEnumType(string typeStr)
        {
            EArticleListType retval = EArticleListType.Content;

            if (Equals(EArticleListType.Summary, typeStr))
            {
                retval = EArticleListType.Summary;
            }
            else if (Equals(EArticleListType.Content, typeStr))
            {
                retval = EArticleListType.Content;
            }
            else if (Equals(EArticleListType.TitleOnly, typeStr))
            {
                retval = EArticleListType.TitleOnly;
            }

            return retval;
        }

        public static bool Equals(EArticleListType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EArticleListType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EArticleListType type, bool selected)
        {
            ListItem item = new ListItem(GetText(type), GetValue(type));
            if (selected)
            {
                item.Selected = true;
            }
            return item;
        }

        public static void AddListItems(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(EArticleListType.Summary, false));
                listControl.Items.Add(GetListItem(EArticleListType.Content, false));
                listControl.Items.Add(GetListItem(EArticleListType.TitleOnly, false));
            }
        }

    }
}
