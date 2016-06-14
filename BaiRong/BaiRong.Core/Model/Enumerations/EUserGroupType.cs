using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;

namespace BaiRong.Model
{
    public enum EUserGroupType
    {
        Credits,
        Specials,
        Administrator,          //����Ա
        SuperModerator,         //��������
        Moderator,              //��̳����
        WriteForbidden,         //��ֹ����
        ReadForbidden,          //��ֹ����
        Guest                   //�ο�
    }

    public class EUserGroupTypeUtils
    {
        public static string GetValue(EUserGroupType type)
        {
            if (type == EUserGroupType.Credits)
            {
                return "Credits";
            }
            else if (type == EUserGroupType.Specials)
            {
                return "Specials";
            }
            else if (type == EUserGroupType.Administrator)
            {
                return "Administrator";
            }
            else if (type == EUserGroupType.SuperModerator)
            {
                return "SuperModerator";
            }
            else if (type == EUserGroupType.Moderator)
            {
                return "Moderator";
            }
            else if (type == EUserGroupType.WriteForbidden)
            {
                return "WriteForbidden";
            }
            else if (type == EUserGroupType.ReadForbidden)
            {
                return "ReadForbidden";
            }
            else if (type == EUserGroupType.Guest)
            {
                return "Guest";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EUserGroupType type)
        {
            if (type == EUserGroupType.Credits)
            {
                return "�����û���";
            }
            else if (type == EUserGroupType.Specials)
            {
                return "�����û���";
            }
            else if (type == EUserGroupType.Administrator)
            {
                return "����Ա";
            }
            else if (type == EUserGroupType.SuperModerator)
            {
                return "��������";
            }
            else if (type == EUserGroupType.Moderator)
            {
                return "��̳����";
            }
            else if (type == EUserGroupType.WriteForbidden)
            {
                return "��ֹ����";
            }
            else if (type == EUserGroupType.ReadForbidden)
            {
                return "��ֹ����";
            }
            else if (type == EUserGroupType.Guest)
            {
                return "�ο�";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EUserGroupType GetEnumType(string typeStr)
        {
            EUserGroupType retval = EUserGroupType.Credits;

            if (Equals(EUserGroupType.Credits, typeStr))
            {
                retval = EUserGroupType.Credits;
            }
            else if (Equals(EUserGroupType.Specials, typeStr))
            {
                retval = EUserGroupType.Specials;
            }
            else if (Equals(EUserGroupType.Administrator, typeStr))
            {
                retval = EUserGroupType.Administrator;
            }
            else if (Equals(EUserGroupType.SuperModerator, typeStr))
            {
                retval = EUserGroupType.SuperModerator;
            }
            else if (Equals(EUserGroupType.Moderator, typeStr))
            {
                retval = EUserGroupType.Moderator;
            }
            else if (Equals(EUserGroupType.WriteForbidden, typeStr))
            {
                retval = EUserGroupType.WriteForbidden;
            }
            else if (Equals(EUserGroupType.ReadForbidden, typeStr))
            {
                retval = EUserGroupType.ReadForbidden;
            }
            else if (Equals(EUserGroupType.Guest, typeStr))
            {
                retval = EUserGroupType.Guest;
            }

            return retval;
        }

        public static bool Equals(EUserGroupType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EUserGroupType type)
        {
            return Equals(type, typeStr);
        }
    }
}
