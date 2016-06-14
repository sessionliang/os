using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.B2C.Model
{
    public enum ERequestEstimate
	{
        VerySatisfied,         //非常满意
        Satisfied,             //比较满意
        Dissatisfied           //不满意
	}

    public class ERequestEstimateUtils
	{
		public static string GetValue(ERequestEstimate type)
		{
            if (type == ERequestEstimate.VerySatisfied)
			{
                return "VerySatisfied";
			}
            else if (type == ERequestEstimate.Satisfied)
            {
                return "Satisfied";
            }
            else if (type == ERequestEstimate.Dissatisfied)
            {
                return "Dissatisfied";
            }
			else
			{
				throw new Exception();
			}
		}

        public static string GetText(string strType)
        {
            return GetText(GetEnumType(strType));
        }

		public static string GetText(ERequestEstimate type)
		{
            if (type == ERequestEstimate.VerySatisfied)
			{
                return "非常满意";
			}
            else if (type == ERequestEstimate.Satisfied)
            {
                return "比较满意";
            }
            else if (type == ERequestEstimate.Dissatisfied)
            {
                return "不满意";
            }
			else
			{
				throw new Exception();
			}
		}

		public static ERequestEstimate GetEnumType(string typeStr)
		{
            ERequestEstimate retval = ERequestEstimate.Satisfied;

            if (Equals(ERequestEstimate.VerySatisfied, typeStr))
			{
                retval = ERequestEstimate.VerySatisfied;
			}
            else if (Equals(ERequestEstimate.Satisfied, typeStr))
            {
                retval = ERequestEstimate.Satisfied;
            }
            else if (Equals(ERequestEstimate.Dissatisfied, typeStr))
            {
                retval = ERequestEstimate.Dissatisfied;
            }
			return retval;
		}

		public static bool Equals(ERequestEstimate type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, ERequestEstimate type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(ERequestEstimate type, bool selected)
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
                listControl.Items.Add(GetListItem(ERequestEstimate.VerySatisfied, false));
                listControl.Items.Add(GetListItem(ERequestEstimate.Satisfied, false));
                listControl.Items.Add(GetListItem(ERequestEstimate.Dissatisfied, false));
            }
        }
	}
}
