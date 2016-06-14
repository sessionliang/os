using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using BaiRong.Core.Net;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class GatherTest : BackgroundBasePage
	{
		protected DropDownList GatherUrls;
		protected Repeater ContentUrlRepeater;

		protected Button GetContentUrls;
		protected NoTagText Content;

		private string gatherRuleName;
        private bool isFileRule;

        public static string GetOpenWindowString(int publishmentSystemID, string gatherRuleName, bool isFileRule)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("GatherRuleName", gatherRuleName);
            arguments.Add("IsFileRule", isFileRule.ToString());
            return PageUtility.GetOpenWindowString("信息采集测试", "modal_gatherTest.aspx", arguments, 700, 650, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "GatherRuleName", "IsFileRule");

			this.gatherRuleName = base.GetQueryString("GatherRuleName");
            this.isFileRule = TranslateUtils.ToBool(base.GetQueryString("IsFileRule"), false);

			if (!IsPostBack)
			{
                base.InfoMessage("采集名称：" + this.gatherRuleName);

                if (this.isFileRule)
                {
                    GatherFileRuleInfo gatherFileRuleInfo = DataProvider.GatherFileRuleDAO.GetGatherFileRuleInfo(this.gatherRuleName, base.PublishmentSystemID);

                    this.GatherUrls.Items.Add(new ListItem(gatherFileRuleInfo.GatherUrl, gatherFileRuleInfo.GatherUrl));

                    this.GetContentUrls.Text = "获取内容";
                }
                else
                {
                    GatherRuleInfo gatherRuleInfo = DataProvider.GatherRuleDAO.GetGatherRuleInfo(this.gatherRuleName, base.PublishmentSystemID);

                    ArrayList gatherUrlArrayList = GatherUtility.GetGatherUrlArrayList(gatherRuleInfo);
                    foreach (string gatherUrl in gatherUrlArrayList)
                    {
                        this.GatherUrls.Items.Add(new ListItem(gatherUrl, gatherUrl));
                    }
                }				
			}
		}

		public void GatherUrls_Click(object sender, EventArgs E)
		{
            if (this.isFileRule)
            {
                GatherFileRuleInfo gatherFileRuleInfo = DataProvider.GatherFileRuleDAO.GetGatherFileRuleInfo(this.gatherRuleName, base.PublishmentSystemID);

                StringBuilder builder = new StringBuilder();
                if (gatherFileRuleInfo.IsToFile == false)
                {
                    string regexTitle = GatherUtility.GetRegexTitle(gatherFileRuleInfo.ContentTitleStart, gatherFileRuleInfo.ContentTitleEnd);
                    string regexContentExclude = GatherUtility.GetRegexString(gatherFileRuleInfo.ContentExclude);
                    string regexContent = GatherUtility.GetRegexContent(gatherFileRuleInfo.ContentContentStart, gatherFileRuleInfo.ContentContentEnd);

                    ArrayList contentAttributes = TranslateUtils.StringCollectionToArrayList(gatherFileRuleInfo.ContentAttributes);
                    NameValueCollection contentAttributesXML = TranslateUtils.ToNameValueCollection(gatherFileRuleInfo.ContentAttributesXML);

                    NameValueCollection attributes = GatherUtility.GetContentNameValueCollection(gatherFileRuleInfo.Charset, gatherFileRuleInfo.GatherUrl, string.Empty, regexContentExclude, gatherFileRuleInfo.ContentHtmlClearCollection, gatherFileRuleInfo.ContentHtmlClearTagCollection, regexTitle, regexContent, string.Empty, string.Empty, string.Empty, string.Empty, contentAttributes, contentAttributesXML);

                    ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.BackgroundContent, base.PublishmentSystemInfo.AuxiliaryTableForContent, null);
                    foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                    {
                        if (string.IsNullOrEmpty(attributes[styleInfo.AttributeName.ToLower()])) continue;
                        builder.AppendFormat("{0}： {1}<br><br>", styleInfo.DisplayName, attributes[styleInfo.AttributeName.ToLower()]);
                    }
                }
                else
                {
                    try
                    {
                        string fileContent = WebClientUtils.GetRemoteFileSource(gatherFileRuleInfo.GatherUrl, gatherFileRuleInfo.Charset, string.Empty);

                        builder.Append(StringUtils.HtmlEncode(fileContent));
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, ex.Message);
                    }
                }

                this.Content.Text = builder.ToString();
            }
            else
            {
                string gatherUrl = this.GatherUrls.SelectedValue;
                StringBuilder errorBuilder = new StringBuilder();
                GatherRuleInfo gatherRuleInfo = DataProvider.GatherRuleDAO.GetGatherRuleInfo(this.gatherRuleName, base.PublishmentSystemID);

                string regexUrlInclude = GatherUtility.GetRegexString(gatherRuleInfo.UrlInclude);
                string regexListArea = GatherUtility.GetRegexArea(gatherRuleInfo.ListAreaStart, gatherRuleInfo.ListAreaEnd);

                ArrayList contentUrlArrayList = GatherUtility.GetContentUrls(gatherUrl, gatherRuleInfo.Charset, gatherRuleInfo.CookieString, regexListArea, regexUrlInclude, errorBuilder);

                this.ContentUrlRepeater.DataSource = contentUrlArrayList;
                this.ContentUrlRepeater.ItemDataBound += new RepeaterItemEventHandler(ContentUrlRepeater_ItemDataBound);
                this.ContentUrlRepeater.DataBind();

                base.InfoMessage(string.Format("采集名称：{0}&nbsp;&nbsp;内容数：{1}", this.gatherRuleName, contentUrlArrayList.Count));

                this.Content.Text = string.Empty;
            }
		}

		public void GetContent_Click(object sender, EventArgs e)
		{
			Button getContent = (Button)sender;
			string contentUrl = getContent.CommandArgument;

            GatherRuleInfo gatherRuleInfo = DataProvider.GatherRuleDAO.GetGatherRuleInfo(this.gatherRuleName, base.PublishmentSystemID);

			string regexContentExclude = GatherUtility.GetRegexString(gatherRuleInfo.ContentExclude);
			string regexChannel = GatherUtility.GetRegexChannel(gatherRuleInfo.ContentChannelStart, gatherRuleInfo.ContentChannelEnd);
			string regexContent = GatherUtility.GetRegexContent(gatherRuleInfo.ContentContentStart, gatherRuleInfo.ContentContentEnd);
            string regexContent2 = string.Empty;
            if (!string.IsNullOrEmpty(gatherRuleInfo.Additional.ContentContentStart2) && !string.IsNullOrEmpty(gatherRuleInfo.Additional.ContentContentEnd2))
            {
                regexContent2 = GatherUtility.GetRegexContent(gatherRuleInfo.Additional.ContentContentStart2, gatherRuleInfo.Additional.ContentContentEnd2);
            }
            string regexContent3 = string.Empty;
            if (!string.IsNullOrEmpty(gatherRuleInfo.Additional.ContentContentStart3) && !string.IsNullOrEmpty(gatherRuleInfo.Additional.ContentContentEnd3))
            {
                regexContent3 = GatherUtility.GetRegexContent(gatherRuleInfo.Additional.ContentContentStart3, gatherRuleInfo.Additional.ContentContentEnd3);
            }
			string regexNextPage = GatherUtility.GetRegexUrl(gatherRuleInfo.ContentNextPageStart, gatherRuleInfo.ContentNextPageEnd);
			string regexTitle = GatherUtility.GetRegexTitle(gatherRuleInfo.ContentTitleStart, gatherRuleInfo.ContentTitleEnd);
            ArrayList contentAttributes = TranslateUtils.StringCollectionToArrayList(gatherRuleInfo.ContentAttributes);
            NameValueCollection contentAttributesXML = TranslateUtils.ToNameValueCollection(gatherRuleInfo.ContentAttributesXML);

            NameValueCollection attributes = GatherUtility.GetContentNameValueCollection(gatherRuleInfo.Charset, contentUrl, gatherRuleInfo.CookieString, regexContentExclude, gatherRuleInfo.ContentHtmlClearCollection, gatherRuleInfo.ContentHtmlClearTagCollection, regexTitle, regexContent, regexContent2, regexContent3, regexNextPage, regexChannel, contentAttributes, contentAttributesXML);

            StringBuilder builder = new StringBuilder();

            ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, base.PublishmentSystemID);

            ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.BackgroundContent, base.PublishmentSystemInfo.AuxiliaryTableForContent, relatedIdentities);
            foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
            {
                if (string.IsNullOrEmpty(attributes[styleInfo.AttributeName.ToLower()])) continue;
                if (StringUtils.EqualsIgnoreCase(BaiRong.Model.ContentAttribute.Title, styleInfo.AttributeName))
                {
                    builder.AppendFormat(@"<a href=""{0}"" target=""_blank"">{1}： {2}</a><br><br>", contentUrl, styleInfo.DisplayName, attributes[styleInfo.AttributeName.ToLower()]);
                }
                else if (StringUtils.EqualsIgnoreCase(BackgroundContentAttribute.ImageUrl, styleInfo.AttributeName) || styleInfo.InputType == EInputType.Image)
                {
                    string imageUrl = PageUtils.GetUrlByBaseUrl(attributes[styleInfo.AttributeName.ToLower()], contentUrl);
                    builder.AppendFormat("{0}： <img src='{1}' /><br><br>", styleInfo.DisplayName, imageUrl);
                }
                else
                {
                    builder.AppendFormat("{0}： {1}<br><br>", styleInfo.DisplayName, attributes[styleInfo.AttributeName.ToLower()]);
                }
            }

			this.Content.Text = builder.ToString();
		}

		private static void ContentUrlRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			string contentUrl = e.Item.DataItem as string;
            NoTagText contentItem = (NoTagText)e.Item.FindControl("ContentItem");
            Button getContent = (Button)e.Item.FindControl("GetContent");
			getContent.CommandArgument = contentUrl;

			contentItem.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtils.AddProtocolToUrl(contentUrl), contentUrl);
		}
	}
}
