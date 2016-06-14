using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core;

namespace SiteServer.BBS.Core.TemplateParser.Model
{
    public enum EContextType
    {
        Forum,
        Thread,
        Post,
        SqlContent,
        Undefined
    }

    public class EContextTypeUtils
    {
        public static string GetValue(EContextType type)
        {
            if (type == EContextType.Forum)
            {
                return "Forum";
            }
            else if (type == EContextType.Thread)
            {
                return "Thread";
            }
            else if (type == EContextType.Post)
            {
                return "Post";
            }
            else if (type == EContextType.SqlContent)
            {
                return "SqlContent";
            }
            else if (type == EContextType.Undefined)
            {
                return "Undefined";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EContextType GetEnumType(string typeStr)
        {
            EContextType retval = EContextType.Undefined;

            if (Equals(EContextType.Forum, typeStr))
            {
                retval = EContextType.Forum;
            }
            else if (Equals(EContextType.Thread, typeStr))
            {
                retval = EContextType.Thread;
            }
            else if (Equals(EContextType.Post, typeStr))
            {
                retval = EContextType.Post;
            }
            else if (Equals(EContextType.SqlContent, typeStr))
            {
                retval = EContextType.SqlContent;
            }
            else if (Equals(EContextType.Undefined, typeStr))
            {
                retval = EContextType.Undefined;
            }

            return retval;
        }

        public static bool Equals(EContextType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EContextType type)
        {
            return Equals(type, typeStr);
        }
    }
}
