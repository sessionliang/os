using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core.AuxiliaryTable;
using System.Text;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WCM.Core;

namespace SiteServer.WCM.BackgroundPages.Modal
{
	public class GovInteractApplyFlow : BackgroundBasePage
	{
        public Literal ltlFlows;

        private int nodeID;
        private int contentID;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, int contentID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            return PageUtilityWCM.GetOpenWindowString("流动轨迹", "modal_govInteractApplyFlow.aspx", arguments, 300, 600, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.nodeID = TranslateUtils.ToInt(base.Request.QueryString["NodeID"]);
            this.contentID = TranslateUtils.ToInt(base.Request.QueryString["ContentID"]);

			if (!IsPostBack)
			{
                if (this.contentID > 0)
                {
                    ArrayList logInfoArrayList = DataProvider.GovInteractLogDAO.GetLogInfoArrayList(base.PublishmentSystemID, this.contentID);
                    StringBuilder builder = new StringBuilder();

                    int count = logInfoArrayList.Count;
                    int i = 1;
                    foreach (GovInteractLogInfo logInfo in logInfoArrayList)
                    {
                        if (logInfo.DepartmentID > 0)
                        {
                            builder.AppendFormat(@"<tr class=""info""><td class=""center""> {0} {1}<br />{2} </td></tr>", DepartmentManager.GetDepartmentName(logInfo.DepartmentID), EGovInteractLogTypeUtils.GetText(logInfo.LogType), DateUtils.GetDateAndTimeString(logInfo.AddDate));
                        }
                        else
                        {
                            builder.AppendFormat(@"<tr class=""info""><td class=""center""> {0}<br />{1} </td></tr>", EGovInteractLogTypeUtils.GetText(logInfo.LogType), DateUtils.GetDateAndTimeString(logInfo.AddDate));
                        }
                        if (i++ < count) builder.Append(@"<tr><td class=""center""><img src=""../pic/flow.gif"" /></td></tr>");
                    }
                    this.ltlFlows.Text = builder.ToString();
                }

				
			}
		}
	}
}
