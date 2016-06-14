using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.Model
{
    public enum EThreadType
    {
        Post,       //帖子
        Poll,       //投票
    }

    public class EThreadTypeUtils
    {
        public static string GetValue(EThreadType type)
        {
            if (type == EThreadType.Post)
            {
                return "Post";
            }
            else if (type == EThreadType.Poll)
            {
                return "Poll";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EThreadType type)
        {
            if (type == EThreadType.Post)
            {
                return "帖子";
            }
            else if (type == EThreadType.Poll)
            {
                return "投票";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EThreadType GetEnumType(string typeStr)
        {
            EThreadType retval = EThreadType.Post;

            if (Equals(EThreadType.Poll, typeStr))
            {
                retval = EThreadType.Poll;
            }

            return retval;
        }

        public static bool Equals(EThreadType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EThreadType type)
        {
            return Equals(type, typeStr);
        }
    }
}
