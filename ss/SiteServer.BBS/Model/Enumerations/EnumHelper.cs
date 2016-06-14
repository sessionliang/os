using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SiteServer.BBS.Model{

    // 枚举字段的信息
    public enum EEnumAttribute {
        [Description("枚举字段的名称")]
        EnumName,
        [Description("枚举字段的值")]
        EnumValue,
        [Description("枚举字段的描述")]
        EnumDescription
    }

    public class EnumHelper {

        private EnumHelper() {
        }
        /// <summary>
        /// 获得某个Enum类型的绑定列表
        /// </summary>
        /// <param name="enumType">枚举的类型，例如：typeof(Sex)</param>
        /// <returns>
        /// 返回一个DataTable有两列： "Text" : System.String; "Value": System.String
        /// </returns>
        public static System.Data.DataTable EnumListToTable(Type enumType) {
            return EnumListToTable(enumType, EEnumAttribute.EnumName, null, null);
        }

        /// <summary>
        /// 获得某个Enum类型的绑定列表
        /// </summary>
        /// <param name="enumType">枚举的类型，例如：typeof(Sex)</param>
        /// <param name="valueSource">datatable的Value列值的来源：EnumValue:枚举字段的值，EnumName:枚举字段的名称，默认为EnumName</param>
        /// <returns>
        /// 返回一个DataTable有两列： "Text" : System.String; "Value": System.String
        /// </returns>
        public static System.Data.DataTable EnumListToTable(Type enumType, EEnumAttribute valueSource) {
            return EnumListToTable(enumType, valueSource, null, null);
        }

        /// <summary>
        /// 获得某个Enum类型的绑定列表
        /// </summary>
        /// <param name="enumType">枚举的类型，例如：typeof(Sex)</param>
        /// <param name="headText">datatable最前面增加枚举类型以外的一列信息的文本比如 --选择全部--</param>
        /// <param name="headValue">datatable最前面增加枚举类型以外的一列信息的值比如 --""--</param>
        /// <returns>
        /// 返回一个DataTable有两列： "Text" : System.String; "Value": System.String
        /// </returns>
        public static System.Data.DataTable EnumListToTable(Type enumType, string headText, string headValue) {
            return EnumListToTable(enumType, EEnumAttribute.EnumName, headText, headValue);
        }

        /// <summary>
        /// 获得某个Enum类型的绑定列表
        /// </summary>
        /// <param name="enumType">枚举的类型，例如：typeof(Sex)</param>
        /// <param name="valueSource">datatable的Value列值的来源：枚举字段的某属性，比如值、名称，默认为EnumName</param>
        /// <param name="headText">datatable最前面增加枚举类型以外的一列信息的文本比如 --选择全部--</param>
        /// <param name="headValue">datatable最前面增加枚举类型以外的一列信息的值比如 --""--</param>
        /// <returns>
        /// 返回一个DataTable有两列： "Text" : System.String; "Value": System.String
        /// </returns>
        public static System.Data.DataTable EnumListToTable(Type enumType, EEnumAttribute valueSource, string headText, string headValue) {
            //不是枚举的要报错
            if (enumType.IsEnum != true) {
                throw new InvalidOperationException();
            }
            //建立DataTable的列信息
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Text", typeof(System.String));
            dt.Columns.Add("Value", typeof(System.String));

            // 需要添加枚举类型以外的信息 比如 --全部--选项
            if (headText != null && headText.Length > 0) {
                System.Data.DataRow drHead = dt.NewRow();
                drHead["Value"] = headValue;
                drHead["Text"] = headText;
                dt.Rows.Add(drHead);
            }

            //获得特性Description的类型信息
            Type typeDescription = typeof(System.ComponentModel.DescriptionAttribute);

            //获得枚举的字段信息（因为枚举的值实际上是一个static的字段的值）
            System.Reflection.FieldInfo[] fields = enumType.GetFields();

            //检索所有字段
            foreach (System.Reflection.FieldInfo field in fields) {
                //过滤掉一个不是枚举值的，记录的是枚举的源类型
                if (field.FieldType.IsEnum == true) {
                    System.Data.DataRow dr = dt.NewRow();

                    //获得这个字段的所有自定义特性，这里只查找Description特性 
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0) {
                        //因为Description这个自定义特性是不允许重复的，所以我们只取第一个就可以了！
                        System.ComponentModel.DescriptionAttribute aa = (System.ComponentModel.DescriptionAttribute)arr[0];
                        //获得特性的描述值，也就是‘男’‘女’等中文描述
                        dr["Text"] = aa.Description;
                    }
                    else {
                        //如果没有特性描述则显示英文的字段名
                        dr["Text"] = field.Name;
                    }
                    // 通过字段的名字得到枚举的值
                    // 枚举的值如果是long的话，ToChar会有问题，但这个不在本文的讨论范围之内
                    if (valueSource == EEnumAttribute.EnumValue) {
                        dr["Value"] = (char)(int)enumType.InvokeMember(field.Name, System.Reflection.BindingFlags.GetField, null, null, null);
                    }
                    else if (valueSource == EEnumAttribute.EnumDescription) {
                        dr["Value"] = dr["Text"];
                    }
                    // 直接把枚举字段名赋给table
                    else {
                        dr["Value"] = field.Name;
                    }

                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        /// <summary>
        /// 返回枚举类型某字段的属性
        /// </summary>
        /// <param name="enumType">枚举的类型，例如：typeof(Sex)</param>
        /// <param name="fieldName">枚举字段的名称</param>		
        /// <returns>返回该枚举字段的某属性的值</returns>
        public static string GetEnumFileAttribute(Type enumType, string fieldName) {
            return GetEnumFileAttribute(enumType, fieldName, EEnumAttribute.EnumName);
        }

        /// <summary>
        /// 返回枚举类型某字段的属性
        /// </summary>
        /// <param name="enumType">枚举的类型，例如：typeof(Sex)</param>
        /// <param name="fieldName">枚举字段的名称</param>
        /// <param name="returnType">返回该枚举字段的属性，默认为名称</param>
        /// <returns>返回该枚举字段的某属性的值</returns>
        public static string GetEnumFileAttribute(Type enumType, string fieldName, EEnumAttribute returnType) {
            string returnValue = null;

            if (enumType.IsEnum != true) {
                //不是枚举的要报错
                throw new InvalidOperationException();
            }
            //获得特性Description的类型信息
            Type typeDescription = typeof(System.ComponentModel.DescriptionAttribute);

            //获得枚举的字段信息（因为枚举的值实际上是一个static的字段的值）
            System.Reflection.FieldInfo field = enumType.GetField(fieldName);
            if (field == null)
                return null;

            if (returnType == EEnumAttribute.EnumValue) {
                returnValue = ((int)enumType.InvokeMember(field.Name, System.Reflection.BindingFlags.GetField, null, null, null)).ToString();
            }
            else if (returnType == EEnumAttribute.EnumDescription) {
                object[] arr = field.GetCustomAttributes(typeDescription, true);
                if (arr.Length > 0) {
                    //因为Description这个自定义特性是不允许重复的，所以我们只取第一个就可以了！
                    System.ComponentModel.DescriptionAttribute aa = (System.ComponentModel.DescriptionAttribute)arr[0];
                    //获得特性的描述值，也就是‘男’‘女’等中文描述
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
