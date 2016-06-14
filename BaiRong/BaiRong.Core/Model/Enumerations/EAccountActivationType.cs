using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model
{
	public enum EAccountActivationType
	{
		Automatic,
		Email,
		AdminApproval
	}

	public class EAccountActivationTypeUtils
	{
		public static string GetValue(EAccountActivationType type)
		{
			if (type == EAccountActivationType.Automatic)
			{
				return "Automatic";
			}
			else if (type == EAccountActivationType.Email)
			{
				return "Email";
			}
			else if (type == EAccountActivationType.AdminApproval)
			{
				return "AdminApproval";
			}
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EAccountActivationType type)
		{
			if (type == EAccountActivationType.Automatic)
			{
				return "�Զ�����";
			}
			else if (type == EAccountActivationType.Email)
			{
				return "�����ʼ�����";
			}
			else if (type == EAccountActivationType.AdminApproval)
			{
				return "����Ա����";
			}
			else
			{
				throw new Exception();
			}
		}

		public static bool Equals(EAccountActivationType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EAccountActivationType type)
        {
            return Equals(type, typeStr);
        }

		public static EAccountActivationType GetEnumType(string typeStr)
		{
			EAccountActivationType retval = EAccountActivationType.AdminApproval;

			if (Equals(EAccountActivationType.Automatic, typeStr))
			{
				retval = EAccountActivationType.Automatic;
			}
			else if (Equals(EAccountActivationType.Email, typeStr))
			{
				retval = EAccountActivationType.Email;
			}
			else if (Equals(EAccountActivationType.AdminApproval, typeStr))
			{
				retval = EAccountActivationType.AdminApproval;
			}

			return retval;
		}

	}
}
