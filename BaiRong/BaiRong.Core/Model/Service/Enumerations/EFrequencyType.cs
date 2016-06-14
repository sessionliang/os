using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model.Service
{
    public enum EFrequencyType
	{
        Month,          //ÿ��һ��
        Week,	        //ÿ��һ��
        Day,			//ÿ��һ��
        Hour,			//ÿСʱһ��
        Period,	        //ÿ����һ��
        JustInTime,      //ʵʱ���
        OnlyOnce        //ֻ��һ��
	}

    public class EFrequencyTypeUtils
	{
		public static string GetValue(EFrequencyType type)
		{
            if (type == EFrequencyType.Month)
			{
                return "Month";
            }
            else if (type == EFrequencyType.Week)
            {
                return "Week";
            }
            else if (type == EFrequencyType.Day)
			{
                return "Day";
			}
            else if (type == EFrequencyType.Hour)
			{
                return "Hour";
            }
            else if (type == EFrequencyType.Period)
            {
                return "Period";
            }
            else if (type == EFrequencyType.JustInTime)
            {
                return "JustInTime";
            }
            else if (type == EFrequencyType.OnlyOnce)
            {
                return "OnlyOnce";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EFrequencyType type)
		{
            if (type == EFrequencyType.Month)
            {
                return "ÿ��һ��";
            }
            else if (type == EFrequencyType.Week)
            {
                return "ÿ��һ��";
            }
            else if (type == EFrequencyType.Day)
			{
                return "ÿ��һ��";
			}
            else if (type == EFrequencyType.Hour)
			{
                return "ÿСʱһ��";
            }
            else if (type == EFrequencyType.Period)
            {
                return "ÿ����һ��";
            }
            else if (type == EFrequencyType.JustInTime)
            {
                return "ʵʱ���";
            }
            else if (type == EFrequencyType.OnlyOnce)
            {
                return "ִֻ��һ��";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EFrequencyType GetEnumType(string typeStr)
		{
            EFrequencyType retval = EFrequencyType.Month;

            if (Equals(EFrequencyType.Month, typeStr))
			{
                retval = EFrequencyType.Month;
            }
            else if (Equals(EFrequencyType.Week, typeStr))
            {
                retval = EFrequencyType.Week;
            }
            else if (Equals(EFrequencyType.Day, typeStr))
			{
                retval = EFrequencyType.Day;
			}
            else if (Equals(EFrequencyType.Hour, typeStr))
			{
                retval = EFrequencyType.Hour;
            }
            else if (Equals(EFrequencyType.Period, typeStr))
            {
                retval = EFrequencyType.Period;
            }
            else if (Equals(EFrequencyType.JustInTime, typeStr))
            {
                retval = EFrequencyType.JustInTime;
            }
            else if (Equals(EFrequencyType.OnlyOnce, typeStr))
            {
                retval = EFrequencyType.OnlyOnce;
            }

			return retval;
		}

		public static bool Equals(EFrequencyType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

		public static bool Equals(string typeStr, EFrequencyType type)
		{
			return Equals(type, typeStr);
		}

        public static ListItem GetListItem(EFrequencyType type, bool selected)
        {
            ListItem item = new ListItem(GetText(type), GetValue(type));
            if (selected)
            {
                item.Selected = true;
            }
            return item;
        }

        public static void AddListItems(ListControl listControl, bool withJustInTime)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(EFrequencyType.Month, false));
                listControl.Items.Add(GetListItem(EFrequencyType.Week, false));
                listControl.Items.Add(GetListItem(EFrequencyType.Day, false));
                listControl.Items.Add(GetListItem(EFrequencyType.Hour, false));
                listControl.Items.Add(GetListItem(EFrequencyType.Period, false));
                //listControl.Items.Add(GetListItem(EFrequencyType.OnlyOnce, false));
                if (withJustInTime)
                {
                    listControl.Items.Add(GetListItem(EFrequencyType.JustInTime, false));
                }
            }
        }

	}
}
