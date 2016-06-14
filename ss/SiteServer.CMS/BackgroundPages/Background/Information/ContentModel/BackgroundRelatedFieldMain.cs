using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System.Web.UI;
using System.Text;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundRelatedFieldMain : BackgroundBasePage
	{
        public Literal ltlFrames;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                int relatedFieldID = TranslateUtils.ToInt(base.GetQueryString("RelatedFieldID"));
                int totalLevel = TranslateUtils.ToInt(base.GetQueryString("TotalLevel"));
                string cols = "100%";
                if (totalLevel == 2)
                {
                    cols = "50%,50%";
                }
                else if (totalLevel == 3)
                {
                    cols = "33%,33%,33%";
                }
                else if (totalLevel == 4)
                {
                    cols = "25%,25%,25%,25%";
                }
                else if (totalLevel == 5)
                {
                    cols = "20%,20%,20%,20%,20%";
                }
                StringBuilder builder = new StringBuilder();
                string urlItem = PageUtils.GetCMSUrl(string.Format("background_relatedFieldItem.aspx?PublishmentSystemID={0}&RelatedFieldID={1}&Level=1", base.PublishmentSystemID, relatedFieldID));
                builder.AppendFormat(@"
<frameset framespacing=""0"" border=""false"" cols=""{0}"" frameborder=""0"" scrolling=""yes"">
	<frame name=""level1"" scrolling=""auto"" marginwidth=""0"" marginheight=""0"" src=""{1}"" >
", cols, urlItem);

                for (int i = 2; i <= totalLevel; i++)
                {
                    builder.AppendFormat(@"
<frame name=""level{0}"" scrolling=""auto"" marginwidth=""0"" marginheight=""0"" src=""background_blank.html"">
", i);
                }

                builder.Append("</frameset>");

                this.ltlFrames.Text = builder.ToString();
			}
		}
	}
}
