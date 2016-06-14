using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CRM.Core;
using SiteServer.CRM.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core.AuxiliaryTable;
using System.Text;

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages.Modal
{
	public class ApplyFlow : BackgroundBasePage
	{
        public Literal ltlFlows;

        private int applyID;

        public static string GetShowPopWinString(int projectID, int applyID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            arguments.Add("ApplyID", applyID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("Á÷¶¯¹ì¼£", "modal_applyFlow.aspx", arguments, 300, 500, true);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.applyID = TranslateUtils.ToInt(base.Request.QueryString["ApplyID"]);

			if (!IsPostBack)
			{
                if (this.applyID > 0)
                {
                    ArrayList logInfoArrayList = DataProvider.ProjectLogDAO.GetLogInfoArrayList(this.applyID);
                    StringBuilder builder = new StringBuilder();

                    int count = logInfoArrayList.Count;
                    int i = 1;
                    foreach (ProjectLogInfo logInfo in logInfoArrayList)
                    {
                        if (logInfo.DepartmentID > 0)
                        {
                            builder.AppendFormat(@"<div class=""flowItem""> {0} {1}<br />{2} </div>", DepartmentManager.GetDepartmentName(logInfo.DepartmentID), EProjectLogTypeUtils.GetText(logInfo.LogType), DateUtils.GetDateAndTimeString(logInfo.AddDate));
                        }
                        else
                        {
                            builder.AppendFormat(@"<div class=""flowItem""> {0}<br />{1} </div>", EProjectLogTypeUtils.GetText(logInfo.LogType), DateUtils.GetDateAndTimeString(logInfo.AddDate));
                        }
                        if (i++ < count) builder.Append(@"<img src=""../pic/flow.gif"" />");
                    }
                    this.ltlFlows.Text = builder.ToString();
                }
			}
		}
	}
}
