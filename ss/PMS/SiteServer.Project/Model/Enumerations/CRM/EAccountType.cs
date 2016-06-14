using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.Project.Model
{
    public enum EAccountType
	{
        SiteServer_Customer,           //SiteServer �ͻ�
        SiteServer_Agent,              //SiteServer ������
        SiteYun_Partner,               //SiteYun �������
	}

    public class EAccountTypeUtils
	{
		public static string GetValue(EAccountType type)
		{
            if (type == EAccountType.SiteServer_Customer)
			{
                return "SiteServer_Customer";
			}
            else if (type == EAccountType.SiteServer_Agent)
            {
                return "SiteServer_Agent";
            }
            else if (type == EAccountType.SiteYun_Partner)
            {
                return "SiteYun_Partner";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EAccountType type)
		{
            if (type == EAccountType.SiteServer_Customer)
			{
                return "SiteServer �ͻ�";
			}
            else if (type == EAccountType.SiteServer_Agent)
            {
                return "SiteServer ������";
            }
            else if (type == EAccountType.SiteYun_Partner)
            {
                return "SiteYun �������";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EAccountType GetEnumType(string typeStr)
		{
            EAccountType retval = EAccountType.SiteServer_Customer;

            if (Equals(EAccountType.SiteServer_Customer, typeStr))
			{
                retval = EAccountType.SiteServer_Customer;
			}
            else if (Equals(EAccountType.SiteServer_Agent, typeStr))
            {
                retval = EAccountType.SiteServer_Agent;
            }
            else if (Equals(EAccountType.SiteYun_Partner, typeStr))
            {
                retval = EAccountType.SiteYun_Partner;
            }
			return retval;
		}

		public static bool Equals(EAccountType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EAccountType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EAccountType type, bool selected)
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
                listControl.Items.Add(GetListItem(EAccountType.SiteServer_Customer, false));
                listControl.Items.Add(GetListItem(EAccountType.SiteServer_Agent, false));
                listControl.Items.Add(GetListItem(EAccountType.SiteYun_Partner, false));
            }
        }
	}
}
