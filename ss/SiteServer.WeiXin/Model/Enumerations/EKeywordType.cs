using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.WeiXin.Core;

namespace SiteServer.WeiXin.Model
{
	public enum EKeywordType
	{
        Text,
        News,
        Coupon,
        Vote,
        Collect,
        Message,
        Appointment,
        View360,
        Map,
        Conference,
        Album,
        Scratch,
        BigWheel,
        GoldEgg,
        Flap,
        YaoYao,
        Search,
        Store,
        Wifi,
        Card
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
            else if (type == EKeywordType.Collect)
            {
                return "Collect";
            }
            else if (type == EKeywordType.Message)
            {
                return "Message";
            }
            else if (type == EKeywordType.Appointment)
            {
                return "Appointment";
            }
            else if (type == EKeywordType.View360)
            {
                return "View360";
            }
            else if (type == EKeywordType.Map)
            {
                return "Map";
            }
            else if (type == EKeywordType.Conference)
            {
                return "Conference";
            }
            else if (type == EKeywordType.Album)
            {
                return "Album";
            }
            else if (type == EKeywordType.Scratch)
            {
                return "Scratch";
            }
            else if (type == EKeywordType.BigWheel)
            {
                return "BigWheel";
            }
            else if (type == EKeywordType.GoldEgg)
            {
                return "GoldEgg";
            }
            else if (type == EKeywordType.Flap)
            {
                return "Flap";
            }
            else if (type == EKeywordType.YaoYao)
            {
                return "YaoYao";
            }
            else if (type == EKeywordType.Search)
            {
                return "Search";
            }
            else if (type == EKeywordType.Store)
            {
                return "Store";
            }
            else if (type == EKeywordType.Wifi)
            {
                return "Wifi";
            }
            else if (type == EKeywordType.Card)
            {
                return "Card";
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
                return "ÎÄ±¾»Ø¸´";
            }
            else if (type == EKeywordType.News)
            {
                return "Í¼ÎÄ»Ø¸´";
            }
            else if (type == EKeywordType.Coupon)
            {
                return "ÓÅ»Ý„»";
            }
            else if (type == EKeywordType.Vote)
            {
                return "Î¢Í¶Æ±";
            }
            else if (type == EKeywordType.Collect)
            {
                return "Õ÷¼¯Í¶Æ±";
            }
            else if (type == EKeywordType.Message)
            {
                return "Î¢ÁôÑÔ";
            }
            else if (type == EKeywordType.Appointment)
            {
                return "Î¢Ô¤Ô¼";
            }
            else if (type == EKeywordType.View360)
            {
                return "360È«¾°";
            }
            else if (type == EKeywordType.Map)
            {
                return "Î¢µ¼º½";
            }
            else if (type == EKeywordType.Conference)
            {
                return "Î¢»áÒé";
            }
            else if (type == EKeywordType.Album)
            {
                return "Î¢Ïà²á";
            }
            else if (type == EKeywordType.Scratch)
            {
                return "¹Î¹Î¿¨";
            }
            else if (type == EKeywordType.BigWheel)
            {
                return "´ó×ªÅÌ";
            }
            else if (type == EKeywordType.GoldEgg)
            {
                return "ÔÒ½ðµ°";
            }
            else if (type == EKeywordType.Flap)
            {
                return "´ó·­ÅÆ";
            }
            else if (type == EKeywordType.YaoYao)
            {
                return "Ò¡Ò¡ÀÖ";
            }
            else if (type == EKeywordType.Search)
            {
                return "Î¢ËÑË÷";
            }
            else if (type == EKeywordType.Store)
            {
                return "Î¢ÃÅµê";
            }
            else if (type == EKeywordType.Wifi)
            {
                return "Î¢ÐÅWIFI";
            }
            else if (type == EKeywordType.Card)
            {
                return "»áÔ±¿¨";
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
            else if (Equals(EKeywordType.Collect, typeStr))
            {
                retval = EKeywordType.Collect;
            }
            else if (Equals(EKeywordType.Message, typeStr))
            {
                retval = EKeywordType.Message;
            }
            else if (Equals(EKeywordType.Appointment, typeStr))
            {
                retval = EKeywordType.Appointment;
            }
            else if (Equals(EKeywordType.View360, typeStr))
            {
                retval = EKeywordType.View360;
            }
            else if (Equals(EKeywordType.Map, typeStr))
            {
                retval = EKeywordType.Map;
            }
            else if (Equals(EKeywordType.Conference, typeStr))
            {
                retval = EKeywordType.Conference;
            }
            else if (Equals(EKeywordType.Album, typeStr))
            {
                retval = EKeywordType.Album;
            }
            else if (Equals(EKeywordType.Scratch, typeStr))
            {
                retval = EKeywordType.Scratch;
            }
            else if (Equals(EKeywordType.BigWheel, typeStr))
            {
                retval = EKeywordType.BigWheel;
            }
            else if (Equals(EKeywordType.GoldEgg, typeStr))
            {
                retval = EKeywordType.GoldEgg;
            }
            else if (Equals(EKeywordType.Flap, typeStr))
            {
                retval = EKeywordType.Flap;
            }
            else if (Equals(EKeywordType.YaoYao, typeStr))
            {
                retval = EKeywordType.YaoYao;
            }
            else if (Equals(EKeywordType.Search, typeStr))
            {
                retval = EKeywordType.Search;
            }
            else if (Equals(EKeywordType.Store, typeStr))
            {
                retval = EKeywordType.Store;
            }
            else if (Equals(EKeywordType.Wifi, typeStr))
            {
                retval = EKeywordType.Wifi;
            }
            else if (Equals(EKeywordType.Card, typeStr))
            {
                retval = EKeywordType.Card;
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
                listControl.Items.Add(GetListItem(EKeywordType.Scratch, false));
                listControl.Items.Add(GetListItem(EKeywordType.BigWheel, false));
                listControl.Items.Add(GetListItem(EKeywordType.GoldEgg, false));
                listControl.Items.Add(GetListItem(EKeywordType.Flap, false));
                listControl.Items.Add(GetListItem(EKeywordType.YaoYao, false));
                listControl.Items.Add(GetListItem(EKeywordType.Vote, false));
                listControl.Items.Add(GetListItem(EKeywordType.Collect, false));
                listControl.Items.Add(GetListItem(EKeywordType.Message, false));
                listControl.Items.Add(GetListItem(EKeywordType.Appointment, false));
                listControl.Items.Add(GetListItem(EKeywordType.Conference, false));
                listControl.Items.Add(GetListItem(EKeywordType.Album, false));
                listControl.Items.Add(GetListItem(EKeywordType.Map, false));
                listControl.Items.Add(GetListItem(EKeywordType.View360, false));
                listControl.Items.Add(GetListItem(EKeywordType.Search, false));
                listControl.Items.Add(GetListItem(EKeywordType.Store, false));
                listControl.Items.Add(GetListItem(EKeywordType.Wifi, false));
                listControl.Items.Add(GetListItem(EKeywordType.Card, false));
            }
        }

        public static void AddListItemsUrlOnly(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(EKeywordType.Coupon, false));
                listControl.Items.Add(GetListItem(EKeywordType.Scratch, false));
                listControl.Items.Add(GetListItem(EKeywordType.BigWheel, false));
                listControl.Items.Add(GetListItem(EKeywordType.GoldEgg, false));
                listControl.Items.Add(GetListItem(EKeywordType.Flap, false));
                listControl.Items.Add(GetListItem(EKeywordType.YaoYao, false));
                listControl.Items.Add(GetListItem(EKeywordType.Vote, false));
                listControl.Items.Add(GetListItem(EKeywordType.Collect, false));
                listControl.Items.Add(GetListItem(EKeywordType.Message, false));
                listControl.Items.Add(GetListItem(EKeywordType.Appointment, false));
                listControl.Items.Add(GetListItem(EKeywordType.Conference, false));
                listControl.Items.Add(GetListItem(EKeywordType.Album, false));
                listControl.Items.Add(GetListItem(EKeywordType.Map, false));
                listControl.Items.Add(GetListItem(EKeywordType.View360, false));
                listControl.Items.Add(GetListItem(EKeywordType.Search, false));
                listControl.Items.Add(GetListItem(EKeywordType.Store, false));
                listControl.Items.Add(GetListItem(EKeywordType.Wifi, false));
                listControl.Items.Add(GetListItem(EKeywordType.Card, false));
            }
        }
	}
}
