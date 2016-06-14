using System;
using System.Web.UI.WebControls;

using BaiRong.Core;
using System.Collections.Specialized;
using System.Text;

namespace BaiRong.Model
{
	public enum EInputValidateType
	{
        None,               //��
        Chinese,			//����
        English,	        //Ӣ��
        Email,				//Email��ʽ
        Url,				//��ַ��ʽ
        Phone,				//�绰���� 
        Mobile,				//�ֻ�����
        Integer,			//����
        Currency,			//���Ҹ�ʽ
        Zip,				//��������
        IdCard,				//���֤����
        QQ,				    //QQ����
        Custom,				//�Զ���������ʽ��֤
	}

	public class EInputValidateTypeUtils
	{
		public static string GetValue(EInputValidateType type)
		{
            if (type == EInputValidateType.None)
			{
                return "None";
            }
            else if (type == EInputValidateType.Chinese)
            {
                return "Chinese";
            }
            else if (type == EInputValidateType.English)
			{
                return "English";
			}
            else if (type == EInputValidateType.Email)
			{
                return "Email";
            }
            else if (type == EInputValidateType.Url)
            {
                return "Url";
            }
            else if (type == EInputValidateType.Phone)
            {
                return "Phone";
            }
            else if (type == EInputValidateType.Mobile)
            {
                return "Mobile";
            }
            else if (type == EInputValidateType.Integer)
            {
                return "Integer";
            }
            else if (type == EInputValidateType.Currency)
            {
                return "Currency";
            }
            else if (type == EInputValidateType.Zip)
            {
                return "Zip";
            }
            else if (type == EInputValidateType.IdCard)
            {
                return "IdCard";
            }
            else if (type == EInputValidateType.QQ)
            {
                return "QQ";
            }
            else if (type == EInputValidateType.Custom)
            {
                return "Custom";
            }
			else
			{
				throw new Exception();
			}
		}

        public static string GetText(EInputValidateType type)
		{
            if (type == EInputValidateType.None)
            {
                return "��";
            }
            else if (type == EInputValidateType.Chinese)
            {
                return "����";
            }
            else if (type == EInputValidateType.English)
            {
                return "Ӣ��";
            }
            else if (type == EInputValidateType.Email)
            {
                return "Email��ʽ";
            }
            else if (type == EInputValidateType.Url)
            {
                return "��ַ��ʽ";
            }
            else if (type == EInputValidateType.Phone)
            {
                return "�绰����";
            }
            else if (type == EInputValidateType.Mobile)
            {
                return "�ֻ�����";
            }
            else if (type == EInputValidateType.Integer)
            {
                return "����";
            }
            else if (type == EInputValidateType.Currency)
            {
                return "���Ҹ�ʽ";
            }
            else if (type == EInputValidateType.Zip)
            {
                return "��������";
            }
            else if (type == EInputValidateType.IdCard)
            {
                return "���֤����";
            }
            else if (type == EInputValidateType.QQ)
            {
                return "QQ����";
            }
            else if (type == EInputValidateType.Custom)
            {
                return "�Զ���������ʽ��֤";
            }
            else
            {
                throw new Exception();
            }
		}

        public static string GetValidateInfo(TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();
            if (styleInfo.Additional.IsRequired)
            {
                builder.Append("������;");
            }
            if (styleInfo.Additional.MinNum > 0)
            {
                builder.AppendFormat("����{0}���ַ�;", styleInfo.Additional.MinNum);
            }
            if (styleInfo.Additional.MaxNum > 0)
            {
                builder.AppendFormat("���{0}���ַ�;", styleInfo.Additional.MaxNum);
            }
            if (styleInfo.Additional.ValidateType != EInputValidateType.None)
            {
                builder.AppendFormat("��֤:{0};", EInputValidateTypeUtils.GetText(styleInfo.Additional.ValidateType));
            }

            if (builder.Length > 0)
            {
                builder.Length = builder.Length - 1;
            }
            else
            {
                builder.Append("����֤");
            }
            return builder.ToString();
        }

		public static EInputValidateType GetEnumType(string typeStr)
		{
			EInputValidateType retval = EInputValidateType.None;

			if (Equals(EInputValidateType.None, typeStr))
			{
                retval = EInputValidateType.None;
            }
            else if (Equals(EInputValidateType.Chinese, typeStr))
            {
                retval = EInputValidateType.Chinese;
            }
            else if (Equals(EInputValidateType.Currency, typeStr))
            {
                retval = EInputValidateType.Currency;
            }
			else if (Equals(EInputValidateType.Custom, typeStr))
			{
                retval = EInputValidateType.Custom;
            }
            else if (Equals(EInputValidateType.Email, typeStr))
            {
                retval = EInputValidateType.Email;
            }
            else if (Equals(EInputValidateType.English, typeStr))
            {
                retval = EInputValidateType.English;
            }
            else if (Equals(EInputValidateType.IdCard, typeStr))
            {
                retval = EInputValidateType.IdCard;
            }
            else if (Equals(EInputValidateType.Integer, typeStr))
            {
                retval = EInputValidateType.Integer;
            }
            else if (Equals(EInputValidateType.Mobile, typeStr))
            {
                retval = EInputValidateType.Mobile;
            }
            else if (Equals(EInputValidateType.Phone, typeStr))
            {
                retval = EInputValidateType.Phone;
            }
            else if (Equals(EInputValidateType.QQ, typeStr))
            {
                retval = EInputValidateType.QQ;
            }
            else if (Equals(EInputValidateType.Url, typeStr))
            {
                retval = EInputValidateType.Url;
            }
            else if (Equals(EInputValidateType.Zip, typeStr))
            {
                retval = EInputValidateType.Zip;
            }

			return retval;
		}

		public static bool Equals(EInputValidateType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EInputValidateType type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(EInputValidateType type, bool selected)
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
                listControl.Items.Add(GetListItem(EInputValidateType.None, false));
                listControl.Items.Add(GetListItem(EInputValidateType.Chinese, false));
                listControl.Items.Add(GetListItem(EInputValidateType.English, false));
                listControl.Items.Add(GetListItem(EInputValidateType.Email, false));
                listControl.Items.Add(GetListItem(EInputValidateType.Url, false));
                listControl.Items.Add(GetListItem(EInputValidateType.Phone, false));
                listControl.Items.Add(GetListItem(EInputValidateType.Mobile, false));
                listControl.Items.Add(GetListItem(EInputValidateType.Integer, false));
                listControl.Items.Add(GetListItem(EInputValidateType.Currency, false));
                listControl.Items.Add(GetListItem(EInputValidateType.Zip, false));
                listControl.Items.Add(GetListItem(EInputValidateType.IdCard, false));
                listControl.Items.Add(GetListItem(EInputValidateType.QQ, false));
                listControl.Items.Add(GetListItem(EInputValidateType.Custom, false));
            }
        }
	}
}
