using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Collections.Generic;

namespace BaiRong.Model
{
	public enum EPublishmentSystemType
	{
        Weixin,	            //΢����
        CMS,	            //���ݹ���
        WCM,	            //��������
        B2C,	            //��������
        BBS,	            //��̳
        CRM,	            //�ͻ���ϵ����

        Mobile,	            //�ֻ���վ
        MobileB2C,	        //�ֻ��̳�
        WeixinB2C,          //΢�̳�

        UserCenter,         //�û�����
        MLib                //Ͷ��ϵͳ
    }

	public class EPublishmentSystemTypeUtils
	{
		public static string GetValue(EPublishmentSystemType type)
		{
            if (type == EPublishmentSystemType.CMS)
			{
                return "CMS";
			}
            else if (type == EPublishmentSystemType.WCM)
			{
                return "WCM";
            }
            else if (type == EPublishmentSystemType.B2C)
            {
                return "B2C";
            }
            
            else if (type == EPublishmentSystemType.Weixin)
            {
                return "Weixin";
            }
            else if (type == EPublishmentSystemType.BBS)
            {
                return "BBS";
            }
            else if (type == EPublishmentSystemType.CRM)
            {
                return "CRM";
            }
            else if (type == EPublishmentSystemType.Mobile)
            {
                return "Mobile";
            }
            else if (type == EPublishmentSystemType.MobileB2C)
            {
                return "MobileB2C";
            }
            else if (type == EPublishmentSystemType.WeixinB2C)
            {
                return "WeixinB2C";
            }
            else if (type == EPublishmentSystemType.UserCenter)
            {
                return "UserCenter";
            }
            else if (type == EPublishmentSystemType.MLib)
            {
                return "MLib";
            }
            else
			{
				throw new Exception();
			}
		}

        public static string GetText(EPublishmentSystemType type)
        {
            if (type == EPublishmentSystemType.CMS)
            {
                return "���ݹ���";
            }
            else if (type == EPublishmentSystemType.WCM)
            {
                return "��������";
            }
            else if (type == EPublishmentSystemType.B2C)
            {
                return "��������";
            }
            else if (type == EPublishmentSystemType.Weixin)
            {
                return "΢����";
            }
            else if (type == EPublishmentSystemType.WeixinB2C)
            {
                return "΢�̳�";
            }
            else if (type == EPublishmentSystemType.BBS)
            {
                return "��̳";
            }
            else if (type == EPublishmentSystemType.CRM)
            {
                return "�ͻ���ϵ����";
            }
            else if (type == EPublishmentSystemType.Mobile)
            {
                return "�ֻ���վ";
            }
            else if (type == EPublishmentSystemType.MobileB2C)
            {
                return "�ֻ��̳�";
            }
            else if (type == EPublishmentSystemType.UserCenter)
            {
                return "�û�����";
            }
            else if (type == EPublishmentSystemType.MLib)
            {
                return "�����";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetDescription(EPublishmentSystemType type)
        {
            if (type == EPublishmentSystemType.CMS)
            {
                return "CMS ���ݹ���ϵͳ";
            }
            else if (type == EPublishmentSystemType.WCM)
            {
                return "WCM ����Э��ƽ̨";
            }
            else if (type == EPublishmentSystemType.B2C)
            {
                return "B2C ��������ϵͳ";
            }
            else if (type == EPublishmentSystemType.Mobile)
            {
                return "Mobile �ֻ����ݹ���";
            }
            else if (type == EPublishmentSystemType.MobileB2C)
            {
                return "MobileB2C �ֻ��̳�ϵͳ";
            }
            else if (type == EPublishmentSystemType.Weixin)
            {
                return "Weixin ΢��������ϵͳ";
            }
            else if (type == EPublishmentSystemType.WeixinB2C)
            {
                return "WeixinB2C ΢�̳ǹ���ϵͳ";
            }
            else if (type == EPublishmentSystemType.BBS)
            {
                return "BBS ��̳ϵͳ";
            }            
            else if (type == EPublishmentSystemType.CRM)
            {
                return "CRM �ͻ���ϵ����ϵͳ";
            }
            else if (type == EPublishmentSystemType.UserCenter)
            {
                return "UserCenter �û�����ϵͳ";
            }
            else if (type == EPublishmentSystemType.MLib)
            {
                return "MLib �����";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetAppName(EPublishmentSystemType type)
        {
            if (type == EPublishmentSystemType.CMS)
            {
                return "SiteServer CMS";
            }
            else if (type == EPublishmentSystemType.WCM)
            {
                return "SiteServer WCM";
            }
            else if (type == EPublishmentSystemType.B2C)
            {
                return "SiteServer B2C";
            }
            else if (type == EPublishmentSystemType.Mobile)
            {
                return "SiteServer Mobile";
            }
            else if (type == EPublishmentSystemType.MobileB2C)
            {
                return "SiteServer MobileB2C";
            }
            else if (type == EPublishmentSystemType.Weixin)
            {
                return "SiteServer Weixin";
            }
            else if (type == EPublishmentSystemType.WeixinB2C)
            {
                return "SiteServer WeixinB2C";
            }
            else if (type == EPublishmentSystemType.BBS)
            {
                return "SiteServer BBS";
            }
            else if (type == EPublishmentSystemType.CRM)
            {
                return "SiteServer CRM";
            }
            else if (type == EPublishmentSystemType.UserCenter)
            {
                return "SiteServer UserCenter";
            }
            else if (type == EPublishmentSystemType.MLib)
            {
                return "SiteServer MLib";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetIconHtml(EPublishmentSystemType type)
        {
            return GetIconHtml(type, "icon-large");
        }

        public static string GetIconHtml(EPublishmentSystemType type, string iconClass)
        {
            string html = string.Empty;

            if (type == EPublishmentSystemType.CMS)
            {
                html = @"<i class=""icon-globe {0}""></i>";
            }
            else if (type == EPublishmentSystemType.WCM)
            {
                html = @"<i class=""icon-sitemap {0}""></i>";
            }
            else if (type == EPublishmentSystemType.B2C)
            {
                html = @"<i class=""icon-shopping-cart {0}""></i>";
            }
            else if (type == EPublishmentSystemType.Mobile)
            {
                html = @"&nbsp;<i class=""icon-mobile-phone {0}""></i>&nbsp;";
            }
            else if (type == EPublishmentSystemType.MobileB2C)
            {
                html = @"&nbsp;<i class=""icon-cny {0}""></i>&nbsp;";
            }
            else if (type == EPublishmentSystemType.Weixin)
            {
                html = @"<i class=""icon-qrcode {0}""></i>";
            }
            else if (type == EPublishmentSystemType.WeixinB2C)
            {
                html = @"<i class=""icon-jpy {0}""></i>";
            }
            else if (type == EPublishmentSystemType.BBS)
            {
                html = @"<i class=""icon-comments {0}""></i>";
            }
            else if (type == EPublishmentSystemType.CRM)
            {
                html = @"<i class=""icon-group {0}""></i>";
            }
            else if (type == EPublishmentSystemType.UserCenter)
            {
                html = @"<i class=""icon-globe {0}""></i>";
            }
            else if (type == EPublishmentSystemType.MLib)
            {
                html = @"<i class=""icon-tracker {0}""></i>";
            }

            return string.Format(html, iconClass);
        }

        public static string GetHtml(EPublishmentSystemType type, bool isDescription)
        {
            if (isDescription)
            {
                return string.Format("{0}&nbsp;{1}Ӧ��<small>({2})</small>", EPublishmentSystemTypeUtils.GetIconHtml(type), EPublishmentSystemTypeUtils.GetText(type), EPublishmentSystemTypeUtils.GetDescription(type));
            }
            else
            {
                return string.Format("{0}&nbsp;{1}Ӧ��", EPublishmentSystemTypeUtils.GetIconHtml(type), EPublishmentSystemTypeUtils.GetText(type));
            }
        }

		public static EPublishmentSystemType GetEnumType(string typeStr)
		{
			EPublishmentSystemType retval = EPublishmentSystemType.CMS;

            if (Equals(EPublishmentSystemType.CMS, typeStr))
			{
                retval = EPublishmentSystemType.CMS;
            }
            else if (Equals(EPublishmentSystemType.WCM, typeStr))
            {
                retval = EPublishmentSystemType.WCM;
            }
            else if (Equals(EPublishmentSystemType.B2C, typeStr))
			{
                retval = EPublishmentSystemType.B2C;
            }
            else if (Equals(EPublishmentSystemType.Mobile, typeStr))
            {
                retval = EPublishmentSystemType.Mobile;
            }
            else if (Equals(EPublishmentSystemType.MobileB2C, typeStr))
            {
                retval = EPublishmentSystemType.MobileB2C;
            }
            else if (Equals(EPublishmentSystemType.Weixin, typeStr))
            {
                retval = EPublishmentSystemType.Weixin;
            }
            else if (Equals(EPublishmentSystemType.WeixinB2C, typeStr))
            {
                retval = EPublishmentSystemType.WeixinB2C;
            }
            else if (Equals(EPublishmentSystemType.BBS, typeStr))
            {
                retval = EPublishmentSystemType.BBS;
            }
            else if (Equals(EPublishmentSystemType.CRM, typeStr))
            {
                retval = EPublishmentSystemType.CRM;
            }
            else if (Equals(EPublishmentSystemType.UserCenter, typeStr))
            {
                retval = EPublishmentSystemType.UserCenter;
            }
            else if (Equals(EPublishmentSystemType.MLib, typeStr))
            {
                retval = EPublishmentSystemType.MLib;
            }

            return retval;
		}

		public static bool Equals(EPublishmentSystemType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

		public static bool Equals(string typeStr, EPublishmentSystemType type)
		{
			return Equals(type, typeStr);
		}

        public static ListItem GetListItem(EPublishmentSystemType type, bool selected)
        {
            ListItem item = new ListItem(GetText(type), GetValue(type));
            if (selected)
            {
                item.Selected = true;
            }
            return item;
        }

        public static bool IsEnabled(EPublishmentSystemType type)
        {
            if (type == EPublishmentSystemType.WCM 
                || type == EPublishmentSystemType.CRM)
            {
                return false;
            }
            return true;
        }

        public static bool IsB2C(EPublishmentSystemType type)
        {
            if (type == EPublishmentSystemType.B2C || type == EPublishmentSystemType.MobileB2C || type == EPublishmentSystemType.WeixinB2C)
            {
                return true;
            }
            return false;
        }

        public static bool IsWeixin(EPublishmentSystemType type)
        {
            if (type == EPublishmentSystemType.Weixin || type == EPublishmentSystemType.WeixinB2C)
            {
                return true;
            }
            return false;
        }

        public static bool IsUserCenter(EPublishmentSystemType type)
        {
            if (type == EPublishmentSystemType.UserCenter)
            {
                return true;
            }
            return false;
        }

        public static bool IsMLib(EPublishmentSystemType type)
        {
            if (type == EPublishmentSystemType.MLib)
            {
                return true;
            }
            return false;
        }


        public static bool IsNodeRelated(EPublishmentSystemType type)
        {
            if (type == EPublishmentSystemType.CMS || type == EPublishmentSystemType.Mobile || type == EPublishmentSystemType.B2C || type == EPublishmentSystemType.MobileB2C || type == EPublishmentSystemType.Weixin || type == EPublishmentSystemType.WeixinB2C || type == EPublishmentSystemType.WCM || type == EPublishmentSystemType.UserCenter)
            {
                return true;
            }
            return false;
        }

        public static bool IsMobile(EPublishmentSystemType type)
        {
            if (type == EPublishmentSystemType.Weixin || type == EPublishmentSystemType.WeixinB2C || type == EPublishmentSystemType.Mobile || type == EPublishmentSystemType.MobileB2C)
            {
                return true;
            }
            return false;
        }

        public static void AddListItems(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(EPublishmentSystemType.CMS, false));
                listControl.Items.Add(GetListItem(EPublishmentSystemType.BBS, false));
                listControl.Items.Add(GetListItem(EPublishmentSystemType.Weixin, false));
                listControl.Items.Add(GetListItem(EPublishmentSystemType.B2C, false));
                listControl.Items.Add(GetListItem(EPublishmentSystemType.WeixinB2C, false));
                listControl.Items.Add(GetListItem(EPublishmentSystemType.Mobile, false));
                listControl.Items.Add(GetListItem(EPublishmentSystemType.MobileB2C, false));
                listControl.Items.Add(GetListItem(EPublishmentSystemType.WCM, false));
                listControl.Items.Add(GetListItem(EPublishmentSystemType.CRM, false));
                listControl.Items.Add(GetListItem(EPublishmentSystemType.UserCenter, false));
            }
        }

        public static List<EPublishmentSystemType> AllList()
        {
            List<EPublishmentSystemType> list = new List<EPublishmentSystemType>();

            list.Add(EPublishmentSystemType.CMS);
            list.Add(EPublishmentSystemType.BBS);
            list.Add(EPublishmentSystemType.Weixin);
            list.Add(EPublishmentSystemType.B2C);
            list.Add(EPublishmentSystemType.WeixinB2C);
            list.Add(EPublishmentSystemType.Mobile);
            list.Add(EPublishmentSystemType.MobileB2C);
            list.Add(EPublishmentSystemType.WCM);
            list.Add(EPublishmentSystemType.CRM);
            list.Add(EPublishmentSystemType.UserCenter);

            return list;
        }

        public static string GetAppID(EPublishmentSystemType type)
        {
            if (type == EPublishmentSystemType.CMS)
            {
                return AppManager.CMS.AppID;
            }
            else if (type == EPublishmentSystemType.WCM)
            {
                return AppManager.WCM.AppID;
            }
            else if (type == EPublishmentSystemType.B2C)
            {
                return AppManager.B2C.AppID;
            }
            //else if (type == EPublishmentSystemType.Mobile)
            //{
            //    return AppManager.mobiel.AppID;
            //}
            //else if (type == EPublishmentSystemType.MobileB2C)
            //{
            //    return AppManager.B2C
            //}
            else if (type == EPublishmentSystemType.Weixin)
            {
                return AppManager.WeiXin.AppID;
            }
            else if (type == EPublishmentSystemType.WeixinB2C)
            {
                return AppManager.WeiXinB2C.AppID;
            }
            else if (type == EPublishmentSystemType.BBS)
            {
                return AppManager.BBS.AppID;
            }
            else if (type == EPublishmentSystemType.CRM)
            {
                return AppManager.CRM.AppID;
            }
            else if (type == EPublishmentSystemType.UserCenter)
            {
                return AppManager.UserCenter.AppID;
            }
            else if (type == EPublishmentSystemType.MLib)
            {
                return AppManager.MLib.AppID;
            }

            return string.Empty;
        }
	}
}
