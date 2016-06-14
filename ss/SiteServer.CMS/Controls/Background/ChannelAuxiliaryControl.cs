using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core;
using System.Text;

namespace SiteServer.CMS.Controls
{

	// 通过栏目辅助表生成的栏目辅助属性控件
	public class ChannelAuxiliaryControl : Control
	{
        private NameValueCollection formCollection;
        private bool isEdit;
        private bool isPostBack;
        private ArrayList displayAttributes;//null代表不限制
        Hashtable inputTypeWithFormatStringHashtable = new Hashtable();

        public void SetParameters(NameValueCollection formCollection, bool isEdit, bool isPostBack)
        {
            this.formCollection = formCollection;
            this.isEdit = isEdit;
            this.isPostBack = isPostBack;
            this.displayAttributes = null;
        }

        public void SetParameters(NameValueCollection formCollection, bool isEdit, bool isPostBack, ArrayList displayAttributes)
        {
            this.formCollection = formCollection;
            this.isEdit = isEdit;
            this.isPostBack = isPostBack;
            this.displayAttributes = displayAttributes;
        }

		protected override void Render(HtmlTextWriter output)
		{
            int nodeID = int.Parse(HttpContext.Current.Request.QueryString["NodeID"]);
            int publishmentSystemID = int.Parse(HttpContext.Current.Request.QueryString["PublishmentSystemID"]);
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            if (this.formCollection == null)
            {
                if (HttpContext.Current.Request.Form != null && HttpContext.Current.Request.Form.Count > 0)
                {
                    this.formCollection = HttpContext.Current.Request.Form;
                }
                else
                {
                    this.formCollection = new NameValueCollection();
                }
            }

            ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemID, nodeID);
            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.Channel, DataProvider.NodeDAO.TableName, relatedIdentities);

            if (styleInfoArrayList != null)
            {
                StringBuilder builder = new StringBuilder();
                NameValueCollection pageScripts = new NameValueCollection();
                foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                {
                    if (styleInfo.IsVisible)
                    {
                        string attributes = InputParserUtils.GetAdditionalAttributes(string.Empty, styleInfo.InputType);
                        //string inputHtml = TableInputParser.Parse(styleInfo, styleInfo.AttributeName, this.formCollection, this.isEdit, isPostBack, attributes, pageScripts);
                        string inputHtml = InputTypeParser.Parse(publishmentSystemInfo, nodeID, styleInfo, ETableStyle.Channel, styleInfo.AttributeName, this.formCollection, this.isEdit, isPostBack, attributes, pageScripts, true, true);

                        builder.AppendFormat(this.GetFormatString(styleInfo.InputType), styleInfo.DisplayName, inputHtml, styleInfo.HelpText);
                    }
                }

                output.Write(builder.ToString());

                foreach (string key in pageScripts.Keys)
                {
                    output.Write(pageScripts[key]);
                }

                //foreach (string key in pageScripts.Keys)
                //{
                //    base.Page.RegisterStartupScript(key, pageScripts[key]);
                //}
            }

            //ArrayList metadataArrayList = TableManager.GetTableMetadataInfoArrayList(tableName);

//            if (metadataArrayList != null)
//            {
//                StringBuilder builder = new StringBuilder();
//                NameValueCollection pageScripts = new NameValueCollection();
//                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemID, nodeID);
//                ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.Channel, publishmentSystemInfo.AuxiliaryTableForChannel, relatedIdentities);
//                foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
//                {
//                    if (displayAttributes != null && !displayAttributes.Contains(styleInfo.AttributeName)) continue;

//                    if (styleInfo.IsVisible == EBoolean.False) continue;

//                    string helpHtml = StringUtility.GetHelpHtml(styleInfo.DisplayName, styleInfo.HelpText);
//                    string inputHtml = InputTypeParser.Parse(publishmentSystemInfo, styleInfo, ETableStyle.Channel, styleInfo.AttributeName, this.formCollection, this.isEdit, isPostBack, InputParserUtils.GetAdditionalAttributes(string.Empty, styleInfo.InputType), pageScripts, true, true);

//                    if (styleInfo.InputType == EInputType.TextEditor)
//                    {
//                        builder.AppendFormat(@"
//<tr><td colspan=""4"" align=""left"">{0}</td></tr>
//<tr><td colspan=""4"" align=""left"">{1}</td></tr>
//", helpHtml, inputHtml);
//                    }
//                    else
//                    {
//                        builder.AppendFormat(@"
//<tr><td>{0}</td><td colspan=""3"">{1}</td></tr>
//", helpHtml, inputHtml);
//                    }
//                }

//                output.Write(builder.ToString());

//                foreach (string key in pageScripts.Keys)
//                {
//                    output.Write(pageScripts[key]);
//                }
//            }
		}

        public string FormatTextEditor
        {
            get
            {
                string formatString = inputTypeWithFormatStringHashtable[EInputTypeUtils.GetValue(EInputType.TextEditor)] as string;
                if (string.IsNullOrEmpty(formatString))
                {
                    formatString = @"
<tr><td height=""25"" colspan=""2"">{0}：</td></tr>
<tr><td height=""25"" colspan=""2"">{1} {2}</td></tr>
";
                }
                return formatString;
            }
            set
            {
                inputTypeWithFormatStringHashtable[EInputTypeUtils.GetValue(EInputType.TextEditor)] = value;
            }
        }

        public string FormatImage
        {
            get
            {
                string formatString = inputTypeWithFormatStringHashtable[EInputTypeUtils.GetValue(EInputType.Image)] as string;
                if (string.IsNullOrEmpty(formatString))
                {
                    formatString = @"
<tr height=""80"" valign=""middle""><td>{0}：</td><td>{1} {2}</td></tr>
";
                }
                return formatString;
            }
            set
            {
                inputTypeWithFormatStringHashtable[EInputTypeUtils.GetValue(EInputType.Image)] = value;
            }
        }

        public string FormatDefault
        {
            get
            {
                string formatString = inputTypeWithFormatStringHashtable[EInputTypeUtils.GetValue(EInputType.Text)] as string;
                if (string.IsNullOrEmpty(formatString))
                {
                    formatString = @"
<tr><td height=""25"">{0}：</td><td height=""25"">{1} {2}</td></tr>
";
                }
                return formatString;
            }
            set
            {
                inputTypeWithFormatStringHashtable[EInputTypeUtils.GetValue(EInputType.Text)] = value;
            }
        }

        private string additionalAttributes;
        public string AdditionalAttributes
        {
            get
            {
                return additionalAttributes;
            }
            set
            {
                additionalAttributes = value;
            }
        }

        protected virtual string GetFormatString(EInputType inputType)
        {
            string formatString = string.Empty;
            if (inputType == EInputType.TextEditor)
            {
                formatString = this.FormatTextEditor;
            }
            else if (inputType == EInputType.Image)
            {
                formatString = this.FormatImage;
            }
            else
            {
                formatString = this.FormatDefault;
            }
            return formatString;
        }

	}
}
