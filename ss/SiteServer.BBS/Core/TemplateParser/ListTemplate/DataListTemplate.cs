using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.BBS.Core.TemplateParser.Model;

namespace SiteServer.BBS.Core.TemplateParser.ListTemplate
{
    public class DataListTemplate : ITemplate
	{
	    readonly string templateString;
        readonly string separatorRepeatTemplate;
        readonly int separatorRepeat;
	    readonly EContextType contextType;
	    readonly ContextInfo contextInfo;
	    readonly PageInfo pageInfo;
        private int i = 0;

        public DataListTemplate(string templateString, string separatorRepeatTemplate, int separatorRepeat, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfo)
		{
			this.templateString = templateString;
            this.separatorRepeatTemplate = separatorRepeatTemplate;
            this.separatorRepeat = separatorRepeat;
            this.contextType = contextType;
            this.contextInfo = contextInfo;
            this.pageInfo = pageInfo;
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
			DataListItem container = (DataListItem) noTagText.NamingContainer;

            DbItemInfo itemInfo = new DbItemInfo(container.DataItem, container.ItemIndex);

            if (this.contextType == EContextType.Forum)
            {
                this.pageInfo.ForumItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetForumsItemTemplateString(this.templateString, container.ClientID, this.pageInfo, contextType, this.contextInfo);
            }
            else if (this.contextType == EContextType.Thread)
            {
                this.pageInfo.ThreadItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetThreadsItemTemplateString(this.templateString, container.ClientID, this.pageInfo, contextType, this.contextInfo);
            }
            else if (this.contextType == EContextType.Post)
            {
                this.pageInfo.PostItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetPostsTemplateString(this.templateString, container.ClientID, this.pageInfo, contextType, this.contextInfo);
            }
            else if (this.contextType == EContextType.SqlContent)
            {
                this.pageInfo.SqlItems.Push(itemInfo);
                noTagText.Text = TemplateUtility.GetSqlContentsTemplateString(this.templateString, container.ClientID, this.pageInfo, contextType, this.contextInfo);
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
