using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.WeiXin.Model
{
	public enum ECashType
	{
        Consume,             //����
        Recharge,            //��ֵ
        Exchange             //�һ�
    }

    public class ECashTypeUtils
	{
        public static string GetValue(ECashType type)
		{
            if (type == ECashType.Consume)
            {
                return "Consume";
            }
            else if (type == ECashType.Recharge)
            {
                return "Recharge";
            }
            else if (type == ECashType.Exchange)
            {
                return "Exchange";
            }
            else
			{
				throw new Exception();
			}
		}

		public static string GetText(ECashType type)
		{
            if (type == ECashType.Consume)
            {
                return "����";
            }
            else if (type == ECashType.Recharge)
            {
                return "��ֵ";
            }
            else if (type == ECashType.Exchange)
            {
                return "�һ�";
            }
            else
            {
                throw new Exception();
            }
		}

		public static ECashType GetEnumType(string typeStr)
		{
            ECashType retval = ECashType.Consume;

            if (Equals(ECashType.Consume, typeStr))
            {
                retval = ECashType.Consume;
            }
            else if (Equals(ECashType.Recharge, typeStr))
            {
                retval = ECashType.Recharge;
            }
            else if (Equals(ECashType.Exchange, typeStr))
            {
                retval = ECashType.Exchange;
            }
			return retval;
		}

		public static bool Equals(ECashType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, ECashType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(ECashType type, bool selected)
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
                listControl.Items.Add(GetListItem(ECashType.Consume, false));
                listControl.Items.Add(GetListItem(ECashType.Recharge, false));
                listControl.Items.Add(GetListItem(ECashType.Exchange,false));
             }
        }
	}
}
