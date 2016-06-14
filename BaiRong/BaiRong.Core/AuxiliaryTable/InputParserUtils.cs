using System.Collections.Specialized;
using System.Web.UI.HtmlControls;

using BaiRong.Model;
using BaiRong.Core;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BaiRong.Core
{
    public class InputParserUtils
    {
        private InputParserUtils()
        {
        }

        private static string GetValidateCheckMethod(string attributeName, string displayName, InputValidateInfo validateInfo)
        {
            if (validateInfo != null)
            {
                return string.Format("checkAttributeValue('{0}', '{1}', {2}, {3}, {4}, '{5}', '{6}');", attributeName, displayName, validateInfo.IsRequire.ToString().ToLower(), validateInfo.MinNum, validateInfo.MaxNum, validateInfo.RegExp, validateInfo.ErrorMessage);
            }
            return string.Empty;
        }

        public static string GetValidateAttributes(bool isValidate, string displayName, bool isRequire, int minNum, int maxNum, EInputValidateType validateType, string regExp, string errorMessage)
        {
            if (isValidate)
            {
                return string.Format(@"isValidate=""{0}"" displayName=""{1}"" isRequire=""{2}"" minNum=""{3}"" maxNum=""{4}"" validateType=""{5}"" regExp=""{6}"" errorMessage=""{7}""", isValidate.ToString().ToLower(), displayName, isRequire.ToString().ToLower(), minNum, maxNum, EInputValidateTypeUtils.GetValue(validateType), regExp, errorMessage);
            }
            return string.Empty;
        }

        public static void GetValidateAttributesForListItem(ListControl control, bool isValidate, string displayName, bool isRequire, int minNum, int maxNum, EInputValidateType validateType, string regExp, string errorMessage)
        {
            if (isValidate)
            {
                control.Attributes.Add("isValidate", isValidate.ToString().ToLower());
                control.Attributes.Add("displayName", displayName);
                control.Attributes.Add("isRequire", isRequire.ToString().ToLower());
                control.Attributes.Add("minNum", minNum.ToString());
                control.Attributes.Add("maxNum", maxNum.ToString());
                control.Attributes.Add("validateType", EInputValidateTypeUtils.GetValue(validateType));
                control.Attributes.Add("regExp", regExp);
                control.Attributes.Add("errorMessage", errorMessage);
                control.Attributes.Add("isListItem", true.ToString().ToLower());
            }
        }

        public static string GetValidateSubmitOnClickScript(string formId)
        {
            return string.Format("return checkFormValueById('{0}');", formId);
        }

        /// <summary>
        /// 带有提示的确认操作
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="isConfirm"></param>
        /// <returns></returns>
        public static string GetValidateSubmitOnClickScript(string formId, bool isConfirm, string confirmFunction)
        {
            if (!isConfirm)
                return GetValidateSubmitOnClickScript(formId);
            else
                return string.Format("return checkFormValueById('{0}')&&{1};", formId, confirmFunction);
        }

        public static string GetAdditionalAttributes(string whereUsed, EInputType inputType)
        {
            string additionalAttributes = string.Empty;
            if (string.IsNullOrEmpty(whereUsed))
            {
                //if (inputType == EInputType.Text || inputType == EInputType.Image || inputType == EInputType.File)
                //{
                //    additionalAttributes = @"class=""colorblur"" onfocus=""this.className='colorfocus';"" onblur=""this.className='colorblur';"" size=""60""";
                //}
                //else if (inputType == EInputType.TextArea)
                //{
                //    additionalAttributes = @"class=""colorblur"" onfocus=""this.className='colorfocus';"" onblur=""this.className='colorblur';"" cols=""60"" rows=""5""";
                //}
                //else if (inputType == EInputType.Date || inputType == EInputType.DateTime)
                //{
                //    additionalAttributes = @"class=""colorblur Wdate"" size=""25""";
                //}
            }
            else if (whereUsed == "usercenter")
            {
                if (inputType == EInputType.Text || inputType == EInputType.Image || inputType == EInputType.Video || inputType == EInputType.File)
                {
                    additionalAttributes = @"class=""input-txt"" style=""width:320px""";
                }
                else if (inputType == EInputType.TextArea)
                {
                    additionalAttributes = @"class=""input-area area-s5"" cols=""60"" rows=""5""";
                }
                else if (inputType == EInputType.Date || inputType == EInputType.DateTime)
                {
                    additionalAttributes = @"class=""input-txt Wdate"" style=""width:120px""";
                }
            }
            return additionalAttributes;
        }

        //public static string GetInnerAdditionalAttributes(EInputType inputType, EAuxiliaryTableType tableType, string attributeName)
        //{
        //    string additionalAttributes = string.Empty;
        //    if (inputType == EInputType.Default)
        //    {
        //        inputType = EInputTypeUtils.GetDefaultInputType(tableType, attributeName);
        //    }

        //    if (inputType == EInputType.Text)
        //    {
        //        additionalAttributes = @"class=""colorblur"" onfocus=""this.className='colorfocus';"" onblur=""this.className='colorblur';"" size=""60""";
        //    }
        //    else if (inputType == EInputType.TextArea)
        //    {
        //        additionalAttributes = @"class=""colorblur"" onfocus=""this.className='colorfocus';"" onblur=""this.className='colorblur';"" cols=""60"" rows=""5""";
        //    }
        //    else if (inputType == EInputType.Date || inputType == EInputType.DateTime)
        //    {
        //        additionalAttributes = @"class=""colorblur"" size=""30""";
        //    }
        //    else if (inputType == EInputType.Image || inputType == EInputType.File)
        //    {
        //        additionalAttributes = @"size=""50""";
        //    }
        //    return additionalAttributes;
        //}
    }
}
