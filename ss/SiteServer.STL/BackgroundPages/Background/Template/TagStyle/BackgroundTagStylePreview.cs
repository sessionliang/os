using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.STL.Parser.StlElement;
using SiteServer.STL.Parser;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundTagStylePreview : BackgroundBasePage
	{
        public Literal ltlStyleName;
        public Literal ltlElement;
        public Literal ltlForm;

        private TagStyleInfo styleInfo;

        public string GetTemplateUrl()
        {
            string urlTemplate = PageUtils.GetSTLUrl(string.Format("background_tagStyleTemplate.aspx?PublishmentSystemID={0}&StyleID={1}&ReturnUrl={2}", base.PublishmentSystemID, this.styleInfo.StyleID, PageUtils.UrlEncode(base.GetQueryString("ReturnUrl"))));
            return string.Format("location.href='{0}';return false;", urlTemplate);
        }

        public string GetEditUrl()
        {
            return TagStyleUtility.GetOpenWindowStringToEdit(this.styleInfo.ElementName, base.PublishmentSystemID, this.styleInfo.StyleID);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("StyleID");

            int styleID = base.GetIntQueryString("StyleID");
            this.styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(styleID);

			if (!IsPostBack)
			{
                this.ltlStyleName.Text = this.styleInfo.StyleName;

                string stlElement = string.Format(@"<{0} styleName=""{1}""></{0}>", this.styleInfo.ElementName, this.styleInfo.StyleName);
                if (StringUtils.EqualsIgnoreCase(this.styleInfo.ElementName, StlGovInteractApply.ElementName))
                {
                    int nodeID = DataProvider.GovInteractChannelDAO.GetNodeIDByApplyStyleID(this.styleInfo.StyleID);
                    string nodeName = NodeManager.GetNodeName(base.PublishmentSystemID, nodeID);
                    this.ltlStyleName.Text = nodeName;
                    stlElement = string.Format(@"<{0} interactName=""{1}""></{0}>", this.styleInfo.ElementName, nodeName);
                }
                else if (StringUtils.EqualsIgnoreCase(this.styleInfo.ElementName, StlGovInteractQuery.ElementName))
                {
                    int nodeID = DataProvider.GovInteractChannelDAO.GetNodeIDByQueryStyleID(this.styleInfo.StyleID);
                    string nodeName = NodeManager.GetNodeName(base.PublishmentSystemID, nodeID);
                    this.ltlStyleName.Text = nodeName;
                    stlElement = string.Format(@"<{0} interactName=""{1}""></{0}>", this.styleInfo.ElementName, nodeName);
                }

                this.ltlElement.Text = StringUtils.HtmlEncode(stlElement);

                this.ltlForm.Text = SiteServer.STL.Parser.StlParserManager.ParsePreviewContent(base.PublishmentSystemInfo, stlElement);
			}
		}
	}
}
