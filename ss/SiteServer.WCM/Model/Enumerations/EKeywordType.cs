using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.WeiXin.Model
{
	public enum EKeywordType
	{
        Text,
        News,
        Coupon,
        Vote,
        Message,
        View360,
        Conference
	}

    public class EKeywordTypeUtils
	{
        public static string GetValue(EKeywordType type)
		{
            if (type == EKeywordType.Text)
            {
                return "Text";
            }
            else if (type == EKeywordType.News)
            {
                return "News";
            }
            else if (type == EKeywordType.Coupon)
            {
                return "Coupon";
            }
            else if (type == EKeywordType.Vote)
            {
                return "Vote";
            }
            else if (type == EKeywordType.Message)
            {
                return "Message";
            }
            else if (type == EKeywordType.View360)
            {
                return "View360";
            }
            else if (type == EKeywordType.Conference)
            {
                return "Conference";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EKeywordType type)
		{
            if (type == EKeywordType.Text)
            {
                return "文本回复";
            }
            else if (type == EKeywordType.News)
            {
                return "图文回复";
            }
            else if (type == EKeywordType.Coupon)
            {
                return "优惠劵";
            }
            else if (type == EKeywordType.Vote)
            {
                return "微投票";
            }
            else if (type == EKeywordType.Message)
            {
                return "微留言";
            }
            else if (type == EKeywordType.View360)
            {
                return "360全景";
            }
            else if (type == EKeywordType.Conference)
            {
                return "微会议";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EKeywordType GetEnumType(string typeStr)
		{
            EKeywordType retval = EKeywordType.Text;

            if (Equals(EKeywordType.Text, typeStr))
            {
                retval = EKeywordType.Text;
            }
            else if (Equals(EKeywordType.News, typeStr))
            {
                retval = EKeywordType.News;
            }
            else if (Equals(EKeywordType.Coupon, typeStr))
            {
                retval = EKeywordType.Coupon;
            }
            else if (Equals(EKeywordType.Vote, typeStr))
            {
                retval = EKeywordType.Vote;
            }
            else if (Equals(EKeywordType.Message, typeStr))
            {
                retval = EKeywordType.Message;
            }
            else if (Equals(EKeywordType.View360, typeStr))
            {
                retval = EKeywordType.View360;
            }
            else if (Equals(EKeywordType.Conference, typeStr))
            {
                retval = EKeywordType.Conference;
            }

			return retval;
		}

		public static bool Equals(EKeywordType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EKeywordType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EKeywordType type, bool selected)
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
                listControl.Items.Add(GetListItem(EKeywordType.Text, false));
                listControl.Items.Add(GetListItem(EKeywordType.News, false));
                listControl.Items.Add(GetListItem(EKeywordType.Coupon, false));
                listControl.Items.Add(GetListItem(EKeywordType.Vote, false));
                listControl.Items.Add(GetListItem(EKeywordType.Message, false));
                listControl.Items.Add(GetListItem(EKeywordType.View360, false));
                listControl.Items.Add(GetListItem(EKeywordType.Conference, false));
            }
        }
	}
}
