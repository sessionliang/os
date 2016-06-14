using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CRM.Model
{
    public enum EContractType
	{
        SiteServer_Software,                    //SiteServer �����ͬ
        SiteServer_Project,                     //SiteServer ��Ŀ��ͬ
        SiteServer_Agent,                       //SiteServer �����ͬ
        SiteYun_Order,                          //SiteYun ������ͬ
        SiteYun_Partner,                        //SiteYun ����Э��
        Other                                   //����
	}

    public class EContractTypeUtils
	{
		public static string GetValue(EContractType type)
		{
            if (type == EContractType.SiteServer_Software)
			{
                return "SiteServer_Software";
			}
            else if (type == EContractType.SiteServer_Project)
            {
                return "SiteServer_Project";
            }
            else if (type == EContractType.SiteServer_Agent)
            {
                return "SiteServer_Agent";
            }
            else if (type == EContractType.SiteYun_Order)
            {
                return "SiteYun_Order";
            }
            else if (type == EContractType.SiteYun_Partner)
            {
                return "SiteYun_Partner";
            }
            else if (type == EContractType.Other)
            {
                return "Other";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EContractType type)
		{
            if (type == EContractType.SiteServer_Software)
            {
                return "SiteServer �����ͬ";
            }
            else if (type == EContractType.SiteServer_Project)
            {
                return "SiteServer ��Ŀ��ͬ";
            }
            else if (type == EContractType.SiteServer_Agent)
            {
                return "SiteServer �����ͬ";
            }
            else if (type == EContractType.SiteYun_Order)
            {
                return "SiteYun ������ͬ";
            }
            else if (type == EContractType.SiteYun_Partner)
            {
                return "SiteYun ����Э��";
            }
            else if (type == EContractType.Other)
            {
                return "����";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EContractType GetEnumType(string typeStr)
		{
            EContractType retval = EContractType.Other;

            if (Equals(EContractType.SiteServer_Software, typeStr))
			{
                retval = EContractType.SiteServer_Software;
			}
            else if (Equals(EContractType.SiteServer_Project, typeStr))
            {
                retval = EContractType.SiteServer_Project;
            }
            else if (Equals(EContractType.SiteServer_Agent, typeStr))
            {
                retval = EContractType.SiteServer_Agent;
            }
            else if (Equals(EContractType.SiteYun_Order, typeStr))
            {
                retval = EContractType.SiteYun_Order;
            }
            else if (Equals(EContractType.SiteYun_Partner, typeStr))
            {
                retval = EContractType.SiteYun_Partner;
            }
			return retval;
		}

		public static bool Equals(EContractType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EContractType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EContractType type, bool selected)
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
                listControl.Items.Add(GetListItem(EContractType.SiteServer_Software, false));
                listControl.Items.Add(GetListItem(EContractType.SiteServer_Project, false));
                listControl.Items.Add(GetListItem(EContractType.SiteServer_Agent, false));
                listControl.Items.Add(GetListItem(EContractType.SiteYun_Order, false));
                listControl.Items.Add(GetListItem(EContractType.SiteYun_Partner, false));
                listControl.Items.Add(GetListItem(EContractType.Other, false));
            }
        }
	}
}
