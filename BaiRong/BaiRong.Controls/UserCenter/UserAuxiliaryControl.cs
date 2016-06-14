using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using BaiRong.Core;
using System.Text;

namespace BaiRong.Controls
{
	public class UserAuxiliaryControl : Control
	{
		private NameValueCollection formCollection;
        private bool isEdit;
        private bool isPostBack;
        Hashtable inputTypeWithFormatStringHashtable = new Hashtable();
        ArrayList excludeAttributeNames = new ArrayList();
        private string groupSN;

        public void SetParameters(NameValueCollection formCollection, string groupSN, bool isEdit, bool isPostBack)
        {
            this.formCollection = formCollection;
            this.groupSN = groupSN;
            this.isEdit = isEdit;
            this.isPostBack = isPostBack;
        }

        public void AddExcludeAttributeNames(ArrayList arraylist)
        {
            this.excludeAttributeNames.AddRange(arraylist);
        }

		protected override void Render(HtmlTextWriter output)
		{
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

            ArrayList styleInfoArrayList = TableStyleManager.GetUserTableStyleInfoArrayList(this.groupSN);

            if (styleInfoArrayList != null)
			{
                StringBuilder builder = new StringBuilder();
                NameValueCollection pageScripts = new NameValueCollection();
                foreach (TableStyleInfo styleInfo in styleInfoArrayList)
				{
                    if (styleInfo.IsVisible)
                    {
                        if (!this.excludeAttributeNames.Contains(styleInfo.AttributeName.ToLower()))
                        {
                            if (styleInfo.IsVisible)
                            {
                                string inputHtml = TableInputParser.Parse(styleInfo, styleInfo.AttributeName, this.formCollection, this.isEdit, isPostBack, this.AdditionalAttributes, pageScripts, true);
                                string helpText = styleInfo.HelpText;
                                if (!string.IsNullOrEmpty(helpText))
                                {
                                    helpText = string.Format("£¨{0}£©", styleInfo.HelpText);
                                }
                                builder.AppendFormat(this.GetFormatString(styleInfo.InputType), styleInfo.DisplayName, inputHtml, helpText);
                            }
                        }
                    }
				}

                foreach (string key in pageScripts.Keys)
                {
                    output.Write(pageScripts[key]);
                }

                output.Write(builder.ToString());

                //foreach (string key in pageScripts.Keys)
                //{
                //    base.Page.RegisterStartupScript(key, pageScripts[key]);
                //}
			}
		}

        public string FormatTextEditor
        {
            get
            {
                string formatString = inputTypeWithFormatStringHashtable[EInputTypeUtils.GetValue(EInputType.TextEditor)] as string;
                if (string.IsNullOrEmpty(formatString))
                {
                    formatString = @"
<tr><td colspan=""2"">{0}£º</td></tr>
<tr><td colspan=""2"">{1} {2}</td></tr>
";
                }
                return formatString;
            }
            set
            {
                inputTypeWithFormatStringHashtable[EInputTypeUtils.GetValue(EInputType.TextEditor)] = value;
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
<tr><td>{0}£º</td><td>{1} <span class=""gray"">{2}</span></td></tr>
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
            else
            {
                formatString = this.FormatDefault;
            }
            return formatString;
        }
    }
}
