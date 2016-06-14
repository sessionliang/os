using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser.StlElement;

namespace SiteServer.STL.Parser.ListTemplate
{
	public class RepeaterTemplate : ITemplate
	{
	    readonly string templateString;
	    readonly LowerNameValueCollection selectedItems;
	    readonly LowerNameValueCollection selectedValues;
        readonly string separatorRepeatTemplate;
        readonly int separatorRepeat;
	    readonly PageInfo pageInfo;
        readonly EContextType contextType;
        readonly ContextInfo contextInfo;
        private int i = 0;

        public RepeaterTemplate(string templateString, LowerNameValueCollection selectedItems, LowerNameValueCollection selectedValues, string separatorRepeatTemplate, int separatorRepeat, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfo)
		{
			this.templateString = templateString;
            this.selectedItems = selectedItems;
            this.selectedValues = selectedValues;
            this.separatorRepeatTemplate = separatorRepeatTemplate;
            this.separatorRepeat = separatorRepeat;
            this.pageInfo = pageInfo;
            this.contextType = contextType;
            this.contextInfo = contextInfo;
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
			RepeaterItem container = (RepeaterItem) noTagText.NamingContainer;

            DbItemInfo itemInfo = new DbItemInfo(container.DataItem, container.ItemIndex);

            if (this.contextType == EContextType.Channel)
            {
                this.pageInfo.ChannelItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetChannelsItemTemplateString(this.templateString, this.selectedItems, this.selectedValues, container.ClientID, this.pageInfo, contextType, this.contextInfo);
            }
            else if (this.contextType == EContextType.Content)
            {
                this.pageInfo.ContentItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetContentsItemTemplateString(this.templateString, this.selectedItems, this.selectedValues, container.ClientID, this.pageInfo, contextType, this.contextInfo);
            }
            else if (this.contextType == EContextType.Comment)
            {
                this.pageInfo.CommentItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetCommentsTemplateString(this.templateString, container.ClientID, this.pageInfo, contextType, this.contextInfo);
            }
            else if (this.contextType == EContextType.InputContent)
            {
                this.pageInfo.InputItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetInputContentsTemplateString(this.templateString, container.ClientID, this.pageInfo, contextType, this.contextInfo);
            }
            else if (this.contextType == EContextType.WebsiteMessageContent)
            {
                this.pageInfo.WebsiteMessageItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetWebsiteMessageContentsTemplateString(this.templateString, container.ClientID, this.pageInfo, contextType, this.contextInfo);
            }
            else if (this.contextType == EContextType.SqlContent)
            {
                this.pageInfo.SqlItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetSqlContentsTemplateString(this.templateString, this.selectedItems, this.selectedValues, container.ClientID, this.pageInfo, contextType, this.contextInfo);
            }
            else if (this.contextType == EContextType.Site)
            {
                this.pageInfo.SiteItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetSitesTemplateString(this.templateString, container.ClientID, this.pageInfo, contextType, this.contextInfo);
            }
            else if (this.contextType == EContextType.Photo)
            {
                this.pageInfo.PhotoItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetPhotosTemplateString(this.templateString, this.selectedItems, this.selectedValues, container.ClientID, this.pageInfo, contextType, this.contextInfo);
            }
            else if (this.contextType == EContextType.Teleplay)
            {
                this.pageInfo.TeleplayItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetTeleplaysTemplateString(this.templateString, this.selectedItems, this.selectedValues, container.ClientID, this.pageInfo, contextType, this.contextInfo);
            }
            else if (this.contextType == EContextType.Each)
            {
                this.pageInfo.EachItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetEachsTemplateString(this.templateString, this.selectedItems, this.selectedValues, container.ClientID, this.pageInfo, contextType, this.contextInfo);
            }
            else if (this.contextType == EContextType.Spec)
            {
                this.pageInfo.SpecItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetSpecsTemplateString(this.templateString, this.selectedItems, this.selectedValues, container.ClientID, this.pageInfo, contextType, this.contextInfo);
            }
            else if (this.contextType == EContextType.Filter)
            {
                this.pageInfo.FilterItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetFiltersTemplateString(this.templateString, this.selectedItems, this.selectedValues, container.ClientID, this.pageInfo, contextType, this.contextInfo);
            }

            if (separatorRepeat > 1)
            {
                i++;
                if (i % separatorRepeat == 0)
                {
                    noTagText.Text += separatorRepeatTemplate;
                }
            }
		}
	}
}
