using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum EAdvRotateType
    {
        Equality,         //����
        HandWeight,       //�ֶ�Ȩ��
        SlideRotate       //�õ�Ƭ�ֻ�
    }

    public class EAdvRotateTypeUtils
    {
        public static string GetValue(EAdvRotateType type)
        {
            if (type == EAdvRotateType.Equality)
            {
                return "Equality";
            }
            else if (type == EAdvRotateType.HandWeight)
            {
                return "HandWeight";
            }
            else if (type == EAdvRotateType.SlideRotate)
            {
                return "SlideRotate";
            }
           else
            {
                throw new Exception();
            }
        }

        public static string GetText(EAdvRotateType type)
        {
            if (type == EAdvRotateType.Equality)
            {
                return "����";
            } 
            else if (type == EAdvRotateType.HandWeight)
            {
                return "�ֶ�Ȩ��";
            }
            else if (type == EAdvRotateType.SlideRotate)
            {
                return "�õ�Ƭ�ֻ�";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EAdvRotateType GetEnumType(string typeStr)
        {
            EAdvRotateType retval = EAdvRotateType.Equality;

            if (Equals(EAdvRotateType.Equality, typeStr))
            {
                retval = EAdvRotateType.Equality;
            }
            else if (Equals(EAdvRotateType.HandWeight, typeStr))
            {
                retval = EAdvRotateType.HandWeight;
            }
            else if (Equals(EAdvRotateType.SlideRotate, typeStr))
            {
                retval = EAdvRotateType.SlideRotate;
            }
            return retval;
        }

        public static bool Equals(EAdvRotateType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EAdvRotateType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EAdvRotateType type, bool selected)
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
                listControl.Items.Add(GetListItem(EAdvRotateType.Equality, false));
                listControl.Items.Add(GetListItem(EAdvRotateType.HandWeight, false));
                listControl.Items.Add(GetListItem(EAdvRotateType.SlideRotate, false));
            }
        }
    }
}
