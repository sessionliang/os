using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	public enum ECrossSiteTransType
	{
        None,
        SelfSite,
        SpecifiedSite,
        ParentSite,
        AllParentSite,
		AllSite
	}

    public class ECrossSiteTransTypeUtils
	{
		public static string GetValue(ECrossSiteTransType type)
		{
            if (type == ECrossSiteTransType.None)
            {
                return "None";
            }
            else if (type == ECrossSiteTransType.SelfSite)
            {
                return "SelfSite";
            }
            else if (type == ECrossSiteTransType.SpecifiedSite)
            {
                return "SpecifiedSite";
            }
            else if (type == ECrossSiteTransType.ParentSite)
            {
                return "ParentSite";
            }
            else if (type == ECrossSiteTransType.AllParentSite)
			{
                return "AllParentSite";
            }
            else if (type == ECrossSiteTransType.AllSite)
            {
                return "AllSite";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(ECrossSiteTransType type)
		{
            if (type == ECrossSiteTransType.None)
            {
                return "��ת��";
            }
            else if (type == ECrossSiteTransType.SelfSite)
            {
                return "����վת��";
            }
            else if (type == ECrossSiteTransType.SpecifiedSite)
            {
                return "����ָ��Ӧ��ת��";
            }
            else if (type == ECrossSiteTransType.ParentSite)
            {
                return "������һ��Ӧ��ת��";
            }
            else if (type == ECrossSiteTransType.AllParentSite)
			{
                return "���������ϼ�Ӧ��ת��";
            }
            else if (type == ECrossSiteTransType.AllSite)
            {
                return "��������Ӧ��ת��";
            }
			else
			{
				throw new Exception();
			}
		}

		public static ECrossSiteTransType GetEnumType(string typeStr)
		{
            ECrossSiteTransType retval = ECrossSiteTransType.None;

            if (Equals(ECrossSiteTransType.None, typeStr))
            {
                retval = ECrossSiteTransType.None;
            }
            else if (Equals(ECrossSiteTransType.SelfSite, typeStr))
            {
                retval = ECrossSiteTransType.SelfSite;
            }
            else if (Equals(ECrossSiteTransType.SpecifiedSite, typeStr))
            {
                retval = ECrossSiteTransType.SpecifiedSite;
            }
            else if (Equals(ECrossSiteTransType.ParentSite, typeStr))
            {
                retval = ECrossSiteTransType.ParentSite;
            }
            else if (Equals(ECrossSiteTransType.AllParentSite, typeStr))
			{
                retval = ECrossSiteTransType.AllParentSite;
            }
            else if (Equals(ECrossSiteTransType.AllSite, typeStr))
            {
                retval = ECrossSiteTransType.AllSite;
            }

			return retval;
		}

		public static bool Equals(ECrossSiteTransType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, ECrossSiteTransType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(ECrossSiteTransType type, bool selected)
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
                listControl.Items.Add(GetListItem(ECrossSiteTransType.None, false));
                listControl.Items.Add(GetListItem(ECrossSiteTransType.SelfSite, false));
                listControl.Items.Add(GetListItem(ECrossSiteTransType.ParentSite, false));
                listControl.Items.Add(GetListItem(ECrossSiteTransType.AllParentSite, false));
                listControl.Items.Add(GetListItem(ECrossSiteTransType.AllSite, false));
            }
        }

        public static void AddAllListItems(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(ECrossSiteTransType.None, false));
                listControl.Items.Add(GetListItem(ECrossSiteTransType.SelfSite, false));
                listControl.Items.Add(GetListItem(ECrossSiteTransType.SpecifiedSite, false));
                listControl.Items.Add(GetListItem(ECrossSiteTransType.ParentSite, false));
                listControl.Items.Add(GetListItem(ECrossSiteTransType.AllParentSite, false));
                listControl.Items.Add(GetListItem(ECrossSiteTransType.AllSite, false));
            }
        }
	}
}
