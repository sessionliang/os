using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.WeiXin.Model
{
    public enum EAppointmentStatus
	{
        Agree,     //ͬ��
        Handling,  //������
        Refuse,    //�ܾ�
        Arrive,    //�Ѹ�Լ
        DisArrive  //δ��Լ
	}

    public class EAppointmentStatusUtils
	{
        public static string GetValue(EAppointmentStatus type)
		{
            if (type == EAppointmentStatus.Agree)
            {
                return "Agree";
            }
            else if (type == EAppointmentStatus.Handling)
            {
                return "Handling";
            }
            else if (type == EAppointmentStatus.Refuse)
            {
                return "Refuse";
            }
            else if (type == EAppointmentStatus.Arrive)
            {
                return "Arrive";
            }
            else if (type == EAppointmentStatus.DisArrive)
            {
                return "DisArrive";
            }
            else
			{
				throw new Exception();
			}
		}

		public static string GetText(EAppointmentStatus type)
		{
            if (type == EAppointmentStatus.Agree)
            {
                return "ͬ��";
            }
            else if (type == EAppointmentStatus.Handling)
            {
                return "������";
            }
            else if (type == EAppointmentStatus.Refuse)
            {
                return "�ܾ�";
            }
            else if (type == EAppointmentStatus.Arrive)
            {
                return "�Ѹ�Լ";
            }
            else if (type == EAppointmentStatus.DisArrive)
            {
                return "δ��Լ";
            }
           else
			{
				throw new Exception();
			}
		}

		public static EAppointmentStatus GetEnumType(string typeStr)
		{
            EAppointmentStatus retval = EAppointmentStatus.Agree;

            if (Equals(EAppointmentStatus.Agree, typeStr))
            {
                retval = EAppointmentStatus.Agree;
            }
            else if (Equals(EAppointmentStatus.Handling, typeStr))
            {
                retval = EAppointmentStatus.Handling;
            }
            else if (Equals(EAppointmentStatus.Refuse, typeStr))
            {
                retval = EAppointmentStatus.Refuse;
            }
            else if (Equals(EAppointmentStatus.Arrive, typeStr))
            {
                retval = EAppointmentStatus.Arrive;
            }
            else if (Equals(EAppointmentStatus.DisArrive, typeStr))
            {
                retval = EAppointmentStatus.DisArrive;
            }
			return retval;
		}

		public static bool Equals(EAppointmentStatus type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EAppointmentStatus type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EAppointmentStatus type, bool selected)
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
                listControl.Items.Add(GetListItem(EAppointmentStatus.Agree, false));
                //listControl.Items.Add(GetListItem(EAppointmentStatus.Handling, false));
                listControl.Items.Add(GetListItem(EAppointmentStatus.Refuse, false));
                listControl.Items.Add(GetListItem(EAppointmentStatus.Arrive, false));
                listControl.Items.Add(GetListItem(EAppointmentStatus.DisArrive, false));
              
            }
        }
	}
}
