using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SiteServer.BBS.Model{

    // ö���ֶε���Ϣ
    public enum EEnumAttribute {
        [Description("ö���ֶε�����")]
        EnumName,
        [Description("ö���ֶε�ֵ")]
        EnumValue,
        [Description("ö���ֶε�����")]
        EnumDescription
    }

    public class EnumHelper {

        private EnumHelper() {
        }
        /// <summary>
        /// ���ĳ��Enum���͵İ��б�
        /// </summary>
        /// <param name="enumType">ö�ٵ����ͣ����磺typeof(Sex)</param>
        /// <returns>
        /// ����һ��DataTable�����У� "Text" : System.String; "Value": System.String
        /// </returns>
        public static System.Data.DataTable EnumListToTable(Type enumType) {
            return EnumListToTable(enumType, EEnumAttribute.EnumName, null, null);
        }

        /// <summary>
        /// ���ĳ��Enum���͵İ��б�
        /// </summary>
        /// <param name="enumType">ö�ٵ����ͣ����磺typeof(Sex)</param>
        /// <param name="valueSource">datatable��Value��ֵ����Դ��EnumValue:ö���ֶε�ֵ��EnumName:ö���ֶε����ƣ�Ĭ��ΪEnumName</param>
        /// <returns>
        /// ����һ��DataTable�����У� "Text" : System.String; "Value": System.String
        /// </returns>
        public static System.Data.DataTable EnumListToTable(Type enumType, EEnumAttribute valueSource) {
            return EnumListToTable(enumType, valueSource, null, null);
        }

        /// <summary>
        /// ���ĳ��Enum���͵İ��б�
        /// </summary>
        /// <param name="enumType">ö�ٵ����ͣ����磺typeof(Sex)</param>
        /// <param name="headText">datatable��ǰ������ö�����������һ����Ϣ���ı����� --ѡ��ȫ��--</param>
        /// <param name="headValue">datatable��ǰ������ö�����������һ����Ϣ��ֵ���� --""--</param>
        /// <returns>
        /// ����һ��DataTable�����У� "Text" : System.String; "Value": System.String
        /// </returns>
        public static System.Data.DataTable EnumListToTable(Type enumType, string headText, string headValue) {
            return EnumListToTable(enumType, EEnumAttribute.EnumName, headText, headValue);
        }

        /// <summary>
        /// ���ĳ��Enum���͵İ��б�
        /// </summary>
        /// <param name="enumType">ö�ٵ����ͣ����磺typeof(Sex)</param>
        /// <param name="valueSource">datatable��Value��ֵ����Դ��ö���ֶε�ĳ���ԣ�����ֵ�����ƣ�Ĭ��ΪEnumName</param>
        /// <param name="headText">datatable��ǰ������ö�����������һ����Ϣ���ı����� --ѡ��ȫ��--</param>
        /// <param name="headValue">datatable��ǰ������ö�����������һ����Ϣ��ֵ���� --""--</param>
        /// <returns>
        /// ����һ��DataTable�����У� "Text" : System.String; "Value": System.String
        /// </returns>
        public static System.Data.DataTable EnumListToTable(Type enumType, EEnumAttribute valueSource, string headText, string headValue) {
            //����ö�ٵ�Ҫ����
            if (enumType.IsEnum != true) {
                throw new InvalidOperationException();
            }
            //����DataTable������Ϣ
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Text", typeof(System.String));
            dt.Columns.Add("Value", typeof(System.String));

            // ��Ҫ���ö�������������Ϣ ���� --ȫ��--ѡ��
            if (headText != null && headText.Length > 0) {
                System.Data.DataRow drHead = dt.NewRow();
                drHead["Value"] = headValue;
                drHead["Text"] = headText;
                dt.Rows.Add(drHead);
            }

            //�������Description��������Ϣ
            Type typeDescription = typeof(System.ComponentModel.DescriptionAttribute);

            //���ö�ٵ��ֶ���Ϣ����Ϊö�ٵ�ֵʵ������һ��static���ֶε�ֵ��
            System.Reflection.FieldInfo[] fields = enumType.GetFields();

            //���������ֶ�
            foreach (System.Reflection.FieldInfo field in fields) {
                //���˵�һ������ö��ֵ�ģ���¼����ö�ٵ�Դ����
                if (field.FieldType.IsEnum == true) {
                    System.Data.DataRow dr = dt.NewRow();

                    //�������ֶε������Զ������ԣ�����ֻ����Description���� 
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0) {
                        //��ΪDescription����Զ��������ǲ������ظ��ģ���������ֻȡ��һ���Ϳ����ˣ�
                        System.ComponentModel.DescriptionAttribute aa = (System.ComponentModel.DescriptionAttribute)arr[0];
                        //������Ե�����ֵ��Ҳ���ǡ��С���Ů������������
                        dr["Text"] = aa.Description;
                    }
                    else {
                        //���û��������������ʾӢ�ĵ��ֶ���
                        dr["Text"] = field.Name;
                    }
                    // ͨ���ֶε����ֵõ�ö�ٵ�ֵ
                    // ö�ٵ�ֵ�����long�Ļ���ToChar�������⣬��������ڱ��ĵ����۷�Χ֮��
                    if (valueSource == EEnumAttribute.EnumValue) {
                        dr["Value"] = (char)(int)enumType.InvokeMember(field.Name, System.Reflection.BindingFlags.GetField, null, null, null);
                    }
                    else if (valueSource == EEnumAttribute.EnumDescription) {
                        dr["Value"] = dr["Text"];
                    }
                    // ֱ�Ӱ�ö���ֶ�������table
                    else {
                        dr["Value"] = field.Name;
                    }

                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        /// <summary>
        /// ����ö������ĳ�ֶε�����
        /// </summary>
        /// <param name="enumType">ö�ٵ����ͣ����磺typeof(Sex)</param>
        /// <param name="fieldName">ö���ֶε�����</param>		
        /// <returns>���ظ�ö���ֶε�ĳ���Ե�ֵ</returns>
        public static string GetEnumFileAttribute(Type enumType, string fieldName) {
            return GetEnumFileAttribute(enumType, fieldName, EEnumAttribute.EnumName);
        }

        /// <summary>
        /// ����ö������ĳ�ֶε�����
        /// </summary>
        /// <param name="enumType">ö�ٵ����ͣ����磺typeof(Sex)</param>
        /// <param name="fieldName">ö���ֶε�����</param>
        /// <param name="returnType">���ظ�ö���ֶε����ԣ�Ĭ��Ϊ����</param>
        /// <returns>���ظ�ö���ֶε�ĳ���Ե�ֵ</returns>
        public static string GetEnumFileAttribute(Type enumType, string fieldName, EEnumAttribute returnType) {
            string returnValue = null;

            if (enumType.IsEnum != true) {
                //����ö�ٵ�Ҫ����
                throw new InvalidOperationException();
            }
            //�������Description��������Ϣ
            Type typeDescription = typeof(System.ComponentModel.DescriptionAttribute);

            //���ö�ٵ��ֶ���Ϣ����Ϊö�ٵ�ֵʵ������һ��static���ֶε�ֵ��
            System.Reflection.FieldInfo field = enumType.GetField(fieldName);
            if (field == null)
                return null;

            if (returnType == EEnumAttribute.EnumValue) {
                returnValue = ((int)enumType.InvokeMember(field.Name, System.Reflection.BindingFlags.GetField, null, null, null)).ToString();
            }
            else if (returnType == EEnumAttribute.EnumDescription) {
                object[] arr = field.GetCustomAttributes(typeDescription, true);
                if (arr.Length > 0) {
                    //��ΪDescription����Զ��������ǲ������ظ��ģ���������ֻȡ��һ���Ϳ����ˣ�
                    System.ComponentModel.DescriptionAttribute aa = (System.ComponentModel.DescriptionAttribute)arr[0];
                    //������Ե�����ֵ��Ҳ���ǡ��С���Ů������������
                    returnValue = aa.Description;
                }
            }
            else {
                returnValue = field.Name;
            }

            return returnValue;
        }

        public static object ToEnum(Type enumType, string value, object defaultType) {
            
            object retval;
            try {
                retval = Enum.Parse(enumType, value, true);
            }
            catch {
                retval = defaultType;
            }
            return retval;
        }
    }
}
