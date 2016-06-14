using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CRM.Model
{
    public enum EOrderType
	{
        Aliyun_Moban,           //阿里云成品网站
        Aliyun_Software,        //阿里云软件
        Taobao_Service,         //淘宝服务
	}

    public class EOrderTypeUtils
	{
		public static string GetValue(EOrderType type)
		{
            if (type == EOrderType.Aliyun_Moban)
			{
                return "Aliyun_Moban";
			}
            else if (type == EOrderType.Aliyun_Software)
            {
                return "Aliyun_Software";
            }
            else if (type == EOrderType.Taobao_Service)
            {
                return "Taobao_Service";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EOrderType type)
		{
            if (type == EOrderType.Aliyun_Moban)
			{
                return "阿里云成品网站";
			}
            else if (type == EOrderType.Aliyun_Software)
            {
                return "阿里云软件";
            }
            else if (type == EOrderType.Taobao_Service)
            {
                return "淘宝服务";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EOrderType GetEnumType(string typeStr)
		{
            EOrderType retval = EOrderType.Aliyun_Moban;

            if (Equals(EOrderType.Aliyun_Moban, typeStr))
			{
                retval = EOrderType.Aliyun_Moban;
			}
            else if (Equals(EOrderType.Aliyun_Software, typeStr))
            {
                retval = EOrderType.Aliyun_Software;
            }
            else if (Equals(EOrderType.Taobao_Service, typeStr))
            {
                retval = EOrderType.Taobao_Service;
            }
			return retval;
		}

		public static bool Equals(EOrderType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EOrderType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EOrderType type, bool selected)
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
                listControl.Items.Add(GetListItem(EOrderType.Aliyun_Moban, false));
                listControl.Items.Add(GetListItem(EOrderType.Aliyun_Software, false));
                listControl.Items.Add(GetListItem(EOrderType.Taobao_Service, false));
            }
        }
	}
}
