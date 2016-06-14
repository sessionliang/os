using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.B2C.Model
{
	
	public enum EPromotionTarget
	{
		Site,
		Channels,
		Contents
	}

	public class EPromotionTargetUtils
	{
		public static string GetValue(EPromotionTarget type)
		{
            if (type == EPromotionTarget.Site)
			{
                return "Site";
			}
            else if (type == EPromotionTarget.Channels)
			{
                return "Channels";
			}
            else if (type == EPromotionTarget.Contents)
			{
                return "Contents";
			}
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EPromotionTarget type)
		{
            if (type == EPromotionTarget.Site)
			{
				return "全站促销";
			}
            else if (type == EPromotionTarget.Channels)
			{
				return "指定分类促销";
			}
			else if (type == EPromotionTarget.Contents)
			{
				return "指定商品促销";
			}
			else
			{
				throw new Exception();
			}
		}

		public static EPromotionTarget GetEnumType(string typeStr)
		{
            EPromotionTarget retval = EPromotionTarget.Site;

            if (Equals(EPromotionTarget.Site, typeStr))
			{
                retval = EPromotionTarget.Site;
			}
            else if (Equals(EPromotionTarget.Channels, typeStr))
			{
                retval = EPromotionTarget.Channels;
			}
            else if (Equals(EPromotionTarget.Contents, typeStr))
			{
                retval = EPromotionTarget.Contents;
			}

			return retval;
		}

		public static bool Equals(EPromotionTarget type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EPromotionTarget type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(EPromotionTarget type, bool selected)
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
                listControl.Items.Add(GetListItem(EPromotionTarget.Site, false));
                listControl.Items.Add(GetListItem(EPromotionTarget.Channels, false));
                listControl.Items.Add(GetListItem(EPromotionTarget.Contents, false));
			}
		}

	}
}
