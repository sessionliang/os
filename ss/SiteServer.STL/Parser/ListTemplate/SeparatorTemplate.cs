using System;
using System.Web.UI;
using BaiRong.Controls;

namespace SiteServer.STL.Parser.ListTemplate
{
	public class SeparatorTemplate : ITemplate
	{
	    readonly string separator = string.Empty;

		public SeparatorTemplate(string separator)
		{
			this.separator = separator;
		}

		public void InstantiateIn(Control container)
		{
			NoTagText noTagText = new NoTagText();
			noTagText.DataBinding += new EventHandler(TemplateControl_DataBinding);
			container.Controls.Add(noTagText);
		}

		private void TemplateControl_DataBinding(object sender, EventArgs e)
		{
			NoTagText noTagText = (NoTagText) sender;
			noTagText.Text = this.separator;
		}
	}
}
