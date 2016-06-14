using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;

namespace SiteServer.BBS.Model
{
    public enum ETemplateType
    {
        Index,
        Forum,
        Thread,
        File
    }

    public class ETemplateTypeUtils
    {
        public static string GetValue(ETemplateType type)
        {
            if (type == ETemplateType.Index)
            {
                return "Index";
            }
            else if (type == ETemplateType.Forum)
            {
                return "Forum";
            }
            else if (type == ETemplateType.Thread)
            {
                return "Thread";
            }
            else if (type == ETemplateType.File)
            {
                return "File";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(ETemplateType type)
        {
            if (type == ETemplateType.Index)
            {
                return "首页";
            }
            else if (type == ETemplateType.Forum)
            {
                return "板块页";
            }
            else if (type == ETemplateType.Thread)
            {
                return "主题页";
            }
            else if (type == ETemplateType.File)
            {
                return "文件页";
            }
            else
            {
                throw new Exception();
            }
        }

        public static ETemplateType GetEnumType(string typeStr)
        {
            ETemplateType retval = ETemplateType.File;

            if (Equals(ETemplateType.Index, typeStr))
            {
                retval = ETemplateType.Index;
            }
            else if (Equals(ETemplateType.Forum, typeStr))
            {
                retval = ETemplateType.Forum;
            }
            else if (Equals(ETemplateType.Thread, typeStr))
            {
                retval = ETemplateType.Thread;
            }

            return retval;
        }

        public static bool Equals(ETemplateType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ETemplateType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(ETemplateType type, bool selected)
        {
            ListItem item = new ListItem(GetText(type), GetValue(type));
            if (selected)
            {
                item.Selected = true;
            }
            return item;
        }
    }
}
