using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.WeiXin.Model
{
	public enum EConsumeType
	{
        Cash,             //�ֽ�����
        CardAmount           //��Ա���������
    }

    public class EConsumeTypeUtils
	{
        public static string GetValue(EConsumeType type)
		{
            if (type == EConsumeType.Cash)
            {
                return "Cash";
            }
            else if (type == EConsumeType.CardAmount)
            {
                return "CardAmount";
            }
            else
			{
				throw new Exception();
			}
		}

		public static string GetText(EConsumeType type)
		{
            if (type == EConsumeType.Cash )
            {
                return "�ֽ�����";
            }
            else if (type == EConsumeType.CardAmount)
            {
                return "��Ա���������";
            }
            else
			{
				throw new Exception();
			}
		}

		public static EConsumeType GetEnumType(string typeStr)
		{
            EConsumeType retval = EConsumeType.Cash ;

            if (Equals(EConsumeType.Cash, typeStr))
            {
                retval = EConsumeType.Cash;
            }
            else if (Equals(EConsumeType.CardAmount, typeStr))
            {
                retval = EConsumeType.CardAmount;
            }
              
			return retval;
		}

		public static bool Equals(EConsumeType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EConsumeType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EConsumeType type, bool selected)
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
                listControl.Items.Add(GetListItem(EConsumeType.Cash , false));
                listControl.Items.Add(GetListItem(EConsumeType.CardAmount, false));
             }
        }
	}
}
