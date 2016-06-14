using System.Text;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Model;
using System.Web;
using BaiRong.Core.Cryptography;
using SiteServer.STL.Parser.StlElement;
using System.Collections;
using SiteServer.STL.Parser;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Xml;
using SiteServer.CMS.Core;

namespace SiteServer.STL.StlTemplate
{
    public class InputTemplateBase
    {
        protected InputTemplateBase() { }

        public string GetInnerAdditionalAttributes(TableStyleInfo styleInfo)
        {
            string additionalAttributes = string.Empty;

            EInputType inputType = styleInfo.InputType;

            if (inputType == EInputType.Text || inputType == EInputType.Date || inputType == EInputType.DateTime)
            {
                additionalAttributes = @"class=""is_text""";
            }
            else if (inputType == EInputType.Image || inputType == EInputType.Video || inputType == EInputType.File)
            {
                additionalAttributes = @"class=""is_upload""";
            }
            else if (inputType == EInputType.TextArea)
            {
                additionalAttributes = @"class=""is_textarea""";
            }

            return additionalAttributes;
        }

        public string GetStyle(ETableStyle tableStyle)
        {
            int width = 260;
            if (tableStyle == ETableStyle.BackgroundContent)
            {
                width = 320;
            }
            else if (tableStyle == ETableStyle.WebsiteMessageContent)
            {
                return string.Format(@"
/*浮层*/
.tiwen_fc{{width: 100%;height: 100%;position: fixed;top: 0;left: 0;/*display: none;*/padding: 0;}}
.tiwen_bg{{position: absolute;top: 0;left: 0;width: 100%;height: 100%;background: #000;filter: alpha(opacity=70);opacity: .7;}}
.tiwen_fc_c{{width: 285px;/*height: 515px;*/position: absolute;top: 50%;left: 50%;margin: -277px -162px;background: #fff;padding: 40px 20px 0;}}
.tiwen_fc_c .close{{width: 17px;height: 17px;background: url({0}/close.png) no-repeat;position: absolute;top: 5px;right: 5px;cursor: pointer;}}
.tiwen_fc_c .leixing{{overflow: hidden;}}
.tiwen_fc_c .leixing{{width: 100%;height: auto;margin-bottom: 10px;}}
.tiwen_fc_c .leixing p{{font-size: 14px;color: #666666;margin-bottom: 2px;}}
.tiwen_fc_c .leixing input[type='text']{{width: 278px;height: 33px;border: 1px solid #d6dddf;border-radius: 5px;padding-left: 5px;padding: 0;margin-bottom: 0; }}
.tiwen_fc_c .leixing input[type='text'].buchong{{height: 70px;}}
.tiwen_fc_c .leixing ul li{{float: left;width: 88px;border: 1px solid #e4e8ec;border-radius: 5px;height: 38px;line-height: 38px;text-align: center;margin-left: 5px;margin-bottom: 10px;font-size: 14px;cursor: pointer;}}
.tiwen_fc_c .leixing ul li.current{{border-color: #0a67e1;background: url({0}/jiaobiao.jpg) no-repeat bottom right;}}
.tiwen_fc_c .leixing ul li input[type='radio']{{display:none;}}
.tiwen_fc_c .leixing ul li.Nomargin{{margin-left: 0;}}
.tiwen_fc_c .leixing ul{{float: left;margin: 0;padding: 0;list-style: none;border: 0;font-family: '微软雅黑';}}
.tiwen_fc_c input[type='submit']{{display: block;width: 120px;height: 35px;line-height: 35px;text-align: center;background: #3579d6;color: #fff;margin: 10px auto 0;}}
", PageUtility.Services.GetImageUrl(string.Empty));
            }

            return string.Format(@"
.is_text {{ border:#cfd8e1 1px solid;background-color:#fff;width:{0}px; height:18px; line-height:18px; vertical-align:middle }}
.is_upload {{ border:#cfd8e1 1px solid; width:{1}px; }}
.is_textarea {{ border:#cfd8e1 1px solid;background-color:#fff;width:320px; height:90px }}
.is_btn {{ line-height: 16px }}
.is_success{{ margin: 0 auto; font: 14px Arial, Helvetica, sans-serif; color: #090 !important; padding: 10px 10px 10px 45px; width: 90%; background: url({2}/success.gif) no-repeat left center; text-align: left; line-height: 160%; font-weight: bold; }}
.is_failure{{ margin: 0 auto; font: 14px Arial, Helvetica, sans-serif; color:#CC0000 !important; padding: 10px 10px 10px 45px; width: 90%; background: url({2}/failure.gif) no-repeat left center; text-align: left; line-height: 160%; font-weight: bold; }}
", width, (width + 68), PageUtility.Services.GetImageUrl(string.Empty));
        }

        public void ReWriteCheckCode(StringBuilder builder, PublishmentSystemInfo publishmentSystemInfo, VCManager vcManager, bool isCrossDomain, bool isCorsCross)
        {
            RegexOptions options = ((RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline) | RegexOptions.IgnoreCase);
            string element = null;

            string imageUrl = string.Empty;

            TableStyleInfo styleInfo = new TableStyleInfo();
            styleInfo.Additional.IsValidate = true;
            styleInfo.Additional.IsRequired = true;
            styleInfo.Additional.MinNum = 4;
            styleInfo.Additional.MaxNum = 4;
            styleInfo.AttributeName = ValidateCodeManager.AttributeName;
            styleInfo.DisplayName = "验证码";
            styleInfo.Additional.Width = "50px";
            styleInfo.Additional.ValidateType = EInputValidateType.None;
            string validateAttributes = string.Empty;
            string validateHtmlString = InputTypeParser.GetValidateHtmlString(styleInfo, out validateAttributes);

            string regex = string.Format("<input\\s*[^>]*?id\\s*=\\s*(\"{0}\"|\'{0}\'|{0}).*?>", ValidateCodeManager.AttributeName);
            Regex reg = new Regex(regex, options);
            Match match = reg.Match(builder.ToString());
            if (match.Success)
            {
                if (!isCorsCross)//兼容老版
                {
                    imageUrl = vcManager.GetImageUrl(false);
                    if (isCrossDomain)
                    {
                        imageUrl = PageUtils.GetRootUrl(imageUrl);
                    }
                }
                else//cors validate
                {
                    imageUrl = vcManager.GetImageUrl(false, isCorsCross, PageUtility.IsCorsCrossDomain(publishmentSystemInfo) ? publishmentSystemInfo.Additional.APIUrl : string.Empty);
                }

                element = match.Value;
                XmlDocument document = StlParserUtility.GetXmlDocument(element, false);
                XmlNode inputNode = document.DocumentElement;
                inputNode = inputNode.FirstChild;
                IEnumerator inputIE = inputNode.Attributes.GetEnumerator();
                StringDictionary attributes = new StringDictionary();
                while (inputIE.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)inputIE.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals("id"))
                    {
                        attributes.Add("id", ValidateCodeManager.AttributeName);
                        attributes.Add("name", ValidateCodeManager.AttributeName);
                    }
                    else if (!attributeName.Equals("name"))
                    {
                        attributes.Add(attr.Name, attr.Value);
                    }
                }
                //string inputReplace = string.Format(@"<input {0} {1}/>&nbsp;<img border=""0"" align=""absmiddle"" src=""{2}""/>{3}", TranslateUtils.ToAttributesString(attributes), validateAttributes, imageUrl, validateHtmlString);
                //ys修改，点击验证码刷新
                string inputReplace = string.Format(@"<input {0} {1}/>&nbsp;<img border=""0"" align=""absmiddle"" src=""{2}""  onclick=""this.src='{3}?id='+Math.random()"" style=""cursor:pointer;""/>{4}", TranslateUtils.ToAttributesString(attributes), validateAttributes, imageUrl, imageUrl, validateHtmlString);

                builder.Replace(element, inputReplace);
            }
        }

        protected string GetAttributesHtml(NameValueCollection pageScripts, PublishmentSystemInfo publishmentSystemInfo, bool isValidateCode, ArrayList styleInfoArrayList)
        {
            StringBuilder output = new StringBuilder();

            if (isValidateCode)
            {
                isValidateCode = FileConfigManager.Instance.IsValidateCode;
            }

            if (styleInfoArrayList != null)
            {
                foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                {
                    if (styleInfo.IsVisible == false) continue;

                    string helpHtml = styleInfo.DisplayName + ":";
                    NameValueCollection formCollection = new NameValueCollection();
                    formCollection[styleInfo.AttributeName] = string.Empty;
                    string inputHtml = InputTypeParser.Parse(publishmentSystemInfo, 0, styleInfo, ETableStyle.InputContent, styleInfo.AttributeName, formCollection, false, false, this.GetInnerAdditionalAttributes(styleInfo), pageScripts, false, true);
                    output.AppendFormat(@"
<tr><td width=""70""><nobr>{0}</nobr></td><td>{1}</td></tr>
", helpHtml, inputHtml);
                }

                if (isValidateCode)
                {
                    TableStyleInfo styleInfo = new TableStyleInfo();
                    styleInfo.Additional.IsValidate = false;
                    styleInfo.AttributeName = ValidateCodeManager.AttributeName;
                    styleInfo.DisplayName = "验证码";
                    styleInfo.Additional.Width = "50px";
                    string inputHtml = InputTypeParser.ParseText(publishmentSystemInfo, 0, styleInfo.AttributeName, null, true, this.GetInnerAdditionalAttributes(styleInfo), styleInfo, ETableStyle.InputContent, false);

                    output.AppendFormat(@"
<tr><td>验证码:</td><td>{0}</td></tr>
", inputHtml);
                }
            }
            return output.ToString();
        }
    }
}
